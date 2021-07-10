using System;
using System.Collections.Generic;
using Server.Network;

namespace Server.Items
{
    [FlippableAttribute(0x100A /*East*/, 0x100B /*South*/)]
    public class ArcheryButte : AddonComponent
    {
        private static readonly TimeSpan UseDelay = TimeSpan.FromSeconds(2.0);

        private Dictionary<Mobile, ScoreEntry> m_Entries;

        [Constructible]
        public ArcheryButte(int itemID = 0x100A) : base(itemID)
        {
            MinSkill = -25.0;
            MaxSkill = +25.0;
        }

        public ArcheryButte(Serial serial) : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public double MinSkill { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public double MaxSkill { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime LastUse { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool FacingEast
        {
            get => ItemID == 0x100A;
            set => ItemID = value ? 0x100A : 0x100B;
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Arrows { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int Bolts { get; set; }

        public override void OnDoubleClick(Mobile from)
        {
            if ((Arrows > 0 || Bolts > 0) && from.InRange(GetWorldLocation(), 1))
            {
                Gather(from);
            }
            else
            {
                Fire(from);
            }
        }

        public void Gather(Mobile from)
        {
            from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 500592); // You gather the arrows and bolts.

            if (Arrows > 0)
            {
                from.AddToBackpack(new Arrow(Arrows));
            }

            if (Bolts > 0)
            {
                from.AddToBackpack(new Bolt(Bolts));
            }

            Arrows = 0;
            Bolts = 0;

            m_Entries = null;
        }

        private ScoreEntry GetEntryFor(Mobile from)
        {
            m_Entries ??= new Dictionary<Mobile, ScoreEntry>();

            if (!m_Entries.TryGetValue(from, out var e))
            {
                m_Entries[from] = e = new ScoreEntry();
            }

            return e;
        }

        public void Fire(Mobile from)
        {
            if (!(from.Weapon is BaseRanged bow))
            {
                SendLocalizedMessageTo(from, 500593); // You must practice with ranged weapons on this.
                return;
            }

            if (Core.Now < LastUse + UseDelay)
            {
                return;
            }

            var worldLoc = GetWorldLocation();

            if (FacingEast ? from.X <= worldLoc.X : from.Y <= worldLoc.Y)
            {
                from.LocalOverheadMessage(
                    MessageType.Regular,
                    0x3B2,
                    500596
                ); // You would do better to stand in front of the archery butte.
                return;
            }

            if (FacingEast ? from.Y != worldLoc.Y : from.X != worldLoc.X)
            {
                from.LocalOverheadMessage(
                    MessageType.Regular,
                    0x3B2,
                    500597
                ); // You aren't properly lined up with the archery butte to get an accurate shot.
                return;
            }

            if (!from.InRange(worldLoc, 6))
            {
                from.LocalOverheadMessage(
                    MessageType.Regular,
                    0x3B2,
                    500598
                ); // You are too far away from the archery butte to get an accurate shot.
                return;
            }

            if (from.InRange(worldLoc, 4))
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 500599); // You are too close to the target.
                return;
            }

            var pack = from.Backpack;
            var ammoType = bow.AmmoType;

            var isArrow = ammoType == typeof(Arrow);
            var isBolt = ammoType == typeof(Bolt);
            var isKnown = isArrow || isBolt;

            if (pack?.ConsumeTotal(ammoType) != true)
            {
                if (isArrow)
                {
                    from.LocalOverheadMessage(
                        MessageType.Regular,
                        0x3B2,
                        500594
                    ); // You do not have any arrows with which to practice.
                }
                else if (isBolt)
                {
                    from.LocalOverheadMessage(
                        MessageType.Regular,
                        0x3B2,
                        500595
                    ); // You do not have any crossbow bolts with which to practice.
                }
                else
                {
                    SendLocalizedMessageTo(from, 500593); // You must practice with ranged weapons on this.
                }

                return;
            }

            LastUse = Core.Now;

            from.Direction = from.GetDirectionTo(GetWorldLocation());
            bow.PlaySwingAnimation(from);
            from.MovingEffect(this, bow.EffectID, 18, 1, false, false);

            var se = GetEntryFor(from);

            if (!from.CheckSkill(bow.Skill, MinSkill, MaxSkill))
            {
                from.PlaySound(bow.MissSound);

                PublicOverheadMessage(MessageType.Regular, 0x3B2, 500604, from.Name); // You miss the target altogether.

                se.Record(0);

                if (se.Count == 1)
                {
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, 1062719, se.Total.ToString());
                }
                else
                {
                    PublicOverheadMessage(MessageType.Regular, 0x3B2, 1042683, $"{se.Total}\t{se.Count}");
                }

                return;
            }

            Effects.PlaySound(Location, Map, 0x2B1);

            var rand = Utility.RandomDouble();

            int area, score, splitScore;

            if (rand < 0.10)
            {
                area = 0; // bullseye
                score = 50;
                splitScore = 100;
            }
            else if (rand < 0.25)
            {
                area = 1; // inner ring
                score = 10;
                splitScore = 20;
            }
            else if (rand < 0.50)
            {
                area = 2; // middle ring
                score = 5;
                splitScore = 15;
            }
            else
            {
                area = 3; // outer ring
                score = 2;
                splitScore = 5;
            }

            var split = isKnown && (Arrows + Bolts) * 0.02 > Utility.RandomDouble();

            if (split)
            {
                PublicOverheadMessage(
                    MessageType.Regular,
                    0x3B2,
                    1010027 + area,
                    $"{from.Name}\t{(isArrow ? "arrow" : "bolt")}"
                );
            }
            else
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, 1010035 + area, from.Name);

                if (isArrow)
                {
                    ++Arrows;
                }
                else if (isBolt)
                {
                    ++Bolts;
                }
            }

            se.Record(split ? splitScore : score);

            if (se.Count == 1)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, 1062719, se.Total.ToString());
            }
            else
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, 1042683, $"{se.Total}\t{se.Count}");
            }
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);

            writer.Write(MinSkill);
            writer.Write(MaxSkill);
            writer.Write(Arrows);
            writer.Write(Bolts);
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();

            switch (version)
            {
                case 0:
                    {
                        MinSkill = reader.ReadDouble();
                        MaxSkill = reader.ReadDouble();
                        Arrows = reader.ReadInt();
                        Bolts = reader.ReadInt();

                        if (MinSkill == 0.0 && MaxSkill == 30.0)
                        {
                            MinSkill = -25.0;
                            MaxSkill = +25.0;
                        }

                        break;
                    }
            }
        }

        private class ScoreEntry
        {
            public int Total { get; set; }

            public int Count { get; set; }

            public void Record(int score)
            {
                Total += score;
                Count += 1;
            }
        }
    }

    public class ArcheryButteAddon : BaseAddon
    {
        [Constructible]
        public ArcheryButteAddon()
        {
            AddComponent(new ArcheryButte(), 0, 0, 0);
        }

        public ArcheryButteAddon(Serial serial) : base(serial)
        {
        }

        public override BaseAddonDeed Deed => new ArcheryButteDeed();

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    public class ArcheryButteDeed : BaseAddonDeed
    {
        [Constructible]
        public ArcheryButteDeed()
        {
        }

        public ArcheryButteDeed(Serial serial) : base(serial)
        {
        }

        public override BaseAddon Addon => new ArcheryButteAddon();
        public override int LabelNumber => 1024106; // archery butte

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }
}

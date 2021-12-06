using System;
using System.Collections.Generic;
using System.Linq;
using Server.Items;
using Server.Mobiles;

namespace Server.Talent
{
    public class GraveDigger : BaseTalent
    {
        private Dictionary<Point2D, DateTime> m_LastGraves;

        public GraveDigger()
        {
            m_LastGraves = new Dictionary<Point2D, DateTime>();
            DisplayName = "Grave digger";
            Description =
                "Allows you to dig up items from graveyards as well as other things! Each level improves rewards. Requires 70+ mining.";
            ImageID = 402;
            GumpHeight = 85;
            AddEndY = 105;
        }

        public override bool HasSkillRequirement(Mobile mobile) => mobile.Skills.Mining.Base >= 70;

        public void Dig(BaseHarvestTool tool, Mobile mobile, Point3D point)
        {
            var expiryTime = DateTime.Now.AddMinutes(-30);
            var expiredGraves = m_LastGraves.Where(g => g.Value < expiryTime).ToList();
            foreach (KeyValuePair<Point2D, DateTime> entry in expiredGraves)
            {
                m_LastGraves.Remove(entry.Key);
            }

            bool canDig = m_LastGraves.All(entry => !mobile.InRange(entry.Key, 3));
            if (canDig && mobile.BeginAction(tool))
            {
                new DigTimer(mobile, tool, point, this).Start();
            }
            else
            {
                mobile.SendMessage("You cannot dig here right now");
            }
        }

        private class DigTimer : Timer
        {
            private readonly Mobile m_From;
            private readonly BaseHarvestTool m_Tool;
            private readonly long m_LastMoveTime;
            private readonly long m_NextActionTime;
            private readonly long m_NextSpellTime;
            private readonly GraveDigger m_GraveDigger;
            private int m_Count;
            private TreasureChestDirt m_Dirt1;
            private TreasureChestDirt m_Dirt2;
            private GraveDiggerBone m_Bone1;
            private GraveDiggerBone m_Bone2;

            private readonly Point3D m_Location;

            public DigTimer(Mobile from, BaseHarvestTool tool, Point3D location, GraveDigger graveDigger) : base(
                TimeSpan.Zero,
                TimeSpan.FromSeconds(1.0)
            )
            {
                m_From = from;
                m_Location = location;
                m_Tool = tool;
                m_NextSpellTime = from.NextSpellTime;
                m_NextActionTime = from.NextActionTime;
                m_LastMoveTime = from.LastMoveTime;
                m_GraveDigger = graveDigger;
            }

            private void Terminate(bool delete = false)
            {
                m_GraveDigger.m_LastGraves.Add(new Point2D(m_Location.X, m_Location.Y), DateTime.Now);
                Stop();
                m_From.EndAction(m_Tool);
                if (delete)
                {
                    m_Tool.Delete();
                }

                if (m_Dirt1 != null)
                {
                    m_Dirt1.Delete();
                    m_Dirt2.Delete();
                }
            }

            protected override void OnTick()
            {
                if (m_Tool.UsesRemaining == 1)
                {
                    Terminate(true);
                }

                if (m_NextSpellTime != m_From.NextSpellTime ||
                    m_NextActionTime != m_From.NextActionTime)
                {
                    Terminate();
                    return;
                }

                if (m_LastMoveTime != m_From.LastMoveTime)
                {
                    m_From.SendMessage("You cannot move around while digging up graves.");
                    Terminate();
                    return;
                }

                m_Count++;

                m_From.RevealingAction();
                m_From.Direction = m_From.GetDirectionTo(m_Location);

                if (m_From.Body.IsHuman && !m_From.Mounted)
                {
                    m_From.Animate(11, 5, 1, true, false, 0);
                }

                if (m_Count > 1 && m_Dirt1 == null)
                {
                    m_Dirt1 = new TreasureChestDirt();
                    m_Dirt1.MoveToWorld(m_Location, m_From.Map);

                    m_Dirt2 = new TreasureChestDirt();
                    m_Dirt2.MoveToWorld(new Point3D(m_Location.X, m_Location.Y - 1, m_Location.Z), m_From.Map);
                }

                if (m_Count > 5 && m_Bone1 is null)
                {
                    m_Bone1 = new GraveDiggerBone();
                    m_Bone1.MoveToWorld(new Point3D(m_Location.X, m_Location.Y - 1, m_Location.Z), m_From.Map);
                }

                if (m_Count > 10 && m_Bone2 is null)
                {
                    m_Bone2 = new GraveDiggerBone();
                    m_Bone2.MoveToWorld(new Point3D(m_Location.X, m_Location.Y + 1, m_Location.Z), m_From.Map);
                }

                new SoundTimer(m_From, 0x125 + m_Count % 2).Start();

                if (Utility.Random(100) < 10)
                {
                    // spawn undead
                    BaseCreature undead = Utility.RandomMinMax(1, 7 + m_GraveDigger.Level) switch
                    {
                        1  => new Shade(),
                        2  => new Spectre(),
                        3  => new Wraith(),
                        4  => new Skeleton(),
                        5  => new Zombie(),
                        6  => new Ghoul(),
                        7  => new RottingCorpse(),
                        8  => new SkeletalKnight(),
                        9  => new BoneKnight(),
                        10 => new BoneMagi(),
                        11 => new SkeletalMage(),
                        12 => new Lich(),
                        _  => null
                    };

                    if (undead != null)
                    {
                        Effects.PlaySound(m_From.Location, m_From.Map, 0x1FB);
                        Effects.SendLocationParticles(
                            EffectItem.Create(undead.Location, undead.Map, EffectItem.DefaultDuration),
                            0x37CC,
                            1,
                            40,
                            97,
                            3,
                            9917,
                            0
                        );
                        undead.MoveToWorld(m_Location, m_From.Map);
                        Terminate();
                    }
                }
                else if (Utility.Random(100) < m_GraveDigger.Level)
                {
                    if (Utility.RandomBool())
                    {
                        var brigand = new Brigand();
                        brigand.MoveToWorld(m_Location, m_From.Map);
                        m_From.SendMessage("You dig up a murderer who was buried alive.");
                        Terminate();
                    }
                    else
                    {
                        if (m_From.Backpack != null)
                        {
                            if (Utility.Random(500) < 1)
                            {
                                m_From.Backpack.AddItem(new RuneScroll());
                            }
                            else
                            {
                                switch (Utility.RandomMinMax(1, 12))
                                {
                                    case 1:
                                        var gem = Loot.RandomGem();
                                        m_From.Backpack.AddItem(gem);
                                        break;
                                    case 2:
                                        var gold = new Gold(Utility.Random(m_GraveDigger.Level));
                                        m_From.Backpack.AddItem(gold);
                                        break;
                                    case 3:
                                        var jewelry = Loot.RandomJewelry();
                                        m_From.Backpack.AddItem(jewelry);
                                        break;
                                    case 4:
                                        var book = Loot.RandomLibraryBook();
                                        m_From.Backpack.AddItem(book);
                                        break;
                                    case 5:
                                        var reagent = Loot.RandomNecromancyReagent();
                                        m_From.Backpack.AddItem(reagent);
                                        break;
                                    case 6:
                                        var potion = Loot.RandomPotion();
                                        m_From.Backpack.AddItem(potion);
                                        break;
                                    case 7:
                                        var scroll = Loot.RandomScroll(0, 15, SpellbookType.Necromancer);
                                        if (Utility.RandomBool())
                                        {
                                            scroll = Loot.RandomScroll(0, 63, SpellbookType.Regular);
                                        }

                                        m_From.Backpack.AddItem(scroll);
                                        break;
                                    case 8:
                                        var weapon = Loot.RandomWeapon();
                                        m_From.Backpack.AddItem(weapon);
                                        break;
                                    case 9:
                                        var wand = Loot.RandomWand();
                                        m_From.Backpack.AddItem(wand);
                                        break;
                                    case 10:
                                        var clothing = Loot.RandomClothing();
                                        m_From.Backpack.AddItem(clothing);
                                        break;
                                    case 11:
                                        var hat = Loot.RandomHat();
                                        m_From.Backpack.AddItem(hat);
                                        break;
                                    case 12:
                                        var shield = Loot.RandomShield();
                                        m_From.Backpack.AddItem(shield);
                                        break;
                                }
                            }

                            if (Utility.RandomBool())
                            {
                                Terminate();
                            }

                            m_From.SendMessage("You dig up an item from the grave.");
                        }
                    }
                }

                m_Tool.UsesRemaining--;
            }

            private class SoundTimer : Timer
            {
                private readonly Mobile m_From;
                private readonly int m_SoundID;

                public SoundTimer(Mobile from, int soundID) : base(TimeSpan.FromSeconds(0.9))
                {
                    m_From = from;
                    m_SoundID = soundID;
                }

                protected override void OnTick()
                {
                    m_From.PlaySound(m_SoundID);
                }
            }
        }
    }
    public sealed class GraveDiggerBone : Item
    {
        public GraveDiggerBone() : base(0x0ECA)
        {
            Movable = false;
            ItemID = Utility.RandomMinMax(1, 8) switch
            {
                1 => 0x0ECA,
                2 => 0x0ECB,
                3 => 0x0ECC,
                4 => 0x0ECD,
                5 => 0x0ECE,
                6 => 0x0ED0,
                7 => 0x0ED1,
                8 => 0x0ED2,
                _ => ItemID
            };

            Timer.StartTimer(TimeSpan.FromMinutes(2.0), Delete);
        }

        public GraveDiggerBone(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadEncodedInt();

            Delete();
        }
    }
}

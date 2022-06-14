using System;
using Server.Factions;
using Server.Gumps;
using Server.Misc;
using Server.Mobiles;
using Server.Network;
using Server.Regions;

namespace Server.Items
{
    [DispellableField]
    public class Moongate : Item
    {
        [Constructible]
        public Moongate(bool dispellable = true) : this(Point3D.Zero, null, dispellable)
        {
        }

        [Constructible]
        public Moongate(Point3D target, Map targetMap = null, bool dispellable = true) : base(0xF6C)
        {
            Movable = false;
            Light = LightType.Circle300;

            Target = target;
            TargetMap = targetMap;
            Dispellable = dispellable;
        }

        public Moongate(Serial serial) : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public Point3D Target { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public Map TargetMap { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public bool Dispellable { get; set; }

        public virtual bool ShowFeluccaWarning => false;

        public override void OnDoubleClick(Mobile from)
        {
            if (!from.Player)
            {
                return;
            }

            if (from.InRange(GetWorldLocation(), 1))
            {
                CheckGate(from, 1);
            }
            else
            {
                from.SendLocalizedMessage(500446); // That is too far away.
            }
        }

        public override bool OnMoveOver(Mobile m)
        {
            if (m.Player)
            {
                CheckGate(m, 0);
            }

            return true;
        }

        public virtual void CheckGate(Mobile m, int range)
        {
            if (m.Hidden && m.AccessLevel == AccessLevel.Player && Core.ML)
            {
                m.RevealingAction();
            }

            new DelayTimer(m, this, range).Start();
        }

        public virtual void OnGateUsed(Mobile m)
        {
            PlanarTravel.NextPlanarTravel(m);
        }

        public virtual void UseGate(Mobile m)
        {
            var flags = m.NetState?.Flags ?? ClientFlags.None;

            if (!PlanarTravel.CanPlanarTravel(m)) {
                m.LocalOverheadMessage(MessageType.Regular, 0x22, false, PlanarTravel.NO_TRAVEL_MESSAGE);
            }
            else if (Sigil.ExistsOn(m))
            {
                m.SendLocalizedMessage(1061632); // You can't do that while carrying the sigil.
            }
            else if (TargetMap == Map.Felucca && m is PlayerMobile mobile && mobile.Young)
            {
                mobile.SendLocalizedMessage(1049543); // You decide against traveling to Felucca while you are still young.
            }
            else if (m.Kills >= 5 && TargetMap != Map.Felucca ||
                     TargetMap == Map.Tokuno && (flags & ClientFlags.Tokuno) == 0 ||
                     TargetMap == Map.Malas && (flags & ClientFlags.Malas) == 0 ||
                     TargetMap == Map.Ilshenar && (flags & ClientFlags.Ilshenar) == 0)
            {
                m.SendLocalizedMessage(1019004); // You are not allowed to travel there.
            }
            else if (m.Spell != null)
            {
                m.SendLocalizedMessage(1049616); // You are too busy to do that at the moment.
            }
            else if (TargetMap != null && TargetMap != Map.Internal)
            {
                BaseCreature.TeleportPets(m, Target, TargetMap);

                m.MoveToWorld(Target, TargetMap);

                if (m.AccessLevel == AccessLevel.Player || !m.Hidden)
                {
                    m.PlaySound(0x1FE);
                }

                OnGateUsed(m);
            }
            else
            {
                m.SendMessage("This moongate does not seem to go anywhere.");
            }
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version

            writer.Write(Target);
            writer.Write(TargetMap);

            // Version 1
            writer.Write(Dispellable);
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();

            Target = reader.ReadPoint3D();
            TargetMap = reader.ReadMap();

            if (version >= 1)
            {
                Dispellable = reader.ReadBool();
            }
        }

        public virtual bool ValidateUse(Mobile from, bool message)
        {
            if (from.Deleted || Deleted)
            {
                return false;
            }

            if (from.Map != Map || !from.InRange(this, 1))
            {
                if (message)
                {
                    from.SendLocalizedMessage(500446); // That is too far away.
                }

                return false;
            }

            return true;
        }

        public virtual void BeginConfirmation(Mobile from)
        {
            if (IsInTown(from.Location, from.Map) && !IsInTown(Target, TargetMap) ||
                from.Map != Map.Felucca && TargetMap == Map.Felucca && ShowFeluccaWarning)
            {
                if (from.AccessLevel == AccessLevel.Player || !from.Hidden)
                {
                    from.SendSound(0x20E, from);
                }

                from.CloseGump<MoongateConfirmGump>();
                from.SendGump(new MoongateConfirmGump(from, this));
            }
            else
            {
                EndConfirmation(from);
            }
        }

        public virtual void EndConfirmation(Mobile from)
        {
            if (!ValidateUse(from, true))
            {
                return;
            }

            UseGate(from);
        }

        public virtual void DelayCallback(Mobile from, int range)
        {
            if (!ValidateUse(from, false) || !from.InRange(this, range))
            {
                return;
            }

            if (TargetMap != null)
            {
                BeginConfirmation(from);
            }
            else
            {
                from.SendMessage("This moongate does not seem to go anywhere.");
            }
        }

        public static bool IsInTown(Point3D p, Map map) =>
            map != null && Region.Find(p, map).GetRegion<GuardedRegion>()?.IsDisabled() == false;

        private class DelayTimer : Timer
        {
            private readonly Mobile m_From;
            private readonly Moongate m_Gate;
            private readonly int m_Range;

            public DelayTimer(Mobile from, Moongate gate, int range) : base(TimeSpan.FromSeconds(1.0))
            {
                m_From = from;
                m_Gate = gate;
                m_Range = range;
            }

            protected override void OnTick()
            {
                m_Gate.DelayCallback(m_From, m_Range);
            }
        }
    }

    public class ConfirmationMoongate : Moongate
    {
        [Constructible]
        public ConfirmationMoongate() : this(Point3D.Zero)
        {
        }

        [Constructible]
        public ConfirmationMoongate(Point3D target, Map targetMap = null) : base(target, targetMap)
        {
        }

        public ConfirmationMoongate(Serial serial) : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int GumpWidth { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int GumpHeight { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int TitleColor { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int MessageColor { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public int TitleNumber { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public TextDefinition Message { get; set; }

        public virtual void Warning_Callback(Mobile from, bool okay)
        {
            if (okay)
            {
                EndConfirmation(from);
            }
        }

        public override void BeginConfirmation(Mobile from)
        {
            if (GumpWidth > 0 && GumpHeight > 0 && TitleNumber > 0 && Message?.IsEmpty == false)
            {
                from.CloseGump<WarningGump>();
                from.SendGump(
                    new WarningGump(
                        TitleNumber,
                        TitleColor,
                        Message,
                        MessageColor,
                        GumpWidth,
                        GumpHeight,
                        okay => Warning_Callback(from, okay)
                    )
                );
            }
            else
            {
                base.BeginConfirmation(from);
            }
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version

            TextDefinition.Serialize(writer, Message);

            writer.WriteEncodedInt(GumpWidth);
            writer.WriteEncodedInt(GumpHeight);

            writer.WriteEncodedInt(TitleColor);
            writer.WriteEncodedInt(MessageColor);

            writer.WriteEncodedInt(TitleNumber);
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        Message = TextDefinition.Deserialize(reader);
                        goto case 0;
                    }
                case 0:
                    {
                        GumpWidth = reader.ReadEncodedInt();
                        GumpHeight = reader.ReadEncodedInt();

                        TitleColor = reader.ReadEncodedInt();
                        MessageColor = reader.ReadEncodedInt();

                        TitleNumber = reader.ReadEncodedInt();

                        if (version == 0)
                        {
                            var number = reader.ReadEncodedInt();
                            var message = reader.ReadString();
                            Message = number > 0 ? number : message;
                        }

                        break;
                    }
            }
        }
    }

    public class MoongateConfirmGump : Gump
    {
        private readonly Mobile m_From;
        private readonly Moongate m_Gate;

        public MoongateConfirmGump(Mobile from, Moongate gate) : base(Core.AOS ? 110 : 20, Core.AOS ? 100 : 30)
        {
            m_From = from;
            m_Gate = gate;

            if (Core.AOS)
            {
                Closable = false;

                AddPage(0);

                AddBackground(0, 0, 420, 280, 5054);

                AddImageTiled(10, 10, 400, 20, 2624);
                AddAlphaRegion(10, 10, 400, 20);

                AddHtmlLocalized(10, 10, 400, 20, 1062051, 30720); // Gate Warning

                AddImageTiled(10, 40, 400, 200, 2624);
                AddAlphaRegion(10, 40, 400, 200);

                if (from.Map != Map.Felucca && gate.TargetMap == Map.Felucca && gate.ShowFeluccaWarning)
                {
                    AddHtmlLocalized(
                        10,
                        40,
                        400,
                        200,
                        1062050, // This Gate goes to Felucca... Continue to enter the gate, Cancel to stay here
                        32512,
                        false,
                        true
                    );
                }
                else
                {
                    AddHtmlLocalized(
                        10,
                        40,
                        400,
                        200,
                        1062049, // Dost thou wish to step into the moongate? Continue to enter the gate, Cancel to stay here
                        32512,
                        false,
                        true
                    );
                }

                AddImageTiled(10, 250, 400, 20, 2624);
                AddAlphaRegion(10, 250, 400, 20);

                AddButton(10, 250, 4005, 4007, 1);
                AddHtmlLocalized(40, 250, 170, 20, 1011036, 32767); // OKAY

                AddButton(210, 250, 4005, 4007, 0);
                AddHtmlLocalized(240, 250, 170, 20, 1011012, 32767); // CANCEL
            }
            else
            {
                AddPage(0);

                AddBackground(0, 0, 420, 400, 5054);
                AddBackground(10, 10, 400, 380, 3000);

                AddHtml(
                    20,
                    40,
                    380,
                    60,
                    "Dost thou wish to step into the moongate? Continue to enter the gate, Cancel to stay here"
                );

                AddHtmlLocalized(55, 110, 290, 20, 1011012); // CANCEL
                AddButton(20, 110, 4005, 4007, 0);

                AddHtmlLocalized(55, 140, 290, 40, 1011011); // CONTINUE
                AddButton(20, 140, 4005, 4007, 1);
            }
        }

        public override void OnResponse(NetState state, RelayInfo info)
        {
            if (info.ButtonID == 1)
            {
                m_Gate.EndConfirmation(m_From);
            }
        }
    }
}

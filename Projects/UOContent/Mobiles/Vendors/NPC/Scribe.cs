using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    public class Scribe : BaseVendor
    {
        public static readonly TimeSpan ShushDelay = TimeSpan.FromMinutes(1);
        private readonly List<SBInfo> m_SBInfos = new();

        private DateTime m_NextShush;

        [Constructible]
        public Scribe() : base("the scribe")
        {
            SetSkill(SkillName.EvalInt, 60.0, 83.0);
            SetSkill(SkillName.Inscribe, 90.0, 100.0);
        }

        public Scribe(Serial serial) : base(serial)
        {
        }

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override NpcGuild NpcGuild => NpcGuild.MagesGuild;

        public override VendorShoeType ShoeType => Utility.RandomBool() ? VendorShoeType.Shoes : VendorShoeType.Sandals;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBScribe());
        }

        public override void InitOutfit()
        {
            base.InitOutfit();

            AddItem(new Robe(Utility.RandomNeutralHue()));
        }

        public override bool HandlesOnSpeech(Mobile from) => from.Player;

        public override void OnSpeech(SpeechEventArgs e)
        {
            base.OnSpeech(e);

            if (!e.Handled && m_NextShush <= Core.Now && InLOS(e.Mobile))
            {
                Direction = GetDirectionTo(e.Mobile);

                PlaySound(Female ? 0x32F : 0x441);
                PublicOverheadMessage(MessageType.Regular, 0x3B2, 1073990); // Shhhh!

                m_NextShush = Core.Now + ShushDelay;
                e.Handled = true;
            }
        }

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

using Server.Network;
using Server.Pantheon;

namespace Server.Mobiles
{
    public class PantheonSteed : Horse, IPantheonMount
    {
        [Constructible]
        public PantheonSteed(string name = "a pantheon steed")
        {
            SetStr(22, 150);
            SetDex(56, 175);
            SetInt(6, 10);
            Alignment = Deity.Alignment.None;

            SetHits(280, 450);
            SetMana(0);

            SetDamage(5, 7);

            Fame = 1000;
            Karma = 1000;

            Tamable = false;
            ControlSlots = 1;
        }

        public PantheonSteed(Serial serial) : base(serial)
        {
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

        public override void OnDoubleClick(Mobile from)
        {
            if (from is PlayerMobile player && player.Alignment != Alignment)
            {
                PublicOverheadMessage(MessageType.Regular, 0x0481, false, "*** Refuses to let you ride ***");
                return;
            }
            base.OnDoubleClick(from);
        }

        public Deity.Alignment Alignment { get; set; }
    }
}

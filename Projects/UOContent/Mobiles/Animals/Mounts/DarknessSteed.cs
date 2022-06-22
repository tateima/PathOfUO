using Server.Pantheon;

namespace Server.Mobiles
{
    public class DarknessSteed : PantheonSteed, IPantheonMount
    {
        [Constructible]
        public DarknessSteed(string name = "a reanimated steed") : base(name)
        {
            Hue = Deity.DarknessHue;
            Alignment = Deity.Alignment.Darkness;
        }

        public DarknessSteed(Serial serial) : base(serial)
        {
        }

        public override string CorpseName => "a reanimated steed corpse";

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

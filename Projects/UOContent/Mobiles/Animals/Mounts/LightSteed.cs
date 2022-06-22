using Server.Pantheon;

namespace Server.Mobiles
{
    public class LightSteed : PantheonSteed, IPantheonMount
    {
        [Constructible]
        public LightSteed(string name = "a light steed") : base(name)
        {
            Hue = Deity.LightHue;
            Alignment = Deity.Alignment.Light;
        }

        public LightSteed(Serial serial) : base(serial)
        {
        }

        public override string CorpseName => "a light steed corpse";

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

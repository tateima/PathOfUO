using Server.Pantheon;

namespace Server.Mobiles
{
    public class ChaosSteed : PantheonSteed, IPantheonMount
    {
        [Constructible]
        public ChaosSteed(string name = "a chaos steed") : base(name)
        {
            Hue = Deity.ChaosHue;
            Alignment = Deity.Alignment.Chaos;
        }

        public ChaosSteed(Serial serial) : base(serial)
        {
        }

        public override string CorpseName => "a chaos steed corpse";

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

        public Deity.Alignment Alignment { get; set; }
    }
}

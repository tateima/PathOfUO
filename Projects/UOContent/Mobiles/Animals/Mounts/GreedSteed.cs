using Server.Pantheon;

namespace Server.Mobiles
{
    public class GreedSteed : PantheonSteed, IPantheonMount
    {
        [Constructible]
        public GreedSteed(string name = "a greed steed") : base(name)
        {
            Hue = Deity.GreedHue;
            Alignment = Deity.Alignment.Greed;
        }

        public GreedSteed(Serial serial) : base(serial)
        {
        }

        public override string CorpseName => "a greed steed corpse";

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

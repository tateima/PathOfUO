using Server.Pantheon;

namespace Server.Mobiles
{
    public class CharitySteed : PantheonSteed, IPantheonMount
    {
        [Constructible]
        public CharitySteed(string name = "a charity steed") : base(name)
        {
            Hue = Deity.CharityHue;
            Alignment = Deity.Alignment.Charity;
        }

        public CharitySteed(Serial serial) : base(serial)
        {
        }

        public override string CorpseName => "a charity steed corpse";

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

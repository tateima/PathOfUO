using Server.Pantheon;

namespace Server.Mobiles
{
    public class OrderSteed : PantheonSteed, IPantheonMount
    {
        [Constructible]
        public OrderSteed(string name = "an order steed") : base(name)
        {
            Hue = Deity.OrderHue;
            Alignment = Deity.Alignment.Order;
        }

        public OrderSteed(Serial serial) : base(serial)
        {
        }

        public override string CorpseName => "an order steed corpse";

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

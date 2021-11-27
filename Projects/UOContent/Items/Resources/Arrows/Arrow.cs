namespace Server.Items
{
    public class Arrow : Item, ICommodity
    {
        [Constructible]
        public Arrow() : this(1) {
        }
        [Constructible]
        public Arrow(int amount = 1) : base(0xF3F)
        {
            Stackable = true;
            Amount = amount;
        }

        public Arrow(Serial serial) : base(serial)
        {
        }

        public override double DefaultWeight => 0.1;
        int ICommodity.DescriptionNumber => LabelNumber;
        bool ICommodity.IsDeedable => true;

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

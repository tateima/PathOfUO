namespace Server.Mobiles
{
    public class AutomatonConstruct : ValoriteElemental
    {
        public override bool IsDispellable => false;

        [Constructible]
        public AutomatonConstruct() : base(0)
        {
        }

        public AutomatonConstruct(Serial serial) : base(serial)
        {
        }

        public override string CorpseName => "an automaton corpse";
        public override string DefaultName => "an automaton";

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);
            var version = reader.ReadInt();
        }
    }
}

namespace Server.Items
{
    public class AutomatonManual : BaseBook
    {
        public static readonly BookContent Content = new(
            "Automaton Manual",
            "Golem Engineer",
            new BookPageInfo(
                "Introduction :",
                "",
                "Your automaton is",
                "the latest in golem",
                "technology.",
                "The more crafting skills",
                "you have the stronger",
                "it will become."
            ),
            new BookPageInfo(
                "Level 2 :",
                "",
                "clock parts: 500",
                "iron ingots: 2000",
                "dull copper: 4000",
                "bronze ingots: 2000",
                "shadow iron: 2000",
                "gold ingots: 2000"
            ),
            new BookPageInfo(
                "spined hide: 2000",
                "horned hide: 2000",
                "",
                "Level 3 :",
                "clock parts: 800",
                "iron ingots: 4000",
                "gold ingots: 3000",
                "bronze ingots: 4000"
            ),
            new BookPageInfo(
                "shadow iron: 4000",
                "horned hide: 3000",
                "barbed hide: 2000",
                "",
                "Level 4 :",
                "clock parts: 1000",
                "gold ingots: 7000",
                "apagite ingots: 4000"
            ),
            new BookPageInfo(
                "verite ingots: 3000",
                "horned hide: 5000",
                "barbed hide: 3000",
                "",
                "Level 5 :",
                "clock parts: 3000",
                "gold ingots: 9000",
                "apagite ingots: 6000"
            ),
            new BookPageInfo(
                "verite: 5000",
                "valorite: 4000",
                "horned hide: 6000",
                "barbed hide: 5000"
            )
        );

        [Constructible]
        public AutomatonManual() : base(Utility.Random(0xFF1, 2), false)
        {
        }

        public AutomatonManual(Serial serial) : base(serial)
        {
        }

        public override BookContent DefaultContent => Content;

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadEncodedInt();
        }
    }
}

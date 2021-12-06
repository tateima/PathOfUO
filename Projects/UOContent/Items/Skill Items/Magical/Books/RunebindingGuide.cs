namespace Server.Items
{
    public class RunebindingGuide : BaseBook
    {
        public static readonly BookContent Content = new(
            "Runebinding Guide",
            "Mordog the Runebinder",
            new BookPageInfo(
                "Introduction :",
                "",
                "Runebinding is an ancient",
                "practice allowing a",
                "user to place powerful",
                "runes into socketed items.",
                " Sockets are put into items",
                " from ornate crafters."
            ),
            new BookPageInfo(
                "Rune words:",
                "",
                "A rune word is a",
                "powerful incantation",
                "of runes placed",
                "in a specific order.",
                "Sadly most knowledge",
                "of such words has been"
            ),
            new BookPageInfo(
                "lost.",
                "",
                "Rumour has it old scrolls",
                " detailing these words",
                "were buried with their",
                "makers long ago."
            )
        );

        [Constructible]
        public RunebindingGuide() : base(Utility.Random(0x0FF0, 2), false)
        {
        }

        public RunebindingGuide(Serial serial) : base(serial)
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

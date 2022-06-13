namespace Server.Items
{
    public class GamblersGuide : BaseBook
    {
        public static readonly BookContent Content = new(
            "Gambler's Guide",
            "Drast the Gypsy Lord",
            new BookPageInfo(
                "Introduction :",
                "",
                "Gambling allows you ",
                "to bet gold with ",
                "other NPCs. ",
                "If you gamble with ",
                "Gypsy's beware! ",
                "They are experienced"
            ),
            new BookPageInfo(
                "gamblers ",
                "but can yield greater ",
                "rewards.",
                "",
                "Some ordained Gypsy's ",
                "can even bet with ",
                "powerful and valuable ",
                "items."
            )
        );

        [Constructible]
        public GamblersGuide() : base(Utility.Random(0x0FF2, 2), false)
        {
        }

        public GamblersGuide(Serial serial) : base(serial)
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

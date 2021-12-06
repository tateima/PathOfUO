namespace Server.Items
{
    public class EnchantersGuide : BaseBook
    {
        public static readonly BookContent Content = new(
            "Enchanters Guide",
            "Zedric the Enchanter",
            new BookPageInfo(
                "Introduction :",
                "",
                "Enchanting allows you",
                "to imbue any item",
                "with additional",
                "magical properties.",
                "Each item can only",
                "be enchanted once."
            ),
            new BookPageInfo(
                "Levelling costs :",
                "",
                "Each level costs an",
                "additional 250 gold",
                "per level and 100",
                "enchanter's dust per",
                "increase.",
                "Enchanter's dust can"
            ),
            new BookPageInfo(
                "can be found",
                "by disenchanting",
                "items with magical",
                "properties."
            )
        );

        [Constructible]
        public EnchantersGuide() : base(Utility.Random(0x0FF2, 2), false)
        {
        }

        public EnchantersGuide(Serial serial) : base(serial)
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

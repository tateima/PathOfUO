namespace Server.Items
{
    public class BookOfAlignments : BaseBook
    {
        [Constructible]
        public BookOfAlignments() : base(Utility.Random(0xFF1, 2), false)
        {
        }

        public static readonly BookContent Content = new(
            "The book of alignments",
            "Malkor the theologist",
            new BookPageInfo(
                "Introduction :",
                "",
                "This book is a scripture",
                "of known deities to",
                "exist in the mortal",
                "realm.",
                "Details include how",
                "to appease them."
            ),
            new BookPageInfo(
                "Nammom the greedy:",
                "Nammom is thought to be",
                "a goblin from the abyss.",
                "He rewards tricksters",
                "and thievery from",
                "others.",
                "Those who do not please",
                "his desires will."
            ),
            new BookPageInfo(
                "Find themselves",
                "paying high tribute",
                "of gold to attone.",
                "They will also be",
                "cursed for a time,",
                "losing fortunes.",
                "",
                ""
            ),
            new BookPageInfo(
                "Elea the charitable:",
                "Elea is a said to be",
                "a kind and beautiful",
                "spirit elf female.",
                "She rewards acts of",
                "kindness to others",
                "and compassion.",
                "Those who commit"
            ),
            new BookPageInfo(
                "themselves to her",
                "and do misdeeds will",
                "suffer incredible",
                "luck penalties.",
                "Stay on the path of",
                "charity, however",
                "and she will grant",
                "blessings."
            ),
            new BookPageInfo(
                "which may turn",
                "your fortunes around.",
                "",
                "Thendros the lawful:",
                "Thendros is a mighty",
                "sky serpent who",
                "strikes for law and",
                "order in all realms."
            ),
            new BookPageInfo(
                "He seeks to restore",
                "balance to those who wish to",
                "create havoc and turmoil. ",
                "Thendros rewards the just ",
                "and righteous who bring justice",
                " in their quest."
            ),
            new BookPageInfo(
                "to those whom dwell",
                "in the abyss.",
                "Such valor shall earn",
                "riches to aid in the",
                "eternal struggle against",
                "the chaos and imbalance."
            ),
            new BookPageInfo(
                "If you do not assist",
                "Thendros to satisfaction",
                "you may find the law's",
                "rule can bring much",
                "hardship and you may even",
                " be stripped of possession."
            ),
            new BookPageInfo(
                "Lolth the hellish:",
                "Lolth is said to be",
                "an esteemed commander of",
                " the abyss and arch nemesis ",
                "of Thendros and his skylords.",
                ""
            ),
            new BookPageInfo(
                "He desires the",
                "decimation of mortal realms",
                "and chaos above all else.",
                "",
                "He will stop at nothing",
                "in achieving his goal",
                "provided it is chaotic."
            ),
            new BookPageInfo(
                "Submit yourself to",
                "him and be ready to",
                "commit to a life focused ",
                "on battling the righteous",
                "and those who lead"
            ),
            new BookPageInfo(
                "with cold blood",
                "in sky and on land.",
                "Succeed and you shall",
                "receive abyssal armory",
                "destined to punish your",
                "reptilian foes.",
                "Fail and you may find"
            ),
            new BookPageInfo(
                "yourself at the",
                "mercy of this tyrant's",
                "will. He has been known",
                "to throw his failures",
                "at his own enemies!",
                "Itarr the life-bringer.",
                "Itarr is an arch"
            ),
            new BookPageInfo(
                "paladin",
                "who strives to bring",
                "light to the lands.",
                "Though similar in",
                "purpose to Thendros,",
                "Itarr disagrees with",
                "good through law."
            ),
            new BookPageInfo(
                "Purity in natural",
                "form and respect for",
                "death are his mantra.",
                "His power is to restore",
                " and transmute holy faith."
            ),
            new BookPageInfo(
                "Itarr's lense is fixated",
                " on those who would",
                "defile the light",
                "with abomination",
                "and unnatural life.",
                "Removing the undead",
                "and necromancy"
            ),
            new BookPageInfo(
                "is his pantheon",
                "quest.",
                "Defeat these enemies,",
                "avoid dark art",
                "practices and you",
                "may find much reward",
                "under his worship.",
                ""
            ),
            new BookPageInfo(
                "Nashtar the defiler:",
                "Nashtar is the sworn",
                "enemy of Itarr.",
                "Itarr's very presence",
                "fills him with immense",
                "hatred for the living.",
                "His hunger for knowledge",
                " drives his conquest."
            ),
            new BookPageInfo(
                "Those born",
                "of living flesh and",
                "bone are his focus.",
                "If you choose the",
                "alignment with Nashtar",
                "be prepared for the",
                "murder of many humanoid",
                " creatures and put"
            ),
            new BookPageInfo(
                "out the light.",
                "Unlike Lolth, Nashtar",
                "is not so concerned",
                "with the means of",
                "one's dark crusade,",
                "provided they do not",
                "assist the light's way.",
                ""
            ),
            new BookPageInfo(
                "Each Deity",
                "alignment mentioned",
                "here all offer",
                "reward and riches.",
                "Each one, however,",
                "also offers something",
                "unique to their path",
                "to those most loyal."
            ),
            new BookPageInfo(
                "You should",
                "choose this carefully",
                "as it cannot go back.",
                "For some, being neutral",
                "is also the right path.",
                "Though if you do wish",
                "to commit yourself",
                "be sure to pray daily."
            )
        );

        public BookOfAlignments(Serial serial) : base(serial)
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

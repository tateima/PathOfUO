namespace Server.Items
{
    [Flippable(0x1f14, 0x1f15, 0x1f16, 0x1f17)]
    public class RuneWord : Item
    {
        public static string[] WordList() =>
            new[]
            {
                "Amn",
                "Ort",
                "Nex",
                "Um",
                "Drux",
                "Vas",
                "Hem",
                "Zaq",
                "Vax",
                "Doth",
                "Zet",
                "Mar",
                "Leq",
                "Pax",
                "Lux",
                "Kres",
                "Hur",
                "Aeo",
                "Zoat",
                "Mea",
                "Aloz",
                "Vox",
                "Vex",
                "Cur",
                "Esp",
                "Mox",
                "Kaz",
                "Za",
                "Dum",
                "Dem",
                "Ze",
                "Lar",
                "Zo",
                "Mir",
                "Li",
                "Cha"
            };

        [Constructible]
        public RuneWord() : base(0x1F14)
        {
            string[] words = WordList();
            string word = words[Utility.Random(words.Length)];
            if (!string.IsNullOrEmpty(word)) {
                Name = word;
            } else {
                Delete();
            }
            Weight = 1.0;
        }

        public RuneWord(Serial serial) : base(serial)
        {
        }

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

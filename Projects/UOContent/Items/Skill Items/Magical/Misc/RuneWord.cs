namespace Server.Items
{
    [Flippable(0x1f14, 0x1f15, 0x1f16, 0x1f17)]
    public class RuneWord : Item
    {
        [Constructible]
        public RuneWord() : base(0x1F14)
        {
            string word = "";
            switch(Utility.RandomMinMax(1, 15)) {
                case 1:
                    word = "Amn";
                    break;
                case 2:
                    word = "Ort";
                    break;
                case 3:
                    word = "Nex";
                    break;
                case 4:
                    word = "Um";
                    break;
                case 5:
                    word = "Drux";
                    break;
                case 6:
                    word = "Vas";
                    break;
                case 7:
                    word = "Hem";
                    break;
                case 8:
                    word = "Zaq";
                    break;
                case 9:
                    word = "Vax";
                    break;
                case 10:
                    word = "Doth";
                    break;
                case 11:
                    word = "Zet";
                    break;
                case 12:
                    word = "Mar";
                    break;
                case 13:
                    word = "Leq";
                    break;
                case 14:
                    word = "Pax";
                    break;
                case 15:
                    word = "Lux";
                    break;
                case 16:
                    word = "Kres";
                    break;
                case 17:
                    word = "Hur";
                    break;
                case 18:
                    word = "Aeo";
                    break;
                case 19:
                    word = "Zoat";
                    break;
                case 20:
                    word = "Mea";
                    break;
                case 21:
                    word = "Aloz";
                    break;
                case 22:
                    word = "Vox";
                    break;
                case 23:
                    word = "Vex";
                    break;
                case 24:
                    word = "Cur";
                    break;
                case 25:
                    word = "Esp";
                    break;
                case 26:
                    word = "Mox";
                    break;
                case 27:
                    word = "Kaz";
                    break;
                case 28:
                    word = "Za";
                    break;
                case 29:
                    word = "Dum";
                    break;
                case 30:
                    word = "Dem";
                    break;
                case 31:
                    word = "Ze";
                    break;
                case 32:
                    word = "Lar";
                    break;
                case 33:
                    word = "Zo";
                    break;
                case 34:
                    word = "Mir";
                    break;
                case 35:
                    word = "Li";
                    break;
                case 36:
                    word = "Cha";
                    break;
            }
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

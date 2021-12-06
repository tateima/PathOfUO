using Server.Items;

namespace Server.Mobiles
{
    public class GypsyLord : Gypsy
    {
        [Constructible]
        public GypsyLord()
        {
            InitStats(100, 120, 90);

            SpeechHue = Utility.RandomDyedHue();

            SetSkill(SkillName.Cooking, 99, 100);
            SetSkill(SkillName.Snooping, 99, 100);
            SetSkill(SkillName.Stealing, 99, 100);

            Hue = Race.Human.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                AddItem(new Kilt(Utility.RandomDyedHue()));
                AddItem(new Shirt(Utility.RandomDyedHue()));
                AddItem(new ThighBoots());
                Title = "the gypsy lady";
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                AddItem(new ShortPants(Utility.RandomNeutralHue()));
                AddItem(new Shirt(Utility.RandomDyedHue()));
                AddItem(new Sandals());
                Title = "the gypsy lord";
            }

            AddItem(new Bandana(Utility.RandomDyedHue()));
            AddItem(new Dagger());

            Utility.AssignRandomHair(this);

            Container pack = new Backpack();

            pack.DropItem(new Gold(250, 300));

            pack.Movable = false;

            AddItem(pack);
        }

        public GypsyLord(Serial serial)
            : base(serial)
        {
        }

        public override bool CanTeach => true;
        public override bool ClickTitle => false;

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

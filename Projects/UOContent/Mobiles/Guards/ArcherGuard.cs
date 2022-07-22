using System;
using Server.Items;

namespace Server.Mobiles
{
    public class ArcherGuard : BaseGuard
    {

        [Constructible]
        public ArcherGuard()
        {
            AiType = AIType.AI_Archer;
            InitStats(Utility.RandomMinMax(75, 250), Utility.RandomMinMax(75, 250), Utility.RandomMinMax(75, 250));
            Title = "the guard";

            SpeechHue = Utility.RandomDyedHue();

            Hue = Race.Human.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
            }

            new Horse().Rider = this;

            AddItem(new StuddedChest());
            AddItem(new StuddedArms());
            AddItem(new StuddedGloves());
            AddItem(new StuddedGorget());
            AddItem(new StuddedLegs());
            AddItem(new Boots());
            AddItem(new SkullCap());

            var bow = new Bow();

            bow.Movable = false;
            bow.Crafter = this;
            bow.Quality = WeaponQuality.Exceptional;

            AddItem(bow);

            Container pack = new Backpack();

            pack.Movable = false;

            var arrows = new Arrow(250);

            arrows.LootType = LootType.Newbied;

            pack.DropItem(arrows);
            pack.DropItem(new Gold(10, 25));

            AddItem(pack);

            Skills.Anatomy.Base = Utility.RandomMinMax(50.0, 100.0);
            Skills.Tactics.Base = Utility.RandomMinMax(50.0, 100.0);
            Skills.Archery.Base = Utility.RandomMinMax(50.0, 100.0);
            Skills.MagicResist.Base = Utility.RandomMinMax(50.0, 100.0);
            Skills.DetectHidden.Base = Utility.RandomMinMax(50.0, 100.0);

            NextCombatTime = Core.TickCount + 500;
        }

        public ArcherGuard(Serial serial) : base(serial)
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

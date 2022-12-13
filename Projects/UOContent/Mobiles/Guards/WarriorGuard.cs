using System;
using Server.Items;

namespace Server.Mobiles
{
    public class WarriorGuard : BaseGuard
    {
        [Constructible]
        public WarriorGuard(int maxStrength = 3) : base()
        {
            AiType = AIType.AI_Melee;
            SetHumanoidStrength(maxStrength);

            Title = "the guard";

            SpeechHue = Utility.RandomDyedHue();

            Hue = Race.Human.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");

                AddItem(Utility.RandomBool() ? new LeatherSkirt() : new LeatherShorts());

                AddItem(
                    Utility.Random(5) switch
                    {
                        0 => new FemaleLeatherChest(),
                        1 => new FemaleStuddedChest(),
                        2 => new LeatherBustierArms(),
                        3 => new StuddedBustierArms(),
                        _ => new FemalePlateChest() // 4
                    }
                );
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");

                AddItem(new PlateChest());
                AddItem(new PlateArms());
                AddItem(new PlateLegs());

                AddItem(
                    Utility.Random(3) switch
                    {
                        0 => new Doublet(Utility.RandomNondyedHue()),
                        1 => new Tunic(Utility.RandomNondyedHue()),
                        _ => new BodySash(Utility.RandomNondyedHue()) // 3
                    }
                );
            }

            Utility.AssignRandomHair(this);

            if (Utility.RandomBool())
            {
                Utility.AssignRandomFacialHair(this, HairHue);
            }

            var weapon = new Halberd();

            weapon.Movable = false;
            weapon.Crafter = this;
            weapon.Quality = WeaponQuality.Exceptional;

            AddItem(weapon);

            Container pack = new Backpack();

            pack.Movable = false;

            pack.DropItem(new Gold(10, 25));

            AddItem(pack);

            Skills.Anatomy.Base = Utility.RandomMinMax(75.0, 100.0);
            Skills.Tactics.Base = Utility.RandomMinMax(75.0, 100.0);
            Skills.Swords.Base = Utility.RandomMinMax(750, 100.0);
            Skills.MagicResist.Base = Utility.RandomMinMax(75.0, 100.0);
            Skills.DetectHidden.Base = Utility.RandomMinMax(75.0, 100.0);

            NextCombatTime = Core.TickCount + 500;
        }

        public WarriorGuard(Serial serial) : base(serial)
        {
        }

        public override Mobile Focus { get; set; }

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

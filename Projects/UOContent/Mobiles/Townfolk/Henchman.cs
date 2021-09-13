using Server.Items;

namespace Server.Mobiles
{
    public class Henchman : BaseCreature
    {
        [Constructible]
        public Henchman()
            : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 1.4)
        {
            SpeechHue = Utility.RandomDyedHue();

            SetStr(86, 150);
            SetDex(81, 95);
            SetInt(61, 75);

            SetSkill(SkillName.Fencing, 60.0, 60.0);
            SetSkill(SkillName.Macing, 60.0, 60.0);
            SetSkill(SkillName.MagicResist, 60.0, 60.0);
            SetSkill(SkillName.Swords, 60.0, 60.0);
            SetSkill(SkillName.Tactics, 60.0, 60.0);
            SetSkill(SkillName.Wrestling, 60.0, 60.0);
            SetSkill(SkillName.Anatomy, 60.0, 60.0);
            SetSkill(SkillName.Healing, 60.0, 60.0);

            Hue = Race.Human.RandomSkinHue();
            Title = "the henchman";
            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
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
                AddItem(new PlateGloves());
                AddItem(new PlateGorget());
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                if (Utility.Random(2) < 1)
                {
                    AddItem(new StuddedArms());
                    AddItem(new StuddedChest());
                    AddItem(new StuddedGloves());
                    AddItem(new StuddedLegs());
                    AddItem(new Sandals());
                } else
                {
                    AddItem(new ChainChest());
                    AddItem(new ChainCoif());
                    AddItem(new ChainLegs());
                    AddItem(new PlateArms());
                    AddItem(new PlateGloves());
                    AddItem(new PlateGorget());
                }

            }

            AddItem(new Cloak());
            AddItem(
                Utility.Random(5) switch
                {
                    0 => new Longsword(),
                    1 => new Bardiche(),
                    2 => new Mace(),
                    3 => new Kryss(),
                    _ => new Spear() // 4
                }
            );

            Utility.AssignRandomHair(this);

            if (Utility.RandomBool())
            {
                Utility.AssignRandomFacialHair(this, HairHue);
            }
            Container pack = new Backpack();
            pack.Movable = false;
            AddItem(pack);
        }

        public Henchman(Serial serial)
            : base(serial)
        {
        }

        public override bool CanTeach => true;
        public override bool ClickTitle => false;

        public override bool CheckGold(Mobile from, Item dropped) => dropped is Gold gold && OnGoldGiven(from, gold);
        public override bool OnGoldGiven(Mobile from, Gold dropped)
        {
            if (Controlled && (ControlMaster == from))
            {
                Loyalty += dropped.Amount;
            }
            if (Loyalty > MaxLoyalty)
            {
                Loyalty = MaxLoyalty;
            }
            return true;
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

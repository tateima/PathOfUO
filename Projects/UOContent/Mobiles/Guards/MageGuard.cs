using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class MageGuard : BaseGuard
    {
        [Constructible]
        public MageGuard(int maxStrength = 3)
        {
            AiType = AIType.AI_Mage;
            Title = "the guard";
            Female = Utility.RandomBool();
            if (Female)
            {
                Body = 401;
                Name = NameList.RandomName("female");
            }
            else
            {
                Body = 400;
                Name = NameList.RandomName("male");
            }
            SetHumanoidStrength(maxStrength);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 30);
            SetResistance(ResistanceType.Fire, 15, 30);
            SetResistance(ResistanceType.Poison, 15, 30);
            SetResistance(ResistanceType.Energy, 15, 30);

            SetSkill(SkillName.EvalInt, 75.1, 100.0);
            SetSkill(SkillName.Magery, 75.1, 100.0);
            SetSkill(SkillName.MagicResist, 75.0, 100.0);
            SetSkill(SkillName.Tactics, 75.0, 100.0);
            SetSkill(SkillName.Wrestling, 75.2, 100.0);

            Fame = 2500;
            Karma = -2500;

            VirtualArmor = 16;
            PackReg(6);
            EquipItem(new Robe(Utility.RandomNeutralHue()));
            EquipItem(new Sandals());
        }
        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };
        public override string CorpseName => "a mage guard corpse";

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.MedScrolls);
        }

        public override Mobile Focus { get; set; }

        [AfterDeserialization]
        private void AfterDeserialization()
        {
            if (Core.UOR)
            {
                if (Body == 0x190)
                {
                    Body = 124;
                }

                if (Hue != 0)
                {
                    Hue = 0;
                }
            }
            else if (Body == 124)
            {
                Body = 0x190;

                if (Hue == 0)
                {
                    Hue = Race.Human.RandomSkinHue();
                }
            }
        }
    }
}

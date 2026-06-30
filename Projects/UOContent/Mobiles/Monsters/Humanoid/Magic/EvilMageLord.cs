using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class EvilMageLord : BaseCreature
    {
        [Constructible]
        public EvilMageLord() : base(AIType.AI_Mage)
        {
            Name = NameList.RandomName("evil mage lord");
            if (Core.UOR)
            {
                Body = Utility.Random(125, 2);
            }
            else
            {
                Body = 0x190;
                Hue = Race.Human.RandomSkinHue();
                Utility.AssignRandomHair(this);
                Utility.AssignRandomFacialHair(this, HairHue);
            }

            LevelRange = [40, 60];
            StrPerLevel = [1, 3];
            IntPerLevel = [4, 6];
            DexPerLevel = [2, 4];
            ResistancePerLevel = [1, 2];

            SetStr(35, 60);
            SetDex(25, 45);
            SetInt(65, 100);
            SetHits(90,120);

            SetDamage(4, 8);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 20);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.EvalInt, 45.1, 50.0);
            SetSkill(SkillName.Magery, 45.1, 50.0);
            SetSkill(SkillName.Meditation, 45.1, 50.0);
            SetSkill(SkillName.MagicResist, 45.1, 50.0);
            SetSkill(SkillName.Tactics, 45.1, 50.0);
            SetSkill(SkillName.Wrestling, 45.1, 50.0);

            Fame = 10500;
            Karma = -10500;

            VirtualArmor = 16;
            PackReg(23);

            var hue = Utility.RandomNeutralHue();
            EquipItem(new Robe(hue));
            EquipItem(new WizardsHat(hue));
            EquipItem(new Sandals());
        }
        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };
        public override string CorpseName => "an evil mage lord corpse";

        public override bool CanRummageCorpses => true;
        public override bool AlwaysMurderer => true;
        public override int Meat => 1;
        public override int TreasureMapLevel => Core.AOS ? 2 : 0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Meager);
            AddLoot(LootPack.MedScrolls, 2);
        }

        [AfterDeserialization]
        private void AfterDeserialization()
        {
            if (Core.UOR)
            {
                if (Body == 0x190)
                {
                    Body = Utility.Random(125, 2);
                    HairItemID = 0;
                    HairHue = 0;
                    FacialHairItemID = 0;
                    FacialHairItemID = 0;
                    Hue = 0;
                }
            }
            else if (Body.BodyID is 125 or 126)
            {
                Body = 0x190;
                Hue = Race.Human.RandomSkinHue();
                Utility.AssignRandomHair(this);
                Utility.AssignRandomFacialHair(this, HairHue);
            }
        }
    }
}

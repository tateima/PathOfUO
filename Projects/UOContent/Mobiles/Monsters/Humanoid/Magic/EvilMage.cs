using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class EvilMage : BaseCreature
    {
        [Constructible]
        public EvilMage() : base(AIType.AI_Mage)
        {
            Name = NameList.RandomName("evil mage");
            Title = "the evil mage";
            if (Core.UOR)
            {
                Body = 124;
            }
            else
            {
                Body = 0x190;
                Hue = Race.Human.RandomSkinHue();
                Utility.AssignRandomHair(this);
                Utility.AssignRandomFacialHair(this, HairHue);
            }

            LevelRange = [18, 38];
            StrPerLevel = [1, 3];
            IntPerLevel = [2, 4];
            DexPerLevel = [2, 4];
            ResistancePerLevel = [1, 2];

            SetStr(25, 50);
            SetDex(25, 35);
            SetInt(45, 90);
            SetHits(90, 100);

            SetDamage(3, 6);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 10);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.EvalInt, 40.1, 50.0);
            SetSkill(SkillName.Magery, 40.1, 50.0);
            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 2500;
            Karma = -2500;

            VirtualArmor = 16;
            PackReg(6);

            EquipItem(new Robe(Utility.RandomNeutralHue()));
            EquipItem(new Sandals());
        }
        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };
        public override string CorpseName => "an evil mage corpse";

        public override bool CanRummageCorpses => true;
        public override bool AlwaysMurderer => true;
        public override int Meat => 1;
        public override int TreasureMapLevel => Core.AOS ? 1 : 0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.MedScrolls);
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

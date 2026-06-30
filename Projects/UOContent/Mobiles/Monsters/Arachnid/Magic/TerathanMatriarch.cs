using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class TerathanMatriarch : BaseCreature
    {
        [Constructible]
        public TerathanMatriarch() : base(AIType.AI_Mage)
        {
            Body = 72;
            BaseSoundID = 599;

            LevelRange = [27, 35];
            StrPerLevel = [2, 3];
            IntPerLevel = [1, 7];
            DexPerLevel = [2, 3];
            ResistancePerLevel = [1, 3];
            SetStr(56, 75);
            SetDex(50, 99);
            SetInt(60, 95);

            SetHits(52, 89);
            SetMana(0);

            SetDamage(4, 7);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 35);
            SetResistance(ResistanceType.Fire, 5, 15);
            SetResistance(ResistanceType.Cold, 5, 20);
            SetResistance(ResistanceType.Poison, 10, 30);
            SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.EvalInt, 45.1, 55.0);
            SetSkill(SkillName.Magery, 45.1, 55.0);
            SetSkill(SkillName.MagicResist, 45.1, 55.0);
            SetSkill(SkillName.Tactics, 45.1, 55.0);
            SetSkill(SkillName.Wrestling, 45.1, 55.0);

            Fame = 10000;
            Karma = -10000;

            PackItem(new SpidersSilk(5));
            PackNecroReg(Utility.RandomMinMax(4, 10));
        }

        public override string CorpseName => "a terathan matriarch corpse";
        public override string DefaultName => "a terathan matriarch";

        public override int TreasureMapLevel => 4;

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.TerathansAndOphidians };

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Average, 2);
            AddLoot(LootPack.MedScrolls, 2);
            AddLoot(LootPack.Potions);
        }
    }
}

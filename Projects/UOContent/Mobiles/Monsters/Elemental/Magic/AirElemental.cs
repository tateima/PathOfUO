using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class AirElemental : BaseCreature
    {
        [Constructible]
        public AirElemental() : base(AIType.AI_Mage)
        {
            Body = 13;
            Hue = 0x4001;
            BaseSoundID = 655;

            LevelRange = [22, 34];
            StrPerLevel = [2, 3];
            IntPerLevel = [1, 5];
            DexPerLevel = [2, 4];
            ResistancePerLevel = [1, 3];

            SetStr(50, 70);
            SetDex(20, 65);
            SetInt(55, 95);
            SetHits(95, 150);
            SetDamage(2, 5);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Cold, 40);
            SetDamageType(ResistanceType.Energy, 40);

            SetResistance(ResistanceType.Physical, 15, 35);
            SetResistance(ResistanceType.Fire, 5, 15);
            SetResistance(ResistanceType.Cold, 1, 10);
            SetResistance(ResistanceType.Poison, 1, 10);
            SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.EvalInt, 40.1, 50.0);
            SetSkill(SkillName.Magery, 40.1, 50.0);
            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 4500;
            Karma = -4500;

            VirtualArmor = 40;
        }

        public override string CorpseName => "an air elemental corpse";
        public override string DefaultName => "an air elemental";

        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 2;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Meager);
            AddLoot(LootPack.LowScrolls);
            AddLoot(LootPack.MedScrolls);
        }
    }
}

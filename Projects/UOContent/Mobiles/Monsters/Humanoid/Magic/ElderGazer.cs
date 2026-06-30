using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class ElderGazer : BaseCreature
    {
        [Constructible]
        public ElderGazer() : base(AIType.AI_Mage)
        {
            Body = 22;
            BaseSoundID = 377;

            LevelRange = [35, 55];
            StrPerLevel = [1, 2];
            IntPerLevel = [1, 10];
            DexPerLevel = [1, 2];
            ResistancePerLevel = [2, 3];

            SetStr(56, 75);
            SetDex(31, 45);
            SetInt(96, 130);
            SetHits(70,199);

            SetDamage(4, 8);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.Anatomy, 50.0, 60.5);
            SetSkill(SkillName.EvalInt, 50.0, 60.5);
            SetSkill(SkillName.Magery, 50.0, 60.5);
            SetSkill(SkillName.MagicResist, 50.0, 60.5);
            SetSkill(SkillName.Tactics, 50.0, 60.5);
            SetSkill(SkillName.Wrestling, 50.0, 60.5);

            Fame = 12500;
            Karma = -12500;

            VirtualArmor = 50;
        }

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder };
        public override string CorpseName => "an elder gazer corpse";
        public override string DefaultName => "an elder gazer";

        public override int TreasureMapLevel => Core.AOS ? 4 : 0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
        }
    }
}

using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class MoundOfMaggots : BaseCreature
    {
        [Constructible]
        public MoundOfMaggots() : base(AIType.AI_Melee)
        {
            Body = 319;
            BaseSoundID = 898;
            LevelRange = [5, 10];
            StrPerLevel = [1, 3];
            IntPerLevel = [1, 2];
            DexPerLevel = [1, 3];
            ResistancePerLevel = [1, 2];

            SetStr(61, 70);
            SetDex(61, 70);
            SetInt(10);

            SetMana(0);

            SetDamage(1, 6);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 20);
            SetResistance(ResistanceType.Poison, 100);

            SetSkill(SkillName.Tactics, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 1000;
            Karma = -1000;

            VirtualArmor = 24;
        }

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder };
        public override string CorpseName => "a maggoty corpse";
        public override string DefaultName => "a mound of maggots";

        public override Poison PoisonImmune => Poison.Lethal;

        public override int TreasureMapLevel => 1;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager);
            AddLoot(LootPack.Gems);
        }
    }
}

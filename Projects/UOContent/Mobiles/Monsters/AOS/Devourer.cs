using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Devourer : BaseCreature
    {
        [Constructible]
        public Devourer() : base(AIType.AI_Mage)
        {
            Body = 303;
            BaseSoundID = 357;

            LevelRange = [55, 65];
            StrPerLevel = [3, 6];
            IntPerLevel = [3, 6];
            DexPerLevel = [3, 6];
            ResistancePerLevel = [1, 2];

            SetStr(86, 115);
            SetDex(60, 90);
            SetInt(62, 120);

            SetHits(190, 220);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Cold, 20);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 15, 35);
            SetResistance(ResistanceType.Fire, 5, 15);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 10, 40);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.EvalInt, 53.0, 63.5);
            SetSkill(SkillName.Magery, 53.0, 63.5);
            SetSkill(SkillName.Meditation, 53.0, 63.5);
            SetSkill(SkillName.MagicResist, 53.0, 63.5);
            SetSkill(SkillName.Tactics, 53.0, 63.5);
            SetSkill(SkillName.Wrestling, 53.0, 63.5);

            Fame = 9500;
            Karma = -9500;

            VirtualArmor = 44;

            PackNecroReg(24, 45);
        }

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder };
        public override string CorpseName => "a devourer of souls corpse";
        public override string DefaultName => "a devourer of souls";

        public override Poison PoisonImmune => Poison.Lethal;

        public override int Meat => 3;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
        }
    }
}

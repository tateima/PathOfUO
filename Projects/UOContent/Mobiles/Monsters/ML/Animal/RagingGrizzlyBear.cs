using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class RagingGrizzlyBear : BaseCreature
    {
        [Constructible]
        public RagingGrizzlyBear() : base(AIType.AI_Animal, FightMode.Aggressor)
        {
            Body = 212;
            BaseSoundID = 0xA3;
            LevelRange = [55, 65];
            StrPerLevel = [2, 3];
            IntPerLevel = [1, 2];
            DexPerLevel = [1, 3];
            ResistancePerLevel = [1, 2];

            SetStr(76, 106);
            SetDex(30, 40);
            SetInt(22, 45);

            SetHits(86, 150);

            SetDamage(2, 8);
            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Cold, 5, 20);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.Wrestling, 40.4, 50.1);
            SetSkill(SkillName.Tactics, 40.6, 50.5);
            SetSkill(SkillName.MagicResist, 40.8, 50.6);
            SetSkill(SkillName.Anatomy, 40.0, 50);

            Fame = 10000;  // Guessing here
            Karma = 10000; // Guessing here

            VirtualArmor = 24;

            Tamable = false;
        }

        public override string CorpseName => "a grizzly bear corpse";
        public override string DefaultName => "a raging grizzly bear";

        public override int Meat => 4;
        public override int Hides => 32;
        public override PackInstinct PackInstinct => PackInstinct.Bear;
    }
}

using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class TerathanAvenger : BaseCreature
    {
        [Constructible]
        public TerathanAvenger() : base(AIType.AI_Mage)
        {
            Body = 152;
            BaseSoundID = 0x24D;
            LevelRange = [23, 31];
            StrPerLevel = [2, 4];
            IntPerLevel = [1, 2];
            DexPerLevel = [2, 3];
            ResistancePerLevel = [1, 2];
            SetStr(56, 75);
            SetDex(50, 99);
            SetInt(60, 95);

            SetHits(52, 99);
            SetMana(0);

            SetDamage(7, 9);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 10, 30);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 10, 30);
            SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.EvalInt, 50.3, 58.0);
            SetSkill(SkillName.Magery, 50.3, 58.0);
            SetSkill(SkillName.Poisoning, 50.1, 58.0);
            SetSkill(SkillName.MagicResist, 50.1, 58.0);
            SetSkill(SkillName.Tactics, 50.1, 58);
            SetSkill(SkillName.Wrestling, 50.1, 58.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 50;
        }

        public override string CorpseName => "a terathan avenger corpse";
        public override string DefaultName => "a terathan avenger";

        public override Poison PoisonImmune => Poison.Deadly;
        public override Poison HitPoison => Poison.Deadly;
        public override int TreasureMapLevel => 3;
        public override int Meat => 2;

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.TerathansAndOphidians };

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 2);
        }
    }
}

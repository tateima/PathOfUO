using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class BoneDemon : BaseCreature
    {
        [Constructible]
        public BoneDemon() : base(AIType.AI_Mage)
        {
            Body = 308;
            BaseSoundID = 0x48D;

            LevelRange = [65, 75];
            StrPerLevel = [4, 6];
            IntPerLevel = [4, 6];
            DexPerLevel = [4, 6];
            ResistancePerLevel = [1, 2];

            SetStr(126, 170);
            SetDex(70, 100);
            SetInt(72, 130);

            SetHits(210, 240);

            SetDamage(5, 11);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50);

            SetResistance(ResistanceType.Physical, 10, 30);
            SetResistance(ResistanceType.Fire, 10, 30);
            SetResistance(ResistanceType.Cold, 5, 20);
            SetResistance(ResistanceType.Poison, 30);
            SetResistance(ResistanceType.Energy, 10, 30);

            SetSkill(SkillName.DetectHidden, 53.0, 63.5);
            SetSkill(SkillName.EvalInt, 53.0, 63.5);
            SetSkill(SkillName.Magery, 53.0, 63.5);
            SetSkill(SkillName.Meditation, 53.0, 63.5);
            SetSkill(SkillName.MagicResist, 53.0, 63.5);
            SetSkill(SkillName.Tactics, 53.0, 63.5);
            SetSkill(SkillName.Wrestling, 53.0, 63.5);

            Fame = 20000;
            Karma = -20000;

            VirtualArmor = 44;
        }

        public override string CorpseName => "a bone demon corpse";
        public override string DefaultName => "a bone demon";

        public override bool BardImmune => !Core.SE;
        public override bool Unprovokable => Core.SE;
        public override bool AreaPeaceImmune => Core.SE;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int TreasureMapLevel => 1;
        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder, OppositionGroup.CelestialsAndDaemons };

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 8);
        }
    }
}

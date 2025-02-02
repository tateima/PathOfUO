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

            SetStr(1000);
            SetDex(151, 175);
            SetInt(171, 220);

            SetHits(3600);

            SetDamage(34, 36);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50);

            SetResistance(ResistanceType.Physical, 75);
            SetResistance(ResistanceType.Fire, 60);
            SetResistance(ResistanceType.Cold, 90);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60);

            SetSkill(SkillName.DetectHidden, 80.0);
            SetSkill(SkillName.EvalInt, 77.6, 87.5);
            SetSkill(SkillName.Magery, 77.6, 87.5);
            SetSkill(SkillName.Meditation, 100.0);
            SetSkill(SkillName.MagicResist, 50.1, 75.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);

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
        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder };

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 8);
        }
    }
}

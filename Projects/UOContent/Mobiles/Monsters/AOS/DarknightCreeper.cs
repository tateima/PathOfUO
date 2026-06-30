using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class DarknightCreeper : BaseCreature
    {
        [Constructible]
        public DarknightCreeper() : base(AIType.AI_Mage)
        {
            Name = NameList.RandomName("darknight creeper");
            Body = 313;
            BaseSoundID = 0xE0;

            LevelRange = [35, 45];
            StrPerLevel = [3, 4];
            IntPerLevel = [3, 4];
            DexPerLevel = [3, 4];
            ResistancePerLevel = [1, 2];

            SetStr(66, 95);
            SetDex(40, 60);
            SetInt(42, 90);

            SetHits(190, 220);

            SetDamage(3, 7);

            SetDamageType(ResistanceType.Physical, 85);
            SetDamageType(ResistanceType.Poison, 15);

            SetResistance(ResistanceType.Physical, 10, 30);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 20);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.DetectHidden, 53.0, 63.5);
            SetSkill(SkillName.EvalInt, 53.0, 63.5);
            SetSkill(SkillName.Magery, 53.0, 63.5);
            SetSkill(SkillName.Meditation, 53.0, 63.5);
            SetSkill(SkillName.Poisoning, 53.0, 63.5);
            SetSkill(SkillName.MagicResist, 53.0, 63.5);
            SetSkill(SkillName.Tactics, 53.0, 63.5);
            SetSkill(SkillName.Wrestling, 53.0, 63.5);
            SetSkill(SkillName.Necromancy, 53.0, 63.5);
            SetSkill(SkillName.SpiritSpeak, 53.0, 63.5);

            Fame = 22000;
            Karma = -22000;

            VirtualArmor = 34;
        }

        public override string CorpseName => "a darknight creeper corpse";
        public override bool IgnoreYoungProtection => Core.ML;

        public override bool BardImmune => !Core.SE;
        public override bool Unprovokable => Core.SE;
        public override bool AreaPeaceImmune => Core.SE;
        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override Poison HitPoison => Poison.Lethal;

        public override int TreasureMapLevel => 1;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (!Summoned && !NoKillAwards && DemonKnight.CheckArtifactChance(this))
            {
                DemonKnight.DistributeArtifact(this);
            }
        }
    }
}

using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class FleshRenderer : BaseCreature
    {
        [Constructible]
        public FleshRenderer() : base(AIType.AI_Melee)
        {
            Body = 315;
            LevelRange = [25, 45];
            StrPerLevel = [2, 6];
            IntPerLevel = [1, 2];
            DexPerLevel = [2, 6];
            ResistancePerLevel = [1, 2];

            SetStr(96, 125);
            SetDex(70, 100);
            SetInt(52, 90);

            SetHits(150, 180);

            SetDamage(3, 10);

            SetDamageType(ResistanceType.Physical, 80);
            SetDamageType(ResistanceType.Poison, 20);

            SetResistance(ResistanceType.Physical, 10, 30);
            SetResistance(ResistanceType.Fire, 5, 20);
            SetResistance(ResistanceType.Cold, 15, 25);
            SetResistance(ResistanceType.Poison, 30, 50);
            SetResistance(ResistanceType.Energy, 15, 25);

            SetSkill(SkillName.DetectHidden, 53.0, 63.5);
            SetSkill(SkillName.MagicResist, 53.0, 63.5);
            SetSkill(SkillName.Meditation, 53.0, 63.5);
            SetSkill(SkillName.Tactics, 53.0, 63.5);
            SetSkill(SkillName.Wrestling, 53.0, 63.5);

            Fame = 23000;
            Karma = -23000;

            VirtualArmor = 24;
        }

        public override string CorpseName => "a fleshrenderer corpse";

        public override bool IgnoreYoungProtection => Core.ML;

        public override string DefaultName => "a fleshrenderer";

        public override bool AutoDispel => true;
        public override bool BardImmune => !Core.SE;
        public override bool Unprovokable => Core.SE;
        public override bool AreaPeaceImmune => Core.SE;
        public override Poison PoisonImmune => Poison.Lethal;

        public override int TreasureMapLevel => 1;

        public override WeaponAbility GetWeaponAbility() =>
            Utility.RandomBool() ? WeaponAbility.Dismount : WeaponAbility.ParalyzingBlow;

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

        public override int GetAttackSound() => 0x34C;

        public override int GetHurtSound() => 0x354;

        public override int GetAngerSound() => 0x34C;

        public override int GetIdleSound() => 0x34C;

        public override int GetDeathSound() => 0x354;
    }
}

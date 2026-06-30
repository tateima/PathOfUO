using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class AbysmalHorror : BaseCreature
    {
        [Constructible]
        public AbysmalHorror() : base(AIType.AI_Mage)
        {
            Body = 312;
            BaseSoundID = 0x451;

            LevelRange = [50, 60];
            StrPerLevel = [2, 5];
            IntPerLevel = [2, 5];
            DexPerLevel = [2, 5];
            ResistancePerLevel = [1, 2];

            SetStr(66, 95);
            SetDex(60, 90);
            SetInt(82, 140);

            SetHits(190, 220);

            SetDamage(3, 7);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 10, 25);
            SetResistance(ResistanceType.Fire, 10, 25);
            SetResistance(ResistanceType.Cold, 10, 25);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 10, 35);

            SetSkill(SkillName.EvalInt, 53.0, 63.5);
            SetSkill(SkillName.Magery, 53.0, 63.5);
            SetSkill(SkillName.Meditation, 53.0, 63.5);
            SetSkill(SkillName.MagicResist, 53.0, 63.5);
            SetSkill(SkillName.Tactics, 53.0, 63.5);
            SetSkill(SkillName.Wrestling, 53.0, 63.5);

            Fame = 26000;
            Karma = -26000;

            VirtualArmor = 54;
        }

        public override string CorpseName => "an abysmal horror corpse";

        public override bool IgnoreYoungProtection => Core.ML;

        public override string DefaultName => "an abysmal horror";

        public override bool BardImmune => !Core.SE;
        public override bool Unprovokable => Core.SE;
        public override bool AreaPeaceImmune => Core.SE;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int TreasureMapLevel => 1;
        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder };

        public override WeaponAbility GetWeaponAbility() =>
            Utility.RandomBool() ? WeaponAbility.MortalStrike : WeaponAbility.WhirlwindAttack;

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

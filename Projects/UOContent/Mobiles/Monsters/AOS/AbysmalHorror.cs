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

            SetStr(401, 420);
            SetDex(81, 90);
            SetInt(401, 420);

            SetHits(6000);

            SetDamage(13, 17);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 30, 35);
            SetResistance(ResistanceType.Fire, 100);
            SetResistance(ResistanceType.Cold, 50, 55);
            SetResistance(ResistanceType.Poison, 60, 65);
            SetResistance(ResistanceType.Energy, 77, 80);

            SetSkill(SkillName.EvalInt, 200.0);
            SetSkill(SkillName.Magery, 112.6, 117.5);
            SetSkill(SkillName.Meditation, 200.0);
            SetSkill(SkillName.MagicResist, 117.6, 120.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 84.1, 88.0);

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

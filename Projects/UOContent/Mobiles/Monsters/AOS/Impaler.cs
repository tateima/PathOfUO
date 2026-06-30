using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Impaler : BaseCreature
    {
        [Constructible]
        public Impaler() : base(AIType.AI_Melee)
        {
            Name = NameList.RandomName("impaler");
            Body = 306;
            BaseSoundID = 0x2A7;
            LevelRange = [10, 20];
            StrPerLevel = [2, 5];
            IntPerLevel = [1, 2];
            DexPerLevel = [2, 3];
            ResistancePerLevel = [1, 2];

            SetStr(44, 70);
            SetDex(20, 45);
            SetInt(20, 30);

            SetHits(100, 130);

            SetDamage(3, 8);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 30);
            SetResistance(ResistanceType.Fire, 5, 15);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.DetectHidden, 40.1, 50.0);
            SetSkill(SkillName.Meditation, 40.1, 50.0);
            SetSkill(SkillName.Poisoning, 50.1, 60.0);
            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 49;
        }

        public override string CorpseName => "an impaler corpse";

        public override bool IgnoreYoungProtection => Core.ML;

        public override bool AutoDispel => true;
        public override bool BardImmune => !Core.SE;
        public override bool Unprovokable => Core.SE;
        public override bool AreaPeaceImmune => Core.SE;
        public override Poison PoisonImmune => Poison.Lethal;
        public override Poison HitPoison => Utility.RandomDouble() < 0.8 ? Poison.Greater : Poison.Deadly;

        public override int TreasureMapLevel => 1;

        public override WeaponAbility GetWeaponAbility() =>
            Utility.RandomBool() ? WeaponAbility.MortalStrike : WeaponAbility.BleedAttack;

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

using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Reptalon : BaseMount
    {
        public override string DefaultName => "a reptalon";

        [Constructible]
        public Reptalon() : base(0x114, 0x3E90, AIType.AI_Melee)
        {
            BaseSoundID = 0x16A;

            LevelRange = [55, 65];
            StrPerLevel = [4, 5];
            IntPerLevel = [2, 3];
            DexPerLevel = [2, 3];
            ResistancePerLevel = [1, 2];

            SetStr(136, 165);
            SetDex(50, 80);
            SetInt(52, 80);

            SetHits(160, 220);

            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Poison, 25);
            SetDamageType(ResistanceType.Energy, 75);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Fire, 5, 20);
            SetResistance(ResistanceType.Cold, 15, 30);
            SetResistance(ResistanceType.Poison, 10, 30);
            SetResistance(ResistanceType.Energy, 10, 15);

            SetSkill(SkillName.Wrestling, 45.4, 50.1);
            SetSkill(SkillName.Tactics, 45.4, 50.1);
            SetSkill(SkillName.MagicResist, 45.4, 50.1);
            SetSkill(SkillName.Anatomy, 45.4, 50.1);

            Tamable = true;
            ControlSlots = 4;
            MinTameSkill = 101.1;
        }

        public override string CorpseName => "a reptalon corpse";
        public override int TreasureMapLevel => 5;
        public override int Meat => 5;
        public override int Hides => 10;
        public override bool CanAngerOnTame => true;
        public override bool StatLossAfterTame => true;
        public override FoodType FavoriteFood => FoodType.Meat;
        public override bool CanFly => true;

        private static MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };
        public override MonsterAbility[] GetMonsterAbilities() => _abilities;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.AosUltraRich, 3);
        }

        public override WeaponAbility GetWeaponAbility() => WeaponAbility.ParalyzingBlow;
    }
}

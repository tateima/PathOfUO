using ModernUO.Serialization;
using Server.Items;
using Server.SkillHandlers;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class GreaterDragon : BaseCreature
    {
        [Constructible]
        public GreaterDragon() : base(AIType.AI_Mage)
        {
            Body = Utility.RandomList(12, 59);
            BaseSoundID = 362;

            SetStr(1025, 1425);
            SetDex(81, 148);
            SetInt(475, 675);

            SetHits(1000, 2000);
            SetStam(120, 135);

            SetDamage(24, 33);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 60, 85);
            SetResistance(ResistanceType.Fire, 65, 90);
            SetResistance(ResistanceType.Cold, 40, 55);
            SetResistance(ResistanceType.Poison, 40, 60);
            SetResistance(ResistanceType.Energy, 50, 75);

            SetSkill(SkillName.Meditation, 0);
            SetSkill(SkillName.EvalInt, 110.0, 140.0);
            SetSkill(SkillName.Magery, 110.0, 140.0);
            SetSkill(SkillName.Poisoning, 0);
            SetSkill(SkillName.Anatomy, 0);
            SetSkill(SkillName.MagicResist, 110.0, 140.0);
            SetSkill(SkillName.Tactics, 110.0, 140.0);
            SetSkill(SkillName.Wrestling, 115.0, 145.0);

            Fame = 22000;
            Karma = -15000;

            VirtualArmor = 60;

            Tamable = false;
            ControlSlots = 5;
            MinTameSkill = 104.7;
        }
        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder };
        public override string CorpseName => "a dragon corpse";
        public override bool StatLossAfterTame => true;
        public override string DefaultName => "a greater dragon";
        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        public override int TreasureMapLevel => 5;
        public override int Meat => 19;
        public override int Hides => 30;
        public override HideType HideType => HideType.Barbed;
        public override int Scales => 7;
        public override ScaleType ScaleType => Body == 12 ? ScaleType.Yellow : ScaleType.Red;
        public override FoodType FavoriteFood => FoodType.Meat;
        public override bool CanAngerOnTame => true;
        public override bool CanFly => true;

        private static MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };
        public override MonsterAbility[] GetMonsterAbilities() => _abilities;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 4);
            AddLoot(LootPack.Gems, 8);
        }

        public override WeaponAbility GetWeaponAbility() => WeaponAbility.BleedAttack;
    }
}

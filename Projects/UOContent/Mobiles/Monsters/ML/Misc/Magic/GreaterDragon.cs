using ModernUO.Serialization;
using Server.Items;

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

            LevelRange = [75, 80];
            StrPerLevel = [4, 10];
            IntPerLevel = [3, 8];
            DexPerLevel = [3, 5];
            ResistancePerLevel = [2, 4];

            SetStr(156, 195);
            SetDex(50, 80);
            SetInt(82, 140);

            SetHits(250, 290);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 35);
            SetResistance(ResistanceType.Fire, 10, 30);
            SetResistance(ResistanceType.Cold, 10, 30);
            SetResistance(ResistanceType.Poison, 5, 15);
            SetResistance(ResistanceType.Energy, 10, 25);

            SetSkill(SkillName.Meditation, 0);
            SetSkill(SkillName.EvalInt, 53.0, 63.5);
            SetSkill(SkillName.Magery, 53.0, 63.5);
            SetSkill(SkillName.Poisoning, 0);
            SetSkill(SkillName.Anatomy, 0);
            SetSkill(SkillName.MagicResist, 53.0, 63.5);
            SetSkill(SkillName.Tactics, 53.0, 63.5);
            SetSkill(SkillName.Wrestling, 53.0, 63.5);

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

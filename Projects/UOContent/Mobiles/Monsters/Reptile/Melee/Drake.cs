using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Drake : BaseCreature
    {
        [Constructible]
        public Drake() : base(AIType.AI_Melee)
        {
            Body = Utility.RandomList(60, 61);
            BaseSoundID = 362;

            SetStr(401, 430);
            SetDex(133, 152);
            SetInt(101, 140);

            SetHits(241, 258);

            SetDamage(11, 17);

            SetDamageType(ResistanceType.Physical, 80);
            SetDamageType(ResistanceType.Fire, 20);

            SetResistance(ResistanceType.Physical, 45, 50);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.MagicResist, 65.1, 80.0);
            SetSkill(SkillName.Tactics, 65.1, 90.0);
            SetSkill(SkillName.Wrestling, 65.1, 80.0);

            Fame = 5500;
            Karma = -5500;

            VirtualArmor = 46;

            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 81.3;

            PackReg(3);
        }

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder };
        public override string CorpseName => "a drake corpse";
        public override string DefaultName => "a drake";

        public override bool ReacquireOnMovement => true;
        public override int TreasureMapLevel => 2;
        public override int Meat => 10;
        public override int Hides => 20;
        public override HideType HideType => HideType.Horned;
        public override int Scales => 2;
        public override ScaleType ScaleType => Body == 60 ? ScaleType.Yellow : ScaleType.Red;
        public override FoodType FavoriteFood => FoodType.Meat | FoodType.Fish;
        public override bool CanFly => true;

        private static MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };
        public override MonsterAbility[] GetMonsterAbilities() => _abilities;

        public override void OnBeforeSpawn(Point3D location, Map m)
        {
            if (Utility.Random(1000) < 3 && this is not ManaDrake or PrismaticDrake)
            {
                BaseCreature creature = Utility.RandomBool() ? new ManaDrake() : new PrismaticDrake();
                creature.MoveToWorld(location, m);
                Delete();
            }
            else
            {
                base.OnBeforeSpawn(location, m);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.MedScrolls, 2);
        }
    }
}

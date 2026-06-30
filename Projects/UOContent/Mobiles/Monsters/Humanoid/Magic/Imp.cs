using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Imp : BaseCreature
    {
        [Constructible]
        public Imp() : base(AIType.AI_Mage)
        {
            Body = 74;
            BaseSoundID = 422;
            LevelRange = [15, 35];
            StrPerLevel = [1, 2];
            IntPerLevel = [2, 4];
            DexPerLevel = [3, 4];
            ResistancePerLevel = [1, 3];
            SetStr(20, 40);
            SetDex(26, 38);
            SetInt(56, 100);

            SetHits(55, 70);

            SetDamage(5, 9);

            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Fire, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Fire, 10, 30);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 20);
            SetResistance(ResistanceType.Energy, 5, 20);

            SetSkill(SkillName.EvalInt, 45.1, 50.0);
            SetSkill(SkillName.Magery, 45.1, 50.0);
            SetSkill(SkillName.MagicResist, 45.1, 50.0);
            SetSkill(SkillName.Tactics, 45.1, 50.0);
            SetSkill(SkillName.Wrestling, 45.1, 50.0);

            Fame = 2500;
            Karma = -2500;

            VirtualArmor = 30;

            Tamable = true;
            ControlSlots = 2;
            MinTameSkill = 83.1;
        }

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder };
        public override string CorpseName => "an imp corpse";
        public override string DefaultName => "an imp";

        public override int Meat => 1;
        public override int Hides => 6;
        public override HideType HideType => HideType.Spined;
        public override FoodType FavoriteFood => FoodType.Meat;
        public override PackInstinct PackInstinct => PackInstinct.Daemon;
        public override bool CanFly => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager);
            AddLoot(LootPack.MedScrolls, 2);
        }
    }
}

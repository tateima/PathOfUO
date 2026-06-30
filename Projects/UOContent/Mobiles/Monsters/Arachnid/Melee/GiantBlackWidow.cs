using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class GiantBlackWidow : BaseCreature
    {
        [Constructible]
        public GiantBlackWidow() : base(AIType.AI_Melee)
        {
            Body = 0x9D;
            BaseSoundID = 0x388; // TODO: validate

            LevelRange = [12, 22];
            StrPerLevel = [2, 7];
            IntPerLevel = [2, 3];
            DexPerLevel = [2, 9];
            ResistancePerLevel = [1, 2];
            SetStr(60, 75);
            SetDex(25, 65);
            SetInt(5, 11);

            SetHits(35, 70);

            SetDamage(2, 9);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 20);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.Anatomy, 40.3, 50.0);
            SetSkill(SkillName.Poisoning, 40.1, 50.0);
            SetSkill(SkillName.MagicResist, 45.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 3500;
            Karma = -3500;

            VirtualArmor = 24;

            PackItem(new SpidersSilk(5));
            PackItem(new LesserPoisonPotion());
            PackItem(new LesserPoisonPotion());
        }

        public override string CorpseName => "a giant black widow spider corpse";
        public override string DefaultName => "a giant black wide";

        public override FoodType FavoriteFood => FoodType.Meat;
        public override Poison PoisonImmune => Poison.Deadly;
        public override Poison HitPoison => Poison.Deadly;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
        }
    }
}

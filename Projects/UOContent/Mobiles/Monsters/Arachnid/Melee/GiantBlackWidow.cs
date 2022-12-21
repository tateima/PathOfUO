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

            SetStr(76, 100);
            SetDex(96, 115);
            SetInt(36, 60);

            SetHits(46, 60);

            SetDamage(5, 17);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 20, 30);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.Anatomy, 30.3, 75.0);
            SetSkill(SkillName.Poisoning, 60.1, 80.0);
            SetSkill(SkillName.MagicResist, 45.1, 60.0);
            SetSkill(SkillName.Tactics, 65.1, 80.0);
            SetSkill(SkillName.Wrestling, 70.1, 85.0);

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

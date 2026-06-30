using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class GiantSpider : BaseCreature
    {
        [Constructible]
        public GiantSpider() : base(AIType.AI_Melee)
        {
            Body = 28;
            BaseSoundID = 0x388;
            LevelRange = [5, 15];
            StrPerLevel = [2, 5];
            IntPerLevel = [1, 2];
            DexPerLevel = [4, 9];
            ResistancePerLevel = [1, 2];
            SetStr(35, 60);
            SetDex(39, 80);
            SetInt(5, 11);

            SetHits(30, 66);

            SetDamage(6, 8);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 20);

            SetSkill(SkillName.Poisoning, 45.1, 50.0);
            SetSkill(SkillName.MagicResist, 25.1, 40.0);
            SetSkill(SkillName.Tactics, 35.1, 50.0);
            SetSkill(SkillName.Wrestling, 45.1, 50.0);

            Fame = 600;
            Karma = -600;

            VirtualArmor = 16;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 61.1;

            PackItem(new SpidersSilk(5));
        }
        public override bool CanCannibalise(Mobile target) => base.CanCannibalise(target) || target is FrostSpider;
        public override string CorpseName => "a giant spider corpse";
        public override string DefaultName => "a giant spider";

        public override FoodType FavoriteFood => FoodType.Meat;
        public override PackInstinct PackInstinct => PackInstinct.Arachnid;
        public override Poison PoisonImmune => Poison.Regular;
        public override Poison HitPoison => Poison.Regular;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Poor);
        }
    }
}

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

            SetStr(76, 100);
            SetDex(76, 95);
            SetInt(36, 60);

            SetHits(46, 60);
            SetMana(0);

            SetDamage(5, 13);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 20);
            SetResistance(ResistanceType.Poison, 25, 35);

            SetSkill(SkillName.Poisoning, 60.1, 80.0);
            SetSkill(SkillName.MagicResist, 25.1, 40.0);
            SetSkill(SkillName.Tactics, 35.1, 50.0);
            SetSkill(SkillName.Wrestling, 50.1, 65.0);

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

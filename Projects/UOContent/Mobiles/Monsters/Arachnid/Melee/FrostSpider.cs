using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class FrostSpider : BaseCreature
    {
        [Constructible]
        public FrostSpider() : base(AIType.AI_Melee)
        {
            Body = 20;
            BaseSoundID = 0x388;

            LevelRange = [8, 18];
            StrPerLevel = [2, 7];
            IntPerLevel = [1, 2];
            DexPerLevel = [2, 6];
            ResistancePerLevel = [1, 2];
            SetStr(65, 70);
            SetDex(15, 35);
            SetInt(5, 11);

            SetHits(35, 70);

            SetDamage(7, 7);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Cold, 80);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 10, 30);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.MagicResist, 25.1, 40.0);
            SetSkill(SkillName.Tactics, 35.1, 50.0);
            SetSkill(SkillName.Wrestling, 45.1, 50.0);

            Fame = 775;
            Karma = -775;

            VirtualArmor = 28;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 74.7;

            PackItem(new SpidersSilk(7));
        }
        public override bool CanCannibalise(Mobile target) => base.CanCannibalise(target) || target is GiantSpider;

        public override string CorpseName => "a frost spider corpse";
        public override string DefaultName => "a frost spider";

        public override FoodType FavoriteFood => FoodType.Meat;
        public override PackInstinct PackInstinct => PackInstinct.Arachnid;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager);
            AddLoot(LootPack.Poor);
        }
    }
}

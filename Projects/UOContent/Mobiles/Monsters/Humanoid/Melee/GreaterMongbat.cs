using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class GreaterMongbat : BaseCreature
    {
        [Constructible]
        public GreaterMongbat() : base(AIType.AI_Melee)
        {
            Body = 39;
            BaseSoundID = 422;
            LevelRange = [6, 12];
            StrPerLevel = [1, 2];
            IntPerLevel = [3, 4];
            DexPerLevel = [5, 10];
            ResistancePerLevel = [1, 2];
            SetStr(56, 80);
            SetDex(61, 80);
            SetInt(26, 50);

            SetDamage(5, 7);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 15);

            SetSkill(SkillName.MagicResist, 15.1, 30.0);
            SetSkill(SkillName.Tactics, 35.1, 50.0);
            SetSkill(SkillName.Wrestling, 20.1, 35.0);

            Fame = 450;
            Karma = -450;

            VirtualArmor = 10;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 71.1;
        }

        public override string CorpseName => "a mongbat corpse";
        public override string DefaultName => "a greater mongbat";

        public override int Meat => 1;
        public override int Hides => 6;
        public override FoodType FavoriteFood => FoodType.Meat;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Poor);
        }
    }
}

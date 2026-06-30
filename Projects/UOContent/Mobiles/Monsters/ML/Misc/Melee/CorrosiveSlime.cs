using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class CorrosiveSlime : BaseCreature
    {
        [Constructible]
        public CorrosiveSlime() : base(AIType.AI_Melee)
        {
            Body = 51;
            BaseSoundID = 456;

            Hue = Utility.RandomSlimeHue();

            LevelRange = [2, 7];
            StrPerLevel = [1, 2];
            IntPerLevel = [1, 3];
            DexPerLevel = [1, 2];
            ResistancePerLevel = [1, 2];
            SetStr(18, 20);
            SetDex(16, 21);
            SetInt(16, 20);

            SetHits(13, 17);

            SetDamage(1, 3);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 10);
            SetResistance(ResistanceType.Poison, 15, 20);

            SetSkill(SkillName.Poisoning, 30.1, 50.0);
            SetSkill(SkillName.MagicResist, 15.1, 20.0);
            SetSkill(SkillName.Tactics, 19.3, 34.0);
            SetSkill(SkillName.Wrestling, 19.3, 34.0);

            Fame = 300;
            Karma = -300;

            VirtualArmor = 8;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 23.1;
        }

        // TODO: Damage weapon via acid

        public override string CorpseName => "a slimey corpse";
        public override string DefaultName => "a corrosive slime";

        public override Poison PoisonImmune => Poison.Regular;
        public override Poison HitPoison => Poison.Regular;
        public override FoodType FavoriteFood => FoodType.Fish;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Poor);
            AddLoot(LootPack.Gems);
        }
    }
}

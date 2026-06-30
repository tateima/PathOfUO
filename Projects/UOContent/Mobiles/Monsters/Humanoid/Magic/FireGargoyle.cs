using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class FireGargoyle : BaseCreature
    {
        [Constructible]
        public FireGargoyle() : base(AIType.AI_Mage)
        {
            Name = NameList.RandomName("fire gargoyle");
            Body = 130;
            BaseSoundID = 0x174;

            LevelRange = [10, 30];
            StrPerLevel = [3, 4];
            IntPerLevel = [4, 8];
            DexPerLevel = [3, 4];
            ResistancePerLevel = [1, 2];

            SetStr(50, 80);
            SetDex(20, 55);
            SetInt(25, 50);
            SetHits(75, 90);

            SetDamage(1, 8);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 80);

            SetResistance(ResistanceType.Physical, 10, 25);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.Anatomy, 40.1, 50.0);
            SetSkill(SkillName.EvalInt, 40.1, 50.0);
            SetSkill(SkillName.Magery, 40.1, 50.0);
            SetSkill(SkillName.Meditation, 40.1, 50.0);
            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 3500;
            Karma = -3500;

            VirtualArmor = 32;
        }

        public override string CorpseName => "a charred corpse";

        public override int TreasureMapLevel => 1;
        public override int Meat => 1;
        public override bool CanFly => true;

        private static MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };
        public override MonsterAbility[] GetMonsterAbilities() => _abilities;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems);
        }
    }
}

using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class HellHound : BaseCreature
    {
        [Constructible]
        public HellHound() : base(AIType.AI_Melee)
        {
            Body = 98;
            BaseSoundID = 229;

            LevelRange = [20, 30];
            StrPerLevel = [1, 8];
            IntPerLevel = [1, 2];
            DexPerLevel = [1, 9];
            ResistancePerLevel = [1, 2];

            SetStr(40, 115);
            SetDex(30, 55);
            SetInt(25, 30);
            SetHits(65, 80);
            SetDamage(3, 8);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 80);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Fire, 10, 30);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 3400;
            Karma = -3400;

            VirtualArmor = 30;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 85.5;

            PackItem(new SulfurousAsh(5));
        }

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder };
        public override string CorpseName => "a hell hound corpse";
        public override string DefaultName => "a hell hound";
        public override int Meat => 1;
        public override FoodType FavoriteFood => FoodType.Meat;
        public override PackInstinct PackInstinct => PackInstinct.Canine;

        private static MonsterAbility[] _abilities = { MonsterAbilities.FireBreath };
        public override MonsterAbility[] GetMonsterAbilities() => _abilities;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Meager);
        }
    }
}

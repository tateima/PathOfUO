using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class GargoyleDestroyer : BaseCreature
    {
        [Constructible]
        public GargoyleDestroyer() : base(AIType.AI_Mage)
        {
            Body = 0x2F3;
            BaseSoundID = 0x174;

            LevelRange = [20, 30];
            StrPerLevel = [3, 7];
            IntPerLevel = [4, 7];
            DexPerLevel = [3, 10];
            ResistancePerLevel = [1, 2];

            SetStr(70, 100);
            SetDex(20, 65);
            SetInt(25, 70);
            SetHits(75, 100);

            SetDamage(4, 9);

            SetResistance(ResistanceType.Physical, 10, 30);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 5, 15);
            SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.Wrestling, 45.1, 55.0);
            SetSkill(SkillName.Tactics, 45.1, 55.0);
            SetSkill(SkillName.MagicResist, 45.1, 55.0);
            SetSkill(SkillName.Anatomy, 45.1, 55.0);
            SetSkill(SkillName.Swords, 45.1, 55.0);
            SetSkill(SkillName.Macing, 45.1, 55.0);
            SetSkill(SkillName.Fencing, 45.1, 55.0);
            SetSkill(SkillName.Magery, 45.1, 55.0);
            SetSkill(SkillName.EvalInt, 45.1, 55.0);
            SetSkill(SkillName.Meditation, 45.1, 55.0);

            Fame = 10000;
            Karma = -10000;

            VirtualArmor = 50;

            if (Utility.RandomDouble() < 0.2)
            {
                PackItem(new GargoylesPickaxe());
            }
        }

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder };
        public override string CorpseName => "a gargoyle corpse";
        public override string DefaultName => "a gargoyle destroyer";

        public override bool BardImmune => !Core.AOS;
        public override int Meat => 1;
        public override bool CanFly => true;

        private static MonsterAbility[] _abilities = { MonsterAbilities.ThrowHatchetCounter };
        public override MonsterAbility[] GetMonsterAbilities() => _abilities;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.MedScrolls);
            AddLoot(LootPack.Gems, 2);
        }
    }
}

using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Gargoyle : BaseCreature
    {
        [Constructible]
        public Gargoyle() : base(AIType.AI_Mage)
        {
            Body = 4;
            BaseSoundID = 372;

            LevelRange = [8, 28];
            StrPerLevel = [3, 4];
            IntPerLevel = [4, 8];
            DexPerLevel = [3, 4];
            ResistancePerLevel = [1, 2];

            SetStr(50, 80);
            SetDex(20, 55);
            SetInt(25, 50);
            SetHits(75, 90);

            SetDamage(2, 7);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 25);
            SetResistance(ResistanceType.Fire, 5, 20);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 15);

            SetSkill(SkillName.EvalInt, 40.1, 50.0);
            SetSkill(SkillName.Magery, 40.1, 50.0);
            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 3500;
            Karma = -3500;

            VirtualArmor = 32;

            if (Utility.RandomDouble() < 0.025)
            {
                PackItem(new GargoylesPickaxe());
            }
        }
        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder };
        public override string CorpseName => "a gargoyle corpse";
        public override string DefaultName => "a gargoyle";

        public override bool CanFly => true;

        public override int TreasureMapLevel => 1;
        public override int Meat => 1;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.MedScrolls);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(1, 4));
        }
    }
}

using Server.Items;
using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class EnslavedGargoyle : BaseCreature
    {
        [Constructible]
        public EnslavedGargoyle() : base(AIType.AI_Melee)
        {
            Body = 0x2F1;
            BaseSoundID = 0x174;

            LevelRange = [5, 10];
            StrPerLevel = [2, 5];
            IntPerLevel = [3, 6];
            DexPerLevel = [3, 4];
            ResistancePerLevel = [2, 3];

            SetStr(50, 90);
            SetDex(10, 45);
            SetInt(5, 20);
            SetHits(45, 85);

            SetDamage(2, 5);

            SetResistance(ResistanceType.Physical, 10, 30);
            SetResistance(ResistanceType.Fire, 10, 30);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 5, 20);
            SetResistance(ResistanceType.Energy, 5, 20);

            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 3500;
            Karma = 0;

            VirtualArmor = 35;

            if (Utility.RandomDouble() < 0.2)
            {
                PackItem(new GargoylesPickaxe());
            }
        }

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder };
        public override string CorpseName => "an enslaved gargoyle corpse";
        public override string DefaultName => "an enslaved gargoyle";

        public override int Meat => 1;
        public override int TreasureMapLevel => 1;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average, 2);
            AddLoot(LootPack.Gems);
        }
    }
}

using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class StoneGargoyle : BaseCreature
    {
        [Constructible]
        public StoneGargoyle() : base(AIType.AI_Melee)
        {
            Body = 67;
            BaseSoundID = 0x174;

            LevelRange = [15, 20];
            StrPerLevel = [4, 7];
            IntPerLevel = [1, 2];
            DexPerLevel = [3, 4];
            ResistancePerLevel = [2, 3];

            SetStr(70, 130);
            SetDex(30, 55);
            SetInt(15, 40);
            SetHits(85, 130);

            SetDamage(4, 7);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 35);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 4000;
            Karma = -4000;

            VirtualArmor = 50;

            PackItem(new IronIngot(12));

            if (Utility.RandomDouble() < 0.05)
            {
                PackItem(new GargoylesPickaxe());
            }
        }
        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder };
        public override string CorpseName => "a gargoyle corpse";
        public override string DefaultName => "a stone gargoyle";

        public override int TreasureMapLevel => 2;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average, 2);
            AddLoot(LootPack.Gems, 1);
            AddLoot(LootPack.Potions);
        }
    }
}

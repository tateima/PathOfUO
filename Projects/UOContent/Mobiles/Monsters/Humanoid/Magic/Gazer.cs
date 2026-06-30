using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Gazer : BaseCreature
    {
        [Constructible]
        public Gazer() : base(AIType.AI_Mage)
        {
            Body = 22;
            BaseSoundID = 377;

            LevelRange = [5, 25];
            StrPerLevel = [1, 2];
            IntPerLevel = [1, 10];
            DexPerLevel = [1, 2];
            ResistancePerLevel = [2, 3];

            SetStr(46, 55);
            SetDex(31, 45);
            SetInt(66, 100);
            SetHits(50,99);

            SetDamage(2, 4);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 30);
            SetResistance(ResistanceType.Fire, 10, 30);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.EvalInt, 45.1, 50.0);
            SetSkill(SkillName.Magery, 45.1, 50.0);
            SetSkill(SkillName.MagicResist, 45.1, 50.0);
            SetSkill(SkillName.Tactics, 45.1, 50.0);
            SetSkill(SkillName.Wrestling, 45.1, 50.0);

            Fame = 3500;
            Karma = -3500;

            VirtualArmor = 36;

            PackItem(new Nightshade(4));
        }

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder };
        public override string CorpseName => "a gazer corpse";
        public override string DefaultName => "a gazer";

        public override int TreasureMapLevel => 1;
        public override int Meat => 1;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Potions);
        }
    }
}

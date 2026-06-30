using ModernUO.Serialization;
using Server.Engines.Plants;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class TerathanWarrior : BaseCreature
    {
        [Constructible]
        public TerathanWarrior() : base(AIType.AI_Melee)
        {
            Body = 70;
            BaseSoundID = 589;
            LevelRange = [13, 25];
            StrPerLevel = [2, 5];
            IntPerLevel = [1, 2];
            DexPerLevel = [4, 9];
            ResistancePerLevel = [1, 2];
            SetStr(56, 85);
            SetDex(50, 99);
            SetInt(21, 45);

            SetHits(52, 89);
            SetMana(0);

            SetDamage(2, 8);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 25);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.Poisoning, 45.1, 55.0);
            SetSkill(SkillName.MagicResist, 45.1, 55.0);
            SetSkill(SkillName.Tactics, 45.1, 55);
            SetSkill(SkillName.Wrestling, 45.1, 55.0);

            Fame = 4000;
            Karma = -4000;

            VirtualArmor = 30;

            if (Core.ML && Utility.RandomDouble() < .33)
            {
                PackItem(Seed.RandomPeculiarSeed(3));
            }
        }

        public override string CorpseName => "a terathan warrior corpse";
        public override string DefaultName => "a terathan warrior";

        public override int TreasureMapLevel => 1;
        public override int Meat => 4;

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.TerathansAndOphidians };

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
        }
    }
}

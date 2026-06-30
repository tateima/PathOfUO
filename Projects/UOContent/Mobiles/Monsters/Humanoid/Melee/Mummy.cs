using ModernUO.Serialization;
using Server.Engines.Plants;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Mummy : BaseCreature
    {
        [Constructible]
        public Mummy() : base(AIType.AI_Melee)
        {
            Body = 154;
            BaseSoundID = 471;

            LevelRange = [10, 13];
            StrPerLevel = [2, 4];
            IntPerLevel = [4, 6];
            DexPerLevel = [1, 3];
            ResistancePerLevel = [2, 3];

            SetStr(50, 135);
            SetDex(60, 85);
            SetInt(35, 50);
            SetHits(65, 85);
            SetDamage(3, 9);

            // SetStr(346, 370);
            // SetDex(71, 90);
            // SetInt(26, 40);
            //
            // SetHits(208, 222);
            //
            // SetDamage(13, 23);

            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Cold, 60);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Fire, 10, 10);
            SetResistance(ResistanceType.Cold, 10, 30);
            SetResistance(ResistanceType.Poison, 5, 20);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.MagicResist, 15.1, 40.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 4000;
            Karma = -4000;

            VirtualArmor = 50;

            if (Core.ML && Utility.RandomDouble() < .33)
            {
                PackItem(Seed.RandomPeculiarSeed(2));
            }

            PackItem(new Garlic(5));
            PackItem(new Bandage(10));
        }

        public override string CorpseName => "a mummy corpse";
        public override string DefaultName => "a mummy";

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lesser;

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems);
            AddLoot(LootPack.Potions);
        }
    }
}

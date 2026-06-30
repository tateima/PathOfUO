using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class WandererOfTheVoid : BaseCreature
    {
        [Constructible]
        public WandererOfTheVoid() : base(AIType.AI_Mage)
        {
            Body = 316;
            BaseSoundID = 377;

            LevelRange = [13, 25];
            StrPerLevel = [2, 4];
            IntPerLevel = [2, 4];
            DexPerLevel = [2, 4];
            ResistancePerLevel = [1, 2];

            SetStr(45, 80);
            SetDex(20, 40);
            SetInt(50, 90);

            SetHits(51, 130);

            SetDamage(1, 8);

            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Cold, 15);
            SetDamageType(ResistanceType.Energy, 85);

            SetResistance(ResistanceType.Physical, 10, 25);
            SetResistance(ResistanceType.Fire, 5, 15);
            SetResistance(ResistanceType.Cold, 10, 30);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.EvalInt, 45.1, 50.0);
            SetSkill(SkillName.Magery, 45.1, 50.0);
            SetSkill(SkillName.Meditation, 45.1, 50.0);
            SetSkill(SkillName.MagicResist, 45.1, 50.0);
            SetSkill(SkillName.Tactics, 45.1, 50.0);
            SetSkill(SkillName.Wrestling, 45.1, 50.0);

            Fame = 20000;
            Karma = -20000;

            VirtualArmor = 44;

            var count = Utility.RandomMinMax(2, 3);

            for (var i = 0; i < count; ++i)
            {
                PackItem(new TreasureMap(3, Map.Trammel));
            }
        }

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder };
        public override string CorpseName => "a wanderer of the void corpse";
        public override string DefaultName => "a wanderer of the void";

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int TreasureMapLevel => Core.AOS ? 4 : 1;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
        }
    }
}

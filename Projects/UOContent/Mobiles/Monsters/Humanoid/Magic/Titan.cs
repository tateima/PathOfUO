using ModernUO.Serialization;
using Server.Engines.Plants;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Titan : BaseCreature
    {
        [Constructible]
        public Titan() : base(AIType.AI_Mage)
        {
            Body = 76;
            BaseSoundID = 609;
            LevelRange = [35, 55];
            StrPerLevel = [1, 5];
            IntPerLevel = [1, 7];
            DexPerLevel = [1, 2];
            ResistancePerLevel = [1, 2];

            SetStr(40, 120);
            SetDex(10, 45);
            SetInt(70, 120);
            SetHits(85, 100);
            SetDamage(1, 5);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 30);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.EvalInt, 40.0, 50.5);
            SetSkill(SkillName.Magery, 40.0, 50.5);
            SetSkill(SkillName.MagicResist, 40.0, 50.5);
            SetSkill(SkillName.Tactics, 40.0, 50.5);
            SetSkill(SkillName.Wrestling, 40.0, 50.5);

            Fame = 11500;
            Karma = -11500;

            VirtualArmor = 40;

            if (Core.ML && Utility.RandomDouble() < .33)
            {
                PackItem(Seed.RandomPeculiarSeed(1));
            }
        }

        public override string CorpseName => "a titans corpse";
        public override string DefaultName => "a titan";

        public override int Meat => 4;
        public override Poison PoisonImmune => Poison.Regular;
        public override int TreasureMapLevel => 5;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.MedScrolls);
        }
    }
}

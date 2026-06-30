using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Reaper : BaseCreature
    {
        [Constructible]
        public Reaper() : base(AIType.AI_Mage)
        {
            Body = 47;
            BaseSoundID = 442;
            LevelRange = [10, 40];
            StrPerLevel = [1, 2];
            IntPerLevel = [1, 5];
            DexPerLevel = [1, 1];
            ResistancePerLevel = [1, 2];
            SetStr(35, 60);
            SetDex(20, 25);
            SetInt(50, 90);

            SetHits(40, 129);
            SetStam(0);

            SetDamage(3, 6);

            SetDamageType(ResistanceType.Physical, 80);
            SetDamageType(ResistanceType.Poison, 20);

            SetResistance(ResistanceType.Physical, 5, 10);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 15, 25);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.EvalInt, 40.1, 50.0);
            SetSkill(SkillName.Magery, 40.1, 50.0);
            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 45.1, 60.0);
            SetSkill(SkillName.Wrestling, 50.1, 60.0);
            SetSkill(SkillName.Wrestling, 50.1, 60.0);

            Fame = 3500;
            Karma = -3500;

            VirtualArmor = 40;

            PackItem(new Log(10));
            PackItem(new MandrakeRoot(5));
        }

        public override string CorpseName => "a reapers corpse";
        public override string DefaultName => "a reaper";

        public override Poison PoisonImmune => Poison.Greater;
        public override int TreasureMapLevel => 2;
        public override bool DisallowAllMoves => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
        }
    }
}

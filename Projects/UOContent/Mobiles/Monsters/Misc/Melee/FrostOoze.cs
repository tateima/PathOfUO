using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class FrostOoze : BaseCreature
    {
        [Constructible]
        public FrostOoze() : base(AIType.AI_Melee)
        {
            Body = 94;
            BaseSoundID = 456;
            LevelRange = [2, 7];
            StrPerLevel = [1, 3];
            IntPerLevel = [1, 3];
            DexPerLevel = [1, 3];
            ResistancePerLevel = [1, 2];
            SetStr(18, 20);
            SetDex(16, 21);
            SetInt(16, 20);

            SetHits(13, 17);

            SetDamage(2, 4);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 20);
            SetResistance(ResistanceType.Cold, 15, 30);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.MagicResist, 5.1, 10.0);
            SetSkill(SkillName.Tactics, 19.3, 34.0);
            SetSkill(SkillName.Wrestling, 25.3, 40.0);

            Fame = 450;
            Karma = -450;

            VirtualArmor = 38;
        }

        public override string CorpseName => "a frost ooze corpse";
        public override string DefaultName => "a frost ooze";

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Gems, Utility.RandomMinMax(1, 2));
        }
    }
}

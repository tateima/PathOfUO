using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class MantraEffervescence : BaseCreature
    {
        [Constructible]
        public MantraEffervescence()
            : base(AIType.AI_Mage)
        {
            Body = 0x111;
            BaseSoundID = 0x56E;
            LevelRange = [10, 50];
            StrPerLevel = [1, 2];
            IntPerLevel = [2, 4];
            DexPerLevel = [2, 3];
            ResistancePerLevel = [1, 2];
            SetStr(30, 50);
            SetDex(20, 50);
            SetInt(50, 80);

            SetHits(50, 90);

            SetDamage(1, 6);

            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Energy, 70);

            SetResistance(ResistanceType.Physical, 5, 15);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 15);
            SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.Wrestling, 40.0, 60.5);
            SetSkill(SkillName.Tactics, 40.0, 60.5);
            SetSkill(SkillName.MagicResist, 40.0, 60.5);
            SetSkill(SkillName.Magery, 40.0, 60.5);
            SetSkill(SkillName.EvalInt, 40.0, 60.5);
            SetSkill(SkillName.Meditation, 40.0, 60.5);

            Fame = 6500;
            Karma = -6500;
        }

        public override string CorpseName => "a mantra effervescence corpse";
        public override string DefaultName => "a mantra effervescence";

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Rich);
        }
    }
}

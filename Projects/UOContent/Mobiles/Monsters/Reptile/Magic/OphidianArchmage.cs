using ModernUO.Serialization;

namespace Server.Mobiles
{
    [TypeAlias("Server.Mobiles.OphidianJusticar", "Server.Mobiles.OphidianZealot")]
    [SerializationGenerator(0, false)]
    public partial class OphidianArchmage : BaseCreature
    {
        private static readonly string[] m_Names =
        {
            "an ophidian justicar",
            "an ophidian zealot"
        };

        [Constructible]
        public OphidianArchmage() : base(AIType.AI_Mage)
        {
            Name = m_Names.RandomElement();
            Body = 85;
            BaseSoundID = 639;

            SetStr(281, 305);
            SetDex(191, 215);
            SetInt(226, 250);

            SetHits(169, 183);
            SetStam(36, 45);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40, 45);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 25, 35);
            SetResistance(ResistanceType.Poison, 35, 40);
            SetResistance(ResistanceType.Energy, 25, 35);

            SetSkill(SkillName.EvalInt, 95.1, 100.0);
            SetSkill(SkillName.Magery, 95.1, 100.0);
            SetSkill(SkillName.MagicResist, 75.0, 97.5);
            SetSkill(SkillName.Tactics, 65.0, 87.5);
            SetSkill(SkillName.Wrestling, 20.2, 60.0);

            Fame = 11500;
            Karma = -11500;

            VirtualArmor = 44;

            PackReg(5, 15);
            PackNecroReg(5, 15);
        }

        public override string CorpseName => "an ophidian corpse";

        public override int Meat => 1;

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.TerathansAndOphidians, OppositionGroup.ChaosAndOrder };
        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.MedScrolls, 2);
        }
    }
}

using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Wraith : BaseCreature
    {
        [Constructible]
        public Wraith() : base(AIType.AI_Mage)
        {
            Body = 26;
            Hue = 0x4001;
            BaseSoundID = 0x482;

            LevelRange = [4, 10];
            StrPerLevel = [1, 2];
            IntPerLevel = [1, 4];
            DexPerLevel = [1, 2];
            ResistancePerLevel = [1, 3];

            SetStr(36, 70);
            SetDex(46, 55);
            SetInt(66, 90);

            SetHits(46, 60);

            SetDamage(3, 6);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50);

            SetResistance(ResistanceType.Physical, 5, 20);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 5, 10);

            SetSkill(SkillName.EvalInt, 45.1, 50.0);
            SetSkill(SkillName.Magery, 45.1, 50.0);
            SetSkill(SkillName.MagicResist, 45.1, 50.0);
            SetSkill(SkillName.Tactics, 45.1, 50.0);
            SetSkill(SkillName.Wrestling, 45.1, 50.0);

            Fame = 4000;
            Karma = -4000;

            VirtualArmor = 28;

            PackReg(10);
        }

        public override string CorpseName => "a ghostly corpse";
        public override string DefaultName => "a wraith";

        public override bool BleedImmune => true;

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };

        public override Poison PoisonImmune => Poison.Lethal;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager);
        }
    }
}

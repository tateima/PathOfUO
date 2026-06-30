using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [TypeAlias("Server.Mobiles.BoneMage")]
    [SerializationGenerator(0, false)]
    public partial class BoneMagi : BaseCreature
    {
        [Constructible]
        public BoneMagi() : base(AIType.AI_Mage)
        {
            Body = 148;
            BaseSoundID = 451;
            LevelRange = [7, 22];
            StrPerLevel = [1, 3];
            IntPerLevel = [1, 2];
            DexPerLevel = [1, 2];
            ResistancePerLevel = [2, 3];
            SetStr(20, 60);
            SetDex(10, 45);
            SetInt(45, 90);
            SetHits(75, 90);
            SetStam(46, 75);

            SetDamage(3, 6);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 20);
            SetResistance(ResistanceType.Fire, 6, 10);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 6, 20);

            SetSkill(SkillName.EvalInt, 45.1, 50.0);
            SetSkill(SkillName.Magery, 45.1, 50.0);
            SetSkill(SkillName.MagicResist, 45.1, 50.0);
            SetSkill(SkillName.Tactics, 45.1, 50.0);
            SetSkill(SkillName.Wrestling, 45.1, 50.0);
            SetSkill(SkillName.Necromancy, 45.1, 50.0);
            SetSkill(SkillName.SpiritSpeak, 45.1, 50.0);

            Fame = 3000;
            Karma = -3000;

            VirtualArmor = 38;

            PackReg(3);
            PackNecroReg(3, 10);
            PackItem(new Bone());
        }

        public override string CorpseName => "a skeletal corpse";
        public override string DefaultName => "a bone mage";

        public override bool BleedImmune => true;

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };

        public override Poison PoisonImmune => Poison.Regular;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.LowScrolls);
            AddLoot(LootPack.Potions);
        }
    }
}

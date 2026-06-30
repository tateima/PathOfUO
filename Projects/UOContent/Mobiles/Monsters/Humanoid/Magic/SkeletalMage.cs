using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class SkeletalMage : BaseCreature
    {
        [Constructible]
        public SkeletalMage() : base(AIType.AI_Mage)
        {
            Body = 148;
            BaseSoundID = 451;
            LevelRange = [5, 20];
            StrPerLevel = [1, 3];
            IntPerLevel = [1, 2];
            DexPerLevel = [1, 2];
            ResistancePerLevel = [2, 3];
            SetStr(20, 30);
            SetDex(10, 35);
            SetInt(35, 70);
            SetHits(55, 60);
            SetStam(46, 55);

            SetDamage(3, 5);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 20);
            SetResistance(ResistanceType.Fire, 5, 20);
            SetResistance(ResistanceType.Cold, 5, 20);
            SetResistance(ResistanceType.Poison, 5, 20);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.EvalInt, 40.1, 50.0);
            SetSkill(SkillName.Magery, 40.1, 50.0);
            SetSkill(SkillName.MagicResist, 30.1, 40.0);
            SetSkill(SkillName.Tactics, 30.1, 40.0);
            SetSkill(SkillName.Wrestling, 30.1, 40.0);
            SetSkill(SkillName.Necromancy, 50.1, 60.0);
            SetSkill(SkillName.SpiritSpeak, 30.1, 40.0);

            Fame = 3000;
            Karma = -3000;

            VirtualArmor = 38;
            PackReg(3);
            PackNecroReg(3, 10);
            PackItem(new Bone());
        }

        public override string CorpseName => "a skeletal corpse";
        public override string DefaultName => "a skeletal mage";

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

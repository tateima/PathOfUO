using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class IceElemental : BaseCreature
    {
        [Constructible]
        public IceElemental() : base(AIType.AI_Mage)
        {
            Body = 161;
            BaseSoundID = 268;

            LevelRange = [19, 32];
            StrPerLevel = [2, 5];
            IntPerLevel = [1, 5];
            DexPerLevel = [1, 2];
            ResistancePerLevel = [1, 3];

            SetStr(50, 155);
            SetDex(30, 55);
            SetInt(55, 70);
            SetHits(85, 120);

            SetDamage(3, 8);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Cold, 75);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.EvalInt, 30.1, 60.0);
            SetSkill(SkillName.Magery, 30.1, 60.0);
            SetSkill(SkillName.MagicResist, 30.1, 60.0);
            SetSkill(SkillName.Tactics, 30.1, 60.0);
            SetSkill(SkillName.Wrestling, 30.1, 60.0);

            Fame = 4000;
            Karma = -4000;

            VirtualArmor = 40;

            PackItem(new BlackPearl());
            PackReg(3);
        }
        public override string CorpseName => "an ice elemental corpse";
        public override string DefaultName => "an ice elemental";
        public override bool BleedImmune => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average, 2);
            AddLoot(LootPack.Gems, 2);
        }
    }
}

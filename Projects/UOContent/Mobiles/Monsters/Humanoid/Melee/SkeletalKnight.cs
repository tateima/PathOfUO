using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class SkeletalKnight : BaseCreature
    {
        [Constructible]
        public SkeletalKnight() : base(AIType.AI_Melee)
        {
            Body = 147;
            BaseSoundID = 451;

            LevelRange = [15, 20];
            StrPerLevel = [2, 4];
            IntPerLevel = [1, 2];
            DexPerLevel = [1, 3];
            ResistancePerLevel = [2, 3];

            SetStr(50, 135);
            SetDex(30, 55);
            SetInt(25, 30);
            SetHits(75, 90);
            SetStam(46, 55);

            SetDamage(5, 9);

            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Cold, 60);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 10, 30);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 20);

            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 3000;
            Karma = -3000;

            VirtualArmor = 40;

            switch (Utility.Random(6))
            {
                case 0:
                    PackItem(new PlateArms());
                    break;
                case 1:
                    PackItem(new PlateChest());
                    break;
                case 2:
                    PackItem(new PlateGloves());
                    break;
                case 3:
                    PackItem(new PlateGorget());
                    break;
                case 4:
                    PackItem(new PlateLegs());
                    break;
                case 5:
                    PackItem(new PlateHelm());
                    break;
            }

            PackItem(new Scimitar());
            PackItem(new WoodenShield());
        }

        public override string CorpseName => "a skeletal corpse";
        public override string DefaultName => "a skeletal knight";

        public override bool BleedImmune => true;

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Meager);
        }
    }
}

using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class BoneKnight : BaseCreature
    {
        [Constructible]
        public BoneKnight() : base(AIType.AI_Melee)
        {
            Body = 57;
            BaseSoundID = 451;

            LevelRange = [15, 20];
            StrPerLevel = [3, 6];
            IntPerLevel = [1, 2];
            DexPerLevel = [3, 5];
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
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 10, 15);

            SetSkill(SkillName.MagicResist, 40.0, 50.5);
            SetSkill(SkillName.Tactics, 40.0, 50.5);
            SetSkill(SkillName.Wrestling, 40.0, 50.5);

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

            PackSlayer();
            PackItem(new Scimitar());
            PackItem(new WoodenShield());
        }

        public override string CorpseName => "a skeletal corpse";
        public override string DefaultName => "a bone knight";

        public override bool BleedImmune => true;

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Meager);
        }
    }
}

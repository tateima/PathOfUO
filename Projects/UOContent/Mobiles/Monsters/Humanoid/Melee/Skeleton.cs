using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Skeleton : BaseCreature
    {
        [Constructible]
        public Skeleton() : base(AIType.AI_Melee)
        {
            Body = Utility.RandomList(50, 56);
            BaseSoundID = 0x48D;
            LevelRange = [1, 6];
            StrPerLevel = [1, 3];
            IntPerLevel = [1, 2];
            DexPerLevel = [1, 2];
            ResistancePerLevel = [2, 3];

            SetStr(20, 40);
            SetDex(10, 35);
            SetInt(15, 20);
            SetHits(35, 60);
            SetStam(46, 55);

            SetDamage(3, 5);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 10);
            SetResistance(ResistanceType.Fire, 1, 6);
            SetResistance(ResistanceType.Cold, 5, 20);
            SetResistance(ResistanceType.Poison, 5, 25);
            SetResistance(ResistanceType.Energy, 1, 10);

            SetSkill(SkillName.MagicResist, 30.1, 40.0);
            SetSkill(SkillName.Tactics, 30.1, 40.0);
            SetSkill(SkillName.Wrestling, 30.1, 40.0);

            Fame = 450;
            Karma = -450;

            VirtualArmor = 16;

            switch (Utility.Random(5))
            {
                case 0:
                    {
                        PackItem(new BoneArms());
                        break;
                    }
                case 1:
                    {
                        PackItem(new BoneChest());
                        break;
                    }
                case 2:
                    {
                        PackItem(new BoneGloves());
                        break;
                    }
                case 3:
                    {
                        PackItem(new BoneLegs());
                        break;
                    }
                case 4:
                    {
                        PackItem(new BoneHelm());
                        break;
                    }
            }
        }

        public override string CorpseName => "a skeletal corpse";
        public override string DefaultName => "a skeleton";

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lesser;

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Poor);
        }
    }
}

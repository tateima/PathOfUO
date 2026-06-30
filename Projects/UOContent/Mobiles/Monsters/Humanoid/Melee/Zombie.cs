using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Zombie : BaseCreature
    {
        [Constructible]
        public Zombie() : base(AIType.AI_Melee)
        {
            Body = 3;
            BaseSoundID = 471;

            LevelRange = [2, 7];
            StrPerLevel = [1, 4];
            IntPerLevel = [1, 2];
            DexPerLevel = [1, 2];
            ResistancePerLevel = [1, 2];

            SetStr(30, 50);
            SetDex(10, 45);
            SetInt(5, 10);
            SetHits(25, 70);

            SetDamage(2, 6);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 10);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 1, 10);

            SetSkill(SkillName.MagicResist, 30.1, 40.0);
            SetSkill(SkillName.Tactics, 35.1, 50.0);
            SetSkill(SkillName.Wrestling, 35.1, 50.0);

            Fame = 600;
            Karma = -600;

            VirtualArmor = 18;

            PackItem(
                Utility.Random(10) switch
                {
                    0 => new LeftArm(),
                    1 => new RightArm(),
                    2 => new Torso(),
                    3 => new Bone(),
                    4 => new RibCage(),
                    5 => new RibCage(),
                    _ => new BonePile() // 6-9
                }
            );
        }

        public override string CorpseName => "a rotting corpse";
        public override string DefaultName => "a zombie";

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Regular;

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager);
        }
    }
}

using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class GazerLarva : BaseCreature
    {
        [Constructible]
        public GazerLarva() : base(AIType.AI_Melee)
        {
            Body = 778;
            BaseSoundID = 377;

            LevelRange = [1, 10];
            StrPerLevel = [1, 4];
            IntPerLevel = [1, 2];
            DexPerLevel = [1, 2];
            ResistancePerLevel = [1, 2];

            SetStr(46, 70);
            SetDex(31, 45);
            SetInt(46, 70);

            SetDamage(2, 6);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 15);

            SetSkill(SkillName.MagicResist, 40.0);
            SetSkill(SkillName.Tactics, 40.0);
            SetSkill(SkillName.Wrestling, 40.0);

            Fame = 900;
            Karma = -900;

            VirtualArmor = 25;

            PackItem(new Nightshade(Utility.RandomMinMax(2, 3)));
        }

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder };
        public override string CorpseName => "a gazer larva corpse";
        public override string DefaultName => "a gazer larva";

        public override int Meat => 1;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Poor);
        }
    }
}

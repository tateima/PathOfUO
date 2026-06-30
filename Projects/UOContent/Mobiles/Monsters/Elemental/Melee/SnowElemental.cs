using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class SnowElemental : BaseCreature
    {
        [Constructible]
        public SnowElemental() : base(AIType.AI_Melee)
        {
            Body = 163;
            BaseSoundID = 263;
            LevelRange = [22, 34];
            StrPerLevel = [2, 7];
            IntPerLevel = [1, 2];
            DexPerLevel = [2, 5];
            ResistancePerLevel = [1, 3];

            SetStr(50, 90);
            SetDex(20, 55);
            SetInt(15, 40);
            SetHits(95, 150);
            SetDamage(1, 11);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Cold, 80);

            SetResistance(ResistanceType.Physical, 15, 35);
            SetResistance(ResistanceType.Fire, 1, 5);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 5, 15);
            SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 50;

            PackItem(new BlackPearl(3));
            PackItem(new IronOre(3)
            {
                ItemID = 0x19B8
            });
        }

        public override string CorpseName => "a snow elemental corpse";
        public override string DefaultName => "a snow elemental";

        public override bool BleedImmune => true;

        public override int TreasureMapLevel => Utility.RandomList(2, 3);

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
        }
    }
}

using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class EarthElemental : BaseCreature
    {
        [Constructible]
        public EarthElemental() : base(AIType.AI_Melee)
        {
            Body = 14;
            BaseSoundID = 268;

            LevelRange = [19, 32];
            StrPerLevel = [1, 4];
            IntPerLevel = [1, 2];
            DexPerLevel = [2, 5];
            ResistancePerLevel = [1, 3];

            SetStr(70, 95);
            SetDex(30, 75);
            SetInt(25, 50);
            SetHits(85, 130);
            SetDamage(2, 8);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 20, 35);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 1, 5);
            SetResistance(ResistanceType.Energy, 1, 5);

            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 3500;
            Karma = -3500;

            VirtualArmor = 34;

            PackItem(new FertileDirt(Utility.RandomMinMax(1, 4)));
            PackItem(new MandrakeRoot());

            PackItem(new IronOre(5)
            {
                ItemID = 0x19B7
            });
        }

        public override string CorpseName => "an earth elemental corpse";
        public override string DefaultName => "an earth elemental";

        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 1;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Meager);
            AddLoot(LootPack.Gems);
        }
    }
}

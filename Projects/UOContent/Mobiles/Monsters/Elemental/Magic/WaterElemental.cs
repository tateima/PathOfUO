using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class WaterElemental : BaseCreature
    {
        [Constructible]
        public WaterElemental() : base(AIType.AI_Mage)
        {
            Body = 16;
            BaseSoundID = 278;

            LevelRange = [22, 34];
            StrPerLevel = [2, 3];
            IntPerLevel = [1, 4];
            DexPerLevel = [2, 4];
            ResistancePerLevel = [1, 3];

            SetStr(50, 70);
            SetDex(20, 65);
            SetInt(55, 95);
            SetHits(95, 150);
            SetDamage(1, 6);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Fire, 5, 15);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 1, 5);

            SetSkill(SkillName.EvalInt, 40.1, 50.0);
            SetSkill(SkillName.Magery, 40.1, 50.0);
            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 4500;
            Karma = -4500;

            VirtualArmor = 40;

            CanSwim = true;

            PackItem(new BlackPearl(3));
        }

        public override string CorpseName => "a water elemental corpse";
        public override string DefaultName => "a water elemental";

        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 2;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Meager);
            AddLoot(LootPack.Potions);
        }
    }
}

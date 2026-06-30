using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class FireElemental : BaseCreature
    {
        [Constructible]
        public FireElemental() : base(AIType.AI_Mage)
        {
            Body = 15;
            BaseSoundID = 838;

            LevelRange = [16, 35];
            StrPerLevel = [2, 4];
            IntPerLevel = [1, 4];
            DexPerLevel = [1, 4];
            ResistancePerLevel = [1, 3];

            SetStr(50, 155);
            SetDex(30, 55);
            SetInt(55, 70);
            SetHits(85, 120);

            SetDamage(4, 7);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Fire, 75);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 20);
            SetResistance(ResistanceType.Energy, 5, 20);

            SetSkill(SkillName.EvalInt, 30.1, 60.0);
            SetSkill(SkillName.Magery, 30.1, 60.0);
            SetSkill(SkillName.MagicResist, 30.1, 60.0);
            SetSkill(SkillName.Tactics, 30.1, 60.0);
            SetSkill(SkillName.Wrestling, 30.1, 60.0);

            Fame = 4500;
            Karma = -4500;

            VirtualArmor = 40;

            PackItem(new SulfurousAsh(3));

            AddItem(new LightSource());
        }

        public override string CorpseName => "a fire elemental corpse";
        public override string DefaultName => "a fire elemental";

        public override bool BleedImmune => true;

        public override int TreasureMapLevel => 2;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Meager);
            AddLoot(LootPack.Gems);
        }
    }
}

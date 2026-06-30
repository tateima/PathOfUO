using ModernUO.Serialization;

namespace Server.Mobiles
{
    [TypeAlias("Server.Mobiles.ToxicElemental")]
    [SerializationGenerator(0, false)]
    public partial class AcidElemental : BaseCreature
    {
        [Constructible]
        public AcidElemental() : base(AIType.AI_Mage)
        {
            Body = 0x9E;
            BaseSoundID = 278;

            LevelRange = [27, 39];
            StrPerLevel = [2, 7];
            IntPerLevel = [1, 4];
            DexPerLevel = [2, 5];
            ResistancePerLevel = [1, 3];

            SetStr(50, 135);
            SetDex(20, 75);
            SetInt(15, 80);
            SetHits(95, 150);
            SetDamage(2, 9);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Fire, 50);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 1, 10);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.Anatomy, 40.1, 50.0);
            SetSkill(SkillName.EvalInt, 40.1, 50.0);
            SetSkill(SkillName.Magery, 40.1, 50.0);
            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 10000;
            Karma = -10000;

            VirtualArmor = 40;
        }

        public override string CorpseName => "an acid elemental corpse";
        public override string DefaultName => "an acid elemental";

        public override bool BleedImmune => true;
        public override Poison HitPoison => Poison.Lethal;
        public override double HitPoisonChance => 0.6;

        public override int TreasureMapLevel => Core.AOS ? 2 : 3;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Average);
        }
    }
}

using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class PoisonElemental : BaseCreature
    {
        [Constructible]
        public PoisonElemental() : base(AIType.AI_Mage)
        {
            Body = 162;
            BaseSoundID = 263;

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

            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Poison, 90);

            SetResistance(ResistanceType.Physical, 10, 30);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 50);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.EvalInt, 40.1, 50.0);
            SetSkill(SkillName.Magery, 40.1, 50.0);
            SetSkill(SkillName.Meditation, 40.1, 50.0);
            SetSkill(SkillName.Poisoning, 60.1, 70.0);
            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 12500;
            Karma = -12500;

            VirtualArmor = 70;
            ControlSlots = 4;

            PackItem(new Nightshade(4));
            PackItem(new LesserPoisonPotion());
        }

        public override string CorpseName => "a poison elementals corpse";
        public override string DefaultName => "a poison elemental";

        public override double DispelDifficulty => 150.5;
        public override double DispelFocus => 45.0;

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;

        public override Poison HitPoison => Poison.Lethal;
        public override double HitPoisonChance => 0.75;

        public override int TreasureMapLevel => 5;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.MedScrolls);
        }
    }
}

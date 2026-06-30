using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class BloodElemental : BaseCreature
    {
        [Constructible]
        public BloodElemental() : base(AIType.AI_Mage)
        {
            Body = 159;
            BaseSoundID = 278;
            LevelRange = [32, 44];
            StrPerLevel = [3, 5];
            IntPerLevel = [1, 6];
            DexPerLevel = [2, 4];
            ResistancePerLevel = [1, 3];

            SetStr(50, 70);
            SetDex(20, 65);
            SetInt(55, 95);
            SetHits(95, 150);
            SetDamage(1, 6);

            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Poison, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Fire, 5, 15);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.EvalInt, 40.1, 50.0);
            SetSkill(SkillName.Magery, 40.1, 50.0);
            SetSkill(SkillName.Meditation, 40.1, 50.0);
            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 12500;
            Karma = -12500;

            VirtualArmor = 60;
        }

        public override string CorpseName => "a blood elemental corpse";
        public override string DefaultName => "a blood elemental";

        public override int TreasureMapLevel => 5;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Rich);
        }
    }
}

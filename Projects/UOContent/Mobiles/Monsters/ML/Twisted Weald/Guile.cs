using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Guile : Changeling
    {
        [Constructible]
        public Guile()
        {
            IsParagon = true;

            Hue = DefaultHue;

            LevelRange = [35, 65];
            StrPerLevel = [1, 2];
            IntPerLevel = [2, 4];
            DexPerLevel = [3, 4];
            ResistancePerLevel = [1, 3];
            SetStr(36, 55);
            SetDex(20, 30);
            SetInt(45, 70);

            SetHits(55, 70);

            SetDamage(6, 7);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 15);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 5, 20);
            SetResistance(ResistanceType.Poison, 5, 30);
            SetResistance(ResistanceType.Energy, 5, 20);

            SetSkill(SkillName.Wrestling, 10.4, 12.5);
            SetSkill(SkillName.Tactics,  45.1, 50.0);
            SetSkill(SkillName.MagicResist,  45.1, 50.0);
            SetSkill(SkillName.Magery,  45.1, 50.0);
            SetSkill(SkillName.EvalInt,  45.1, 50.0);
            SetSkill(SkillName.Meditation,  45.1, 50.0);

            Fame = 21000;
            Karma = -21000;
        }

        public override string CorpseName => "a Guile corpse";
        public override string DefaultName => "Guile";
        public override int DefaultHue => 0x3F;
        public override bool GivesMLMinorArtifact => true;

        private static MonsterAbility[] _abilities = { MonsterAbilities.DrainLifeAttack };
        public override MonsterAbility[] GetMonsterAbilities() => _abilities;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
        }
    }
}

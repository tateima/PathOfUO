using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Spite : Changeling
    {
        [Constructible]
        public Spite()
        {
            IsParagon = true;

            Hue = DefaultHue;

            LevelRange = [50, 70];
            StrPerLevel = [1, 3];
            IntPerLevel = [1, 2];
            DexPerLevel = [2, 4];
            ResistancePerLevel = [1, 3];
            SetStr(35, 55);
            SetDex(40, 60);
            SetInt(30, 56);

            SetHits(70, 150);
            SetDamage(6, 9);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 20);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 15, 25);
            SetResistance(ResistanceType.Energy, 5, 20);

            SetSkill(SkillName.Wrestling, 12.8, 16.7);
            SetSkill(SkillName.Tactics,  45.1, 50.0);
            SetSkill(SkillName.MagicResist,  45.1, 50.0);
            SetSkill(SkillName.Magery,  45.1, 50.0);
            SetSkill(SkillName.EvalInt,  45.1, 50.0);
            SetSkill(SkillName.Meditation,  45.1, 50.0);

            Fame = 21000;
            Karma = -21000;
        }

        public override string CorpseName => "a Spite corpse";
        public override string DefaultName => "Spite";
        public override int DefaultHue => 0x21;

        public override bool GivesMLMinorArtifact => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
        }
    }
}

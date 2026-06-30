using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class MougGuur : Ettin
    {
        [Constructible]
        public MougGuur()
        {
            LevelRange = [40, 70];
            StrPerLevel = [1, 8];
            IntPerLevel = [1, 2];
            DexPerLevel = [1, 4];
            ResistancePerLevel = [1, 2];

            SetStr(60, 95);
            SetDex(30, 55);
            SetInt(25, 30);
            SetHits(85, 100);
            SetDamage(1, 8);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 20);
            SetResistance(ResistanceType.Fire, 5, 15);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 15);
            SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.MagicResist, 45.2, 65.0);
            SetSkill(SkillName.Tactics, 50.8, 61.7);
            SetSkill(SkillName.Wrestling, 43.9, 59.4);

            Fame = 3000;
            Karma = -3000;
        }

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };
        public override string CorpseName => "a Moug-Guur corpse";
        public override string DefaultName => "Moug-Guur";
    }
}

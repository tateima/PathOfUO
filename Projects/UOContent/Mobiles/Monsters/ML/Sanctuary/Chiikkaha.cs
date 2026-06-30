using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Chiikkaha : RatmanMage
    {
        [Constructible]
        public Chiikkaha()
        {
            LevelRange = [30, 60];
            StrPerLevel = [3, 4];
            IntPerLevel = [2, 4];
            DexPerLevel = [9, 11];
            ResistancePerLevel = [1, 2];
            SetStr(15, 20);
            SetDex(29, 40);
            SetInt(14, 19);

            SetHits(35, 60);

            SetDamage(4, 6);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 10);
            SetResistance(ResistanceType.Fire, 1, 10);
            SetResistance(ResistanceType.Cold, 1, 10);
            SetResistance(ResistanceType.Poison, 1, 10);
            SetResistance(ResistanceType.Energy, 1, 10);

            SetSkill(SkillName.EvalInt, 50.1, 60.0);
            SetSkill(SkillName.Magery, 50.1, 60.0);
            SetSkill(SkillName.MagicResist, 45.1, 56.0);
            SetSkill(SkillName.Tactics, 30.1, 55.0);
            SetSkill(SkillName.Wrestling, 40.1, 55.0);

            Fame = 7500;
            Karma = -7500;
        }

        public override string CorpseName => "a Chiikkaha the Toothed corpse";
        public override string DefaultName => "Chiikkaha the Toothed";
    }
}

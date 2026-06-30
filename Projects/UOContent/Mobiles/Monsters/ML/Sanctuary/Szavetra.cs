using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Szavetra : Succubus
    {
        [Constructible]
        public Szavetra()
        {

            LevelRange = [45, 80];
            StrPerLevel = [1, 5];
            IntPerLevel = [1, 7];
            DexPerLevel = [1, 2];
            ResistancePerLevel = [1, 2];

            SetStr(50, 80);
            SetDex(20, 35);
            SetInt(50, 70);
            SetHits(70, 80);
            SetDamage(1, 5);


            SetDamageType(ResistanceType.Physical, 75);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 20, 30);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.EvalInt, 50.3, 59.8);
            SetSkill(SkillName.Magery, 50.1, 60.6); // 10.1-10.6 on OSI, bug?
            SetSkill(SkillName.Meditation, 50.1, 60.0);
            SetSkill(SkillName.MagicResist, 50.2, 57.2);
            SetSkill(SkillName.Tactics, 51.2, 62.8);
            SetSkill(SkillName.Wrestling, 50.2, 56.4);

            Fame = 24000;
            Karma = -24000;
        }

        public override string CorpseName => "a Szavetra corpse";
        public override string DefaultName => "Szavetra";
    }
}

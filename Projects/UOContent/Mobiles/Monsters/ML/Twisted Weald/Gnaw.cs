using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Gnaw : DireWolf
    {
        [Constructible]
        public Gnaw()
        {
            IsParagon = true;

            Hue = 0x130;
            LevelRange = [35, 55];
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

            SetSkill(SkillName.Wrestling,  45.1, 50.0);
            SetSkill(SkillName.Tactics,  45.1, 50.0);
            SetSkill(SkillName.MagicResist,  45.1, 50.0);

            Fame = 17500;
            Karma = -17500;
        }

        /*
        // TODO: uncomment once added
        public override void OnDeath( Container c )
        {
          base.OnDeath( c );

          if (Utility.RandomDouble() < 0.3)
            c.DropItem( new GnawsFang() );
        }
        */

        public override string CorpseName => "a Gnaw corpse";
        public override string DefaultName => "Gnaw";
        public override bool GivesMLMinorArtifact => true;
        public override int Hides => 28;
        public override int Meat => 4;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
        }
    }
}

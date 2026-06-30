using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Irk : Changeling
    {
        [Constructible]
        public Irk()
        {
            IsParagon = true;

            Hue = DefaultHue;

            LevelRange = [45, 65];
            StrPerLevel = [1, 3];
            IntPerLevel = [1, 2];
            DexPerLevel = [2, 4];
            ResistancePerLevel = [1, 3];
            SetStr(35, 75);
            SetDex(40, 90);
            SetInt(30, 56);

            SetHits(70, 150);
            SetDamage(6, 9);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 20);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 15, 25);
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

        /*
        // TODO: uncomment once added
        public override void OnDeath( Container c )
        {
          base.OnDeath( c );

          if (Utility.RandomDouble() < 0.25)
            c.DropItem( new IrksBrain() );

          if (Utility.RandomDouble() < 0.025)
            c.DropItem( new PaladinGloves() );
        }
        */

        public override string CorpseName => "an Irk corpse";
        public override string DefaultName => "Irk";
        public override int DefaultHue => 0x489;

        // TODO: Angry fire

        public override bool GivesMLMinorArtifact => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
        }
    }
}

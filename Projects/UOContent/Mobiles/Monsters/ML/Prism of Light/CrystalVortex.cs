using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class CrystalVortex : BaseCreature
    {
        [Constructible]
        public CrystalVortex()
            : base(AIType.AI_Melee)
        {
            Body = 0xD;
            Hue = 0x2B2;
            BaseSoundID = 0x107;

            LevelRange = [68, 75];
            StrPerLevel = [4, 6];
            IntPerLevel = [3, 3];
            DexPerLevel = [3, 5];
            ResistancePerLevel = [1, 3];

            SetStr(76, 105);
            SetDex(50, 100);
            SetInt(52, 90);

            SetHits(91, 110);

            SetDamage(3, 8);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Fire, 10, 15);
            SetResistance(ResistanceType.Cold, 10, 25);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Cold, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetSkill(SkillName.MagicResist, 50.0, 60.5);
            SetSkill(SkillName.Tactics, 50.0, 60.5);
            SetSkill(SkillName.Wrestling, 50.0, 60.5);

            Fame = 17000;
            Karma = -17000;

            PackArcaneScroll(0, 2);
        }

        public override string CorpseName => "a crystal vortex corpse";
        public override string DefaultName => "a crystal vortex";

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            // TODO: uncomment once added
            // AddLoot( LootPack.Parrot );
        }

        /*
        // TODO: uncomment once added
        public override void OnDeath( Container c )
        {
          base.OnDeath( c );

          if (Utility.RandomDouble() < 0.75)
            c.DropItem( new CrystallineFragments() );

          if (Utility.RandomDouble() < 0.06)
            c.DropItem( new JaggedCrystals() );
        }
        */

        public override int GetAngerSound() => 0x15;

        public override int GetAttackSound() => 0x28;
    }
}

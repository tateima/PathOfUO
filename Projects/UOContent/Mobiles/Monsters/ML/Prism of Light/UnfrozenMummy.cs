using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class UnfrozenMummy : BaseCreature
    {
        [Constructible]
        public UnfrozenMummy()
            : base(AIType.AI_Mage)
        {
            Body = 0x9B;
            Hue = 0x480;
            BaseSoundID = 0x1D7;
            LevelRange = [20, 60];
            StrPerLevel = [2, 4];
            IntPerLevel = [1, 2];
            DexPerLevel = [2, 4];
            ResistancePerLevel = [1, 2];

            SetStr(76, 85);
            SetDex(50, 70);
            SetInt(52, 70);

            SetHits(91, 110);

            SetDamage(3, 8);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Fire, 10, 15);
            SetResistance(ResistanceType.Cold, 10, 25);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Energy, 50);
            SetDamageType(ResistanceType.Cold, 50);

            SetSkill(SkillName.Wrestling, 40.0, 60.5);
            SetSkill(SkillName.Tactics, 40.0, 60.5);
            SetSkill(SkillName.MagicResist, 40.0, 60.5);
            SetSkill(SkillName.Magery, 40.0, 60.5);
            SetSkill(SkillName.EvalInt, 40.0, 60.5);
            SetSkill(SkillName.Meditation, 40.0, 60.5);

            Fame = 25000;
            Karma = -25000;

            PackArcaneScroll(0, 2);
        }

        /*
        // TODO: uncomment once added
        public override void OnDeath( Container c )
        {
          base.OnDeath( c );

          if (Utility.RandomDouble() < 0.6)
            c.DropItem( new BrokenCrystals() );

          if (Utility.RandomDouble() < 0.1)
            c.DropItem( new ParrotItem() );
        }
        */

        public override string CorpseName => "an unfrozen mummy corpse";
        public override string DefaultName => "an unfrozen mummy";

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            // TODO: uncomment once added
            // AddLoot( LootPack.Parrot );
        }
    }
}

using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class CrystalDaemon : BaseCreature
    {
        [Constructible]
        public CrystalDaemon()
            : base(AIType.AI_Mage)
        {
            Body = 0x310;
            Hue = 0x3E8;
            BaseSoundID = 0x47D;

            LevelRange = [40, 60];
            StrPerLevel = [4, 6];
            IntPerLevel = [3, 3];
            DexPerLevel = [3, 5];
            ResistancePerLevel = [1, 3];

            SetStr(76, 95);
            SetDex(50, 100);
            SetInt(52, 90);

            SetHits(91, 110);

            SetDamage(3, 8);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Fire, 10, 15);
            SetResistance(ResistanceType.Cold, 10, 25);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.EvalInt, 50.0, 60.5);
            SetSkill(SkillName.Magery, 50.0, 60.5);
            SetSkill(SkillName.MagicResist, 50.0, 60.5);
            SetSkill(SkillName.Tactics, 50.0, 60.5);
            SetSkill(SkillName.Wrestling, 50.0, 60.5);
            SetSkill(SkillName.Meditation, 50.0, 60.0);

            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Cold, 40);
            SetDamageType(ResistanceType.Energy, 60);

            Fame = 15000;
            Karma = -15000;

            PackArcaneScroll(0, 1);
        }

        /*
        // TODO: uncomment once added
        public override void OnDeath( Container c )
        {
          base.OnDeath( c );

          if (Utility.RandomDouble() < 0.4)
            c.DropItem( new ScatteredCrystals() );
        }
        */

        public override string CorpseName => "a crystal daemon corpse";
        public override string DefaultName => "a crystal daemon";

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder };

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 3);
        }
    }
}

using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class CrystalSeaSerpent : SeaSerpent
    {
        [Constructible]
        public CrystalSeaSerpent()
        {
            Hue = 0x47E;

            LevelRange = [50, 68];
            StrPerLevel = [2, 5];
            IntPerLevel = [1, 3];
            DexPerLevel = [1, 4];
            ResistancePerLevel = [1, 3];

            SetStr(86, 105);
            SetDex(20, 80);
            SetInt(32, 50);

            SetHits(100, 230);

            SetDamage(2, 7);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);
        }

        /*
        // TODO: uncomment once added
        public override void OnDeath( Container c )
        {
          base.OnDeath( c );

          if (Utility.RandomDouble() < 0.05)
            c.DropItem( new CrushedCrystals() );

          if (Utility.RandomDouble() < 0.1)
            c.DropItem( new IcyHeart() );

          if (Utility.RandomDouble() < 0.1)
            c.DropItem( new LuckyDagger() );
        }
        */

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder };
        public override string CorpseName => "a crystal sea serpent corpse";
        public override string DefaultName => "a crystal sea serpent";
    }
}

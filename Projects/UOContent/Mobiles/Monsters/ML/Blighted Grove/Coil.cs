using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Coil : SilverSerpent
    {
        [Constructible]
        public Coil()
        {
            IsParagon = true;

            Hue = 0x3F;
            LevelRange = [65, 75];
            StrPerLevel = [4, 8];
            IntPerLevel = [1, 3];
            DexPerLevel = [3, 7];
            ResistancePerLevel = [2, 4];

            SetStr(126, 185);
            SetDex(120, 230);
            SetInt(52, 70);

            SetHits(210, 270);

            SetDamage(3, 10);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 5, 30);
            SetResistance(ResistanceType.Fire, 10, 25);
            SetResistance(ResistanceType.Cold, 10, 25);
            SetResistance(ResistanceType.Poison, 60);
            SetResistance(ResistanceType.Energy, 10, 25);

            SetSkill(SkillName.Wrestling, 50.0, 63.5);
            SetSkill(SkillName.Tactics, 50.0, 63.5);
            SetSkill(SkillName.MagicResist, 50.0, 63.5);
            SetSkill(SkillName.Anatomy, 50.0, 63.5);
            SetSkill(SkillName.Poisoning, 50.0, 63.5);

            Fame = 22000;
            Karma = -22000;
            PackGem(2);
            PackItem(new Bone());
        }

        public override string CorpseName => "a Coil corpse";
        // TODO: Check faction allegiance

        public override string DefaultName => "Coil";

        public override Poison HitPoison => Poison.Lethal;
        public override Poison PoisonImmune => Poison.Lethal;
        public override bool GivesMLMinorArtifact => true;
        public override int Hides => 48;
        public override int Meat => 1;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
        }

        public override WeaponAbility GetWeaponAbility() => WeaponAbility.MortalStrike;

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.DropItem(new CoilsFang());

            /*
            // TODO: uncomment once added
            if (Utility.RandomDouble() < 0.025)
            {
              switch ( Utility.Random( 5 ) )
              {
                case 0: c.DropItem( new AssassinChest() ); break;
                case 1: c.DropItem( new DeathGloves() ); break;
                case 2: c.DropItem( new LeafweaveLegs() ); break;
                case 3: c.DropItem( new HunterLegs() ); break;
                case 4: c.DropItem( new MyrmidonLegs() ); break;
              }
            }
            */
        }
    }
}

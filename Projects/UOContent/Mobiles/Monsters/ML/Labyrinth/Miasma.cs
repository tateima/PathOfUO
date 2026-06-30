using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Miasma : Scorpion
    {
        [Constructible]
        public Miasma()
        {
            IsParagon = true;

            Hue = 0x8FD;

            LevelRange = [50, 60];
            StrPerLevel = [3, 6];
            IntPerLevel = [1, 2];
            DexPerLevel = [3, 5];
            ResistancePerLevel = [2, 3];

            SetStr(60, 155);
            SetDex(90, 145);
            SetInt(55, 80);
            SetHits(85, 150);

            SetDamage(5, 8);

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Poison, 40);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Fire, 10, 25);
            SetResistance(ResistanceType.Cold, 10, 25);
            SetResistance(ResistanceType.Poison, 10, 30);
            SetResistance(ResistanceType.Energy, 10, 15);

            SetSkill(SkillName.Wrestling, 50.9, 65.1);
            SetSkill(SkillName.Tactics, 50.9, 65.1);
            SetSkill(SkillName.Anatomy, 50.9, 65.1);
            SetSkill(SkillName.MagicResist, 50.9, 65.1);
            SetSkill(SkillName.Poisoning, 50.9, 65.1);

            Fame = 21000;
            Karma = -21000;
        }

        /*
        // TODO: uncomment once added
        public override void OnDeath( Container c )
        {
          base.OnDeath( c );

          if (Utility.RandomDouble() < 0.025)
          {
            switch ( Utility.Random( 16 ) )
            {
              case 0: c.DropItem( new MyrmidonGloves() ); break;
              case 1: c.DropItem( new MyrmidonGorget() ); break;
              case 2: c.DropItem( new MyrmidonLegs() ); break;
              case 3: c.DropItem( new MyrmidonArms() ); break;
              case 4: c.DropItem( new PaladinArms() ); break;
              case 5: c.DropItem( new PaladinGorget() ); break;
              case 6: c.DropItem( new LeafweaveLegs() ); break;
              case 7: c.DropItem( new DeathChest() ); break;
              case 8: c.DropItem( new DeathGloves() ); break;
              case 9: c.DropItem( new DeathLegs() ); break;
              case 10: c.DropItem( new GreymistGloves() ); break;
              case 11: c.DropItem( new GreymistArms() ); break;
              case 12: c.DropItem( new AssassinChest() ); break;
              case 13: c.DropItem( new AssassinArms() ); break;
              case 14: c.DropItem( new HunterGloves() ); break;
              case 15: c.DropItem( new HunterLegs() ); break;
            }
          }
        }
        */

        public override string CorpseName => "a Miasma corpse";
        public override string DefaultName => "Miasma";

        /* yes, this is OSI style */
        public override double WeaponAbilityChance => 0.75;
        public override double HitPoisonChance => 0.35;
        public override Poison HitPoison => Poison.Lethal;
        public override bool HasManaOverride => true;
        public override bool GivesMLMinorArtifact => true;
        public override int TreasureMapLevel => 5;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 4);
        }

        public override WeaponAbility GetWeaponAbility() => WeaponAbility.MortalStrike;
    }
}

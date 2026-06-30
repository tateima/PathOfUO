using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class LadyLissith : GiantBlackWidow
    {
        [Constructible]
        public LadyLissith()
        {
            IsParagon = true;
            Hue = 0x452;

            LevelRange = [45, 65];
            StrPerLevel = [2, 7];
            IntPerLevel = [2, 3];
            DexPerLevel = [2, 9];
            ResistancePerLevel = [1, 2];
            SetStr(60, 75);
            SetDex(25, 65);
            SetInt(5, 11);

            SetHits(35, 70);

            SetDamage(2, 9);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 20);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.Wrestling,  45.1, 50.0);
            SetSkill(SkillName.Tactics,  45.1, 50.0);
            SetSkill(SkillName.MagicResist,  45.1, 50.0);
            SetSkill(SkillName.Anatomy,  45.1, 50.0);
            SetSkill(SkillName.Poisoning,  45.1, 50.0);

            Fame = 18900;
            Karma = -18900;
        }

        public override string CorpseName => "a Lady Lissith corpse";
        public override string DefaultName => "Lady Lissith";

        /*
        // TODO: uncomment once added
        public override void OnDeath( Container c )
        {
          base.OnDeath( c );

          if (Utility.RandomDouble() < 0.025)
            c.DropItem( new GreymistChest() );

          if (Utility.RandomDouble() < 0.45)
            c.DropItem( new LissithsSilk() );

          if (Utility.RandomDouble() < 0.1)
            c.DropItem( new ParrotItem() );
        }
        */

        public override bool GivesMLMinorArtifact => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
        }

        public override WeaponAbility GetWeaponAbility() => WeaponAbility.BleedAttack;
    }
}

using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Putrefier : Balron
    {
        [Constructible]
        public Putrefier()
        {
            IsParagon = true;

            Hue = 63;

            LevelRange = [85, 95];
            StrPerLevel = [4, 8];
            IntPerLevel = [4, 8];
            DexPerLevel = [4, 8];
            SetStr(126, 175);
            SetDex(70, 100);
            SetInt(82, 140);

            SetHits(300, 500);

            SetDamage(8, 24);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 10, 30);
            SetResistance(ResistanceType.Fire, 10, 30);
            SetResistance(ResistanceType.Cold, 5, 20);
            SetResistance(ResistanceType.Poison, 30);
            SetResistance(ResistanceType.Energy, 10, 30);

            SetSkill(SkillName.Anatomy, 53.0, 63.5);
            SetSkill(SkillName.EvalInt, 53.0, 63.5);
            SetSkill(SkillName.Magery, 53.0, 63.5);
            SetSkill(SkillName.Meditation, 53.0, 63.5);
            SetSkill(SkillName.MagicResist, 53.0, 63.5);
            SetSkill(SkillName.Tactics, 53.0, 63.5);
            SetSkill(SkillName.EvalInt, 53.0, 63.5);
            SetSkill(SkillName.Poisoning, 45.0, 50.0);

            Fame = 24000;
            Karma = -24000;

            PackScroll(4, 7);
            PackScroll(4, 7);
        }

        public override string CorpseName => "a Putrefier corpse";
        public override string DefaultName => "Putrefier";

        /*
        // TODO: uncomment once added
        public override void OnDeath( Container c )
        {
          base.OnDeath( c );

          c.DropItem( new SpleenOfThePutrefier() );

          if (Utility.RandomDouble() < 0.6)
            c.DropItem( new ParrotItem() );
        }
        */

        public override bool GivesMLMinorArtifact => true;
        public override Poison HitPoison => Poison.Deadly; // Becomes Lethal with Paragon bonus
        public override int TreasureMapLevel => 5;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
        }
    }
}

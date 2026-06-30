using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class SirPatrick : SkeletalKnight
    {
        [Constructible]
        public SirPatrick()
        {
            IsParagon = true;

            Hue = 0x47E;

            LevelRange = [50, 60];
            StrPerLevel = [3, 6];
            IntPerLevel = [1, 2];
            DexPerLevel = [3, 5];
            ResistancePerLevel = [2, 3];

            SetStr(80, 175);
            SetDex(50, 75);
            SetInt(55, 80);
            SetHits(95, 180);

            SetDamage(6, 10);

            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Cold, 60);

            SetResistance(ResistanceType.Physical, 5, 30);
            SetResistance(ResistanceType.Fire, 10, 25);
            SetResistance(ResistanceType.Cold, 10, 30);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 10, 25);

            SetSkill(SkillName.Wrestling, 50.9, 65.1);
            SetSkill(SkillName.Tactics, 50.4, 65.9);
            SetSkill(SkillName.MagicResist, 50.1, 65.5);
            SetSkill(SkillName.Anatomy, 50.0, 65.5);

            Fame = 18000;
            Karma = -18000;
        }

        public override string CorpseName => "a Sir Patrick corpse";
        public override string DefaultName => "Sir Patrick";

        /*
        // TODO: uncomment once added
        public override void OnDeath( Container c )
        {
          base.OnDeath( c );

          if (Utility.RandomDouble() < 0.15)
            c.DropItem( new DisintegratingThesisNotes() );

          if (Utility.RandomDouble() < 0.05)
            c.DropItem( new AssassinChest() );
        }
        */

        public override bool GivesMLMinorArtifact => true;

        private static MonsterAbility[] _abilities = { new SirPatrickDrainLife() };
        public override MonsterAbility[] GetMonsterAbilities() => _abilities;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
        }

        private class SirPatrickDrainLife : DrainLifeAreaAttack
        {
            public override int MinDamage => 14;
            public override int MaxDamage => 30;

            protected override void DoEffectTarget(BaseCreature source, Mobile defender)
            {
                defender.FixedParticles(0x374A, 10, 15, 5013, 0x455, 0, EffectLayer.Waist);
                defender.PlaySound(0x1EA);

                DrainLife(source, defender);
            }
        }
    }
}

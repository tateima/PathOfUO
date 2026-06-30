using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class CrystalLatticeSeeker : BaseCreature
    {
        [Constructible]
        public CrystalLatticeSeeker()
            : base(AIType.AI_Mage)
        {
            Body = 0x7B;
            Hue = 0x47E;
            LevelRange = [55, 75];
            StrPerLevel = [3, 6];
            IntPerLevel = [2, 3];
            DexPerLevel = [2, 5];
            ResistancePerLevel = [1, 3];

            SetStr(106, 145);
            SetDex(20, 70);
            SetInt(52, 70);

            SetHits(150, 330);

            SetDamage(2, 9);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Fire, 10, 15);
            SetResistance(ResistanceType.Cold, 10, 25);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.Anatomy, 50.0, 60.5);
            SetSkill(SkillName.EvalInt, 50.0, 60.5);
            SetSkill(SkillName.Magery, 50.0, 60.5);
            SetSkill(SkillName.Meditation, 50.0, 60.5);
            SetSkill(SkillName.MagicResist, 50.0, 60.5);
            SetSkill(SkillName.Tactics, 50.0, 60.5);
            SetSkill(SkillName.Wrestling, 50.0, 60.5);

            Fame = 17000;
            Karma = -17000;

            PackArcaneScroll(0, 2);
        }

        public override string CorpseName => "a Crystal Lattice Seeker corpse";
        public override string DefaultName => "Crystal Lattice Seeker";

        /*
        // TODO: uncomment once added
        public override void OnDeath( Container c )
        {
          base.OnDeath( c );

          if (Utility.RandomDouble() < 0.75)
            c.DropItem( new CrystallineFragments() );

          if (Utility.RandomDouble() < 0.07)
            c.DropItem( new PiecesOfCrystal() );
        }
        */

        public override int Feathers => 100;
        public override int TreasureMapLevel => 5;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 4);
            // TODO: uncomment once added
            // AddLoot( LootPack.Parrot );
            AddLoot(LootPack.Gems);
        }

        public override void OnGaveMeleeAttack(Mobile defender, int damage)
        {
            base.OnGaveMeleeAttack(defender, damage);

            if (Utility.RandomDouble() < 0.1)
            {
                Drain(defender);
            }
        }

        public override void OnGotMeleeAttack(Mobile attacker, int damage)
        {
            base.OnGotMeleeAttack(attacker, damage);

            if (Utility.RandomDouble() < 0.1)
            {
                Drain(attacker);
            }
        }

        public virtual void Drain(Mobile m)
        {
            int toDrain;

            switch (Utility.Random(3))
            {
                case 0:
                    {
                        Say(1042156); // I can grant life, and I can sap it as easily.
                        PlaySound(0x1E6);

                        toDrain = Utility.RandomMinMax(3, 6);
                        Hits += toDrain;
                        m.Hits -= toDrain;
                        break;
                    }
                case 1:
                    {
                        Say(1042157); // You'll go nowhere, unless I deem it should be so.
                        PlaySound(0x1DF);

                        toDrain = Utility.RandomMinMax(10, 25);
                        Stam += toDrain;
                        m.Stam -= toDrain;
                        break;
                    }
                case 2:
                    {
                        Say(1042155); // Your power is mine to use as I will.
                        PlaySound(0x1F8);

                        toDrain = Utility.RandomMinMax(15, 25);
                        Mana += toDrain;
                        m.Mana -= toDrain;
                        break;
                    }
            }
        }

        public override int GetAttackSound() => 0x2F6;

        public override int GetDeathSound() => 0x2F7;

        public override int GetAngerSound() => 0x2F8;

        public override int GetHurtSound() => 0x2F9;

        public override int GetIdleSound() => 0x2FA;
    }
}

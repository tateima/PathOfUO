using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class FetidEssence : BaseCreature
    {
        [Constructible]
        public FetidEssence() : base(AIType.AI_Mage)
        {
            Body = 273;

            LevelRange = [25, 35];
            StrPerLevel = [4, 5];
            IntPerLevel = [2, 3];
            DexPerLevel = [2, 5];
            ResistancePerLevel = [2, 3];
            SetStr(25, 40);
            SetDex(29, 40);
            SetInt(30, 35);

            SetHits(50, 86);

            SetDamage(5, 7);

            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Poison, 70);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Fire, 10, 15);
            SetResistance(ResistanceType.Cold, 10, 15);
            SetResistance(ResistanceType.Poison, 10, 30);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.Meditation, 40.4, 50.1);
            SetSkill(SkillName.EvalInt, 40.4, 50.1);
            SetSkill(SkillName.Magery, 40.4, 50.1);
            SetSkill(SkillName.Poisoning, 60.4, 65.1);
            SetSkill(SkillName.Anatomy, 40.4, 50.1);
            SetSkill(SkillName.MagicResist, 40.4, 50.1);
            SetSkill(SkillName.Tactics, 40.4, 50.1);
            SetSkill(SkillName.Wrestling, 40.4, 50.1);

            Fame = 3700;   // Guessed
            Karma = -3700; // Guessed
        }

        public override string CorpseName => "a fetid essence corpse";
        public override string DefaultName => "a fetid essence";

        public override Poison HitPoison => Poison.Deadly;
        public override Poison PoisonImmune => Poison.Deadly;

        public override void GenerateLoot() // Need to verify
        {
            AddLoot(LootPack.FilthyRich);
        }

        public override int GetAngerSound() => 0x56d;

        public override int GetIdleSound() => 0x56b;

        public override int GetAttackSound() => 0x56c;

        public override int GetHurtSound() => 0x56c;

        public override int GetDeathSound() => 0x56e;

        /*private class InternalTimer : Timer
        {
          private Mobile m_From;
          private Mobile m_Mobile;
          private int m_Count;

          public InternalTimer( Mobile from, Mobile m ) : base( TimeSpan.FromSeconds( 1.0 ), TimeSpan.FromSeconds( 1.0 ) )
          {
            m_From = from;
            m_Mobile = m;
          }

        }*/
    }
}

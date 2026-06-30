using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class CorruptedSoul : BaseCreature
    {
        [Constructible]
        public CorruptedSoul() : base(AIType.AI_Melee)
        {
            Body = 0x3CA;
            Hue = 0x453;

            LevelRange = [55, 60];
            StrPerLevel = [4, 8];
            IntPerLevel = [2, 3];
            DexPerLevel = [3, 5];
            ResistancePerLevel = [1, 2];

            SetStr(45, 70);
            SetDex(35, 55);
            SetInt(55, 70);

            SetSpeed(0.25, 2.5);
            SetDamage(5, 6);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Fire, 5, 15);
            SetResistance(ResistanceType.Cold, 10, 30);
            SetResistance(ResistanceType.Poison, 0);
            SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.MagicResist, 45.4, 50.1);
            SetSkill(SkillName.Tactics, 45.4, 50.1);
            SetSkill(SkillName.Wrestling, 45.4, 50.1);

            Fame = 5000;
            Karma = -5000;

            // VirtualArmor = 6; Not sure
        }

        public override bool DeleteCorpseOnDeath => true;

        public override string DefaultName => "a corrupted soul";

        public override bool AlwaysAttackable => true;
        public override bool BleedImmune => true; // NEED TO VERIFY

        /*public override int GetDeathSound()
        {
          return 0x0;
        }*/

        public override bool AlwaysMurderer => true;

        // NEED TO VERIFY SOUNDS! Known: No Idle Sound.

        /*public override int GetAngerSound()
        {
          return 0x0;
        }*/

        public override int GetAttackSound() => 0x233;

        // TODO: Proper OnDeath Effect

        public override bool OnBeforeDeath()
        {
            if (!base.OnBeforeDeath())
            {
                return false;
            }

            // 1 in 20 chance that a Thread of Fate will appear in the killer's pack

            Effects.SendLocationEffect(Location, Map, 0x376A, 10, 1);
            return true;
        }
    }
}

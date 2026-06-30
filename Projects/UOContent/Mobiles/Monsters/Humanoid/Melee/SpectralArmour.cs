using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class SpectralArmour : BaseCreature
    {
        [Constructible]
        public SpectralArmour() : base(AIType.AI_Melee)
        {
            Body = 637;
            Hue = 0x8026;

            AddItem(new Buckler { Movable = false, Hue = 0x835 });
            AddItem(new ChainCoif { Hue = 0x835 });
            AddItem(new PlateGloves { Hue = 0x835 });

            LevelRange = [7, 12];
            StrPerLevel = [2, 4];
            IntPerLevel = [1, 2];
            DexPerLevel = [3, 4];
            ResistancePerLevel = [2, 3];

            SetStr(40, 70);
            SetDex(30, 55);
            SetInt(15, 40);
            SetHits(65, 90);
            SetStam(95, 115);

            SetDamage(6, 7);

            SetDamageType(ResistanceType.Physical, 75);
            SetDamageType(ResistanceType.Cold, 25);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.Wrestling, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.MagicResist, 40.1, 50.0);

            VirtualArmor = 40;
            Fame = 7000;
            Karma = -7000;
        }

        public override bool DeleteCorpseOnDeath => true;

        public override string DefaultName => "a spectral armour";

        public override Poison PoisonImmune => Poison.Regular;

        public override int GetIdleSound() => 0x200;

        public override int GetAngerSound() => 0x56;

        public override bool OnBeforeDeath()
        {
            if (!base.OnBeforeDeath())
            {
                return false;
            }

            var gold = new Gold(Utility.RandomMinMax(240, 375));
            gold.MoveToWorld(Location, Map);

            Effects.SendLocationEffect(Location, Map, 0x376A, 10, 1);
            return true;
        }
    }
}

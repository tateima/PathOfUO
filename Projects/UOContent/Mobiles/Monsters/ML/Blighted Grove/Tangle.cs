using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Tangle : BogThing
    {
        [Constructible]
        public Tangle()
        {
            // TODO: Not a paragon? No ML arties?
            // It moves like a paragon on OSI...

            Hue = 0x21;

            LevelRange = [55, 65];
            StrPerLevel = [2, 5];
            IntPerLevel = [1, 3];
            DexPerLevel = [1, 3];
            ResistancePerLevel = [1, 2];

            SetStr(116, 165);
            SetDex(50, 70);
            SetInt(42, 75);

            SetHits(120, 180);

            SetDamage(2, 9);

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Poison, 40);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Fire, 10, 15);
            SetResistance(ResistanceType.Cold, 10, 30);
            SetResistance(ResistanceType.Poison, 10, 30);
            SetResistance(ResistanceType.Energy, 10, 15);

            SetSkill(SkillName.Wrestling, 40.4, 50.1);
            SetSkill(SkillName.Tactics, 40.4, 50.1);
            SetSkill(SkillName.MagicResist, 40.4, 50.1);

            // TODO: Fame/Karma?
        }

        public override string CorpseName => "a Tangle corpse";
        public override string DefaultName => "Tangle";

        public override Poison PoisonImmune => Poison.Lethal;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.3)
            {
                c.DropItem(new TaintedSeeds());
            }
        }
    }
}

using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Saliva : Harpy
    {
        [Constructible]
        public Saliva()
        {
            // TODO: Not a paragon? No ML arties?
            // It moves like a paragon on OSI...

            Hue = 0x11E;

            LevelRange = [55, 65];
            StrPerLevel = [2, 5];
            IntPerLevel = [1, 3];
            DexPerLevel = [1, 3];
            ResistancePerLevel = [1, 2];

            SetStr(96, 145);
            SetDex(70, 100);
            SetInt(42, 75);

            SetHits(120, 180);

            SetDamage(2, 9);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Fire, 10, 15);
            SetResistance(ResistanceType.Cold, 10, 30);
            SetResistance(ResistanceType.Poison, 10, 30);
            SetResistance(ResistanceType.Energy, 10, 15);

            SetSkill(SkillName.Wrestling, 40.4, 50.1);
            SetSkill(SkillName.Tactics, 40.4, 50.1);
            SetSkill(SkillName.MagicResist, 40.4, 50.1);

            Fame = 20000;
            Karma = -20000;
        }

        public override string CorpseName => "a Saliva corpse";
        public override string DefaultName => "Saliva";

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            c.DropItem(new SalivasFeather());

            // TODO: uncomment once added
            // if (Utility.RandomDouble() < 0.1)
            // c.DropItem( new ParrotItem() );
        }
    }
}

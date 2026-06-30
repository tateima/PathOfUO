using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Protector : BaseCreature
    {
        [Constructible]
        public Protector()
            : base(AIType.AI_Melee)
        {
            Body = 401;
            Female = true;
            Hue = Race.Human.RandomSkinHue();
            HairItemID = Race.Human.RandomHair(this);
            HairHue = Race.Human.RandomHairHue();

            Title = "the mystic llamaherder";

            LevelRange = [57, 77];
            StrPerLevel = [3, 6];
            IntPerLevel = [2, 3];
            DexPerLevel = [2, 5];
            ResistancePerLevel = [1, 3];

            SetStr(96, 125);
            SetDex(30, 80);
            SetInt(32, 80);

            SetHits(100, 350);

            SetDamage(3, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 20);
            SetResistance(ResistanceType.Fire, 5, 20);
            SetResistance(ResistanceType.Cold, 15, 20);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 10, 15);

            SetSkill(SkillName.Wrestling, 40.0, 60.5);
            SetSkill(SkillName.MagicResist, 40.0, 60.5);
            SetSkill(SkillName.Anatomy, 40.0, 60.5);

            Fame = 10000;
            Karma = -10000;

            AddItem(new ThighBoots
            {
                Movable = false,
                Hue = Utility.Random(2)
            });
            AddItem(new Item(0x204E)
            {
                Layer = Layer.OuterTorso,
                Movable = false,
                Hue = Utility.Random(2)
            });
        }

        public override string CorpseName => "a human corpse";
        public override string DefaultName => "a Protector";

        public override bool AlwaysMurderer => true;
        public override bool PropertyTitle => false;
        public override bool ShowFameTitle => false;

        public override void GenerateLoot(bool spawning)
        {
            if (spawning)
            {
                return; // No loot/backpack on spawn
            }

            base.GenerateLoot(true);
            base.GenerateLoot(false);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
        }

        /*
        // TODO: uncomment once added
        public override void OnDeath( Container c )
        {
          base.OnDeath( c );

          if (Utility.RandomDouble() < 0.4)
            c.DropItem( new ProtectorsEssence() );
        }
        */
    }
}

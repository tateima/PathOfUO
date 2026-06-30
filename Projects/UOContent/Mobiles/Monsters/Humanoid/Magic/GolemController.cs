using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class GolemController : BaseCreature
    {
        [Constructible]
        public GolemController() : base(AIType.AI_Mage)
        {
            Name = NameList.RandomName("golem controller");
            Title = "the controller";

            Body = 400;
            Hue = 0x455;

            AddArcane(new Robe());
            AddArcane(new ThighBoots());
            AddArcane(new LeatherGloves());
            AddArcane(new Cloak());

            LevelRange = [10, 60];
            StrPerLevel = [1, 3];
            IntPerLevel = [4, 9];
            DexPerLevel = [2, 4];
            ResistancePerLevel = [1, 2];

            SetStr(25, 40);
            SetDex(15, 25);
            SetInt(55, 80);

            SetDamage(2, 6);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 25);
            SetResistance(ResistanceType.Fire, 15, 15);
            SetResistance(ResistanceType.Cold, 5, 25);
            SetResistance(ResistanceType.Poison, 1, 5);
            SetResistance(ResistanceType.Energy, 1, 5);

            SetSkill(SkillName.EvalInt, 40.0, 50.5);
            SetSkill(SkillName.Magery, 40.0, 50.5);
            SetSkill(SkillName.Meditation, 40.0, 50.5);
            SetSkill(SkillName.MagicResist, 40.0, 50.5);
            SetSkill(SkillName.Tactics, 40.0, 50.5);
            SetSkill(SkillName.Wrestling, 40.0, 50.5);

            Fame = 4000;
            Karma = -4000;

            VirtualArmor = 16;

            if (Utility.RandomDouble() < 0.7)
            {
                PackItem(new ArcaneGem());
            }
        }

        public override string CorpseName => "a golem controller corpse";

        public override bool ClickTitle => false;
        public override bool ShowFameTitle => false;
        public override bool AlwaysMurderer => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
        }

        public void AddArcane(Item item)
        {
            if (item is IArcaneEquip eq)
            {
                eq.CurArcaneCharges = eq.MaxArcaneCharges = 20;
            }

            item.Hue = ArcaneGem.DefaultArcaneHue;
            item.LootType = LootType.Newbied;

            AddItem(item);
        }
    }
}

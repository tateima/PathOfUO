using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Executioner : BaseCreature
    {
        [Constructible]
        public Executioner() : base(AIType.AI_Melee)
        {
            SpeechHue = Utility.RandomDyedHue();
            Title = "the executioner";
            Hue = Race.Human.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                AddItem(new Skirt(Utility.RandomRedHue()));
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                AddItem(new ShortPants(Utility.RandomRedHue()));
            }
            LevelRange = [24, 29];
            StrPerLevel = [1, 6];
            IntPerLevel = [1, 2];
            DexPerLevel = [3, 5];
            ResistancePerLevel = [1, 3];

            SetStr(100, 135);
            SetDex(40, 65);
            SetInt(15, 40);
            SetHits(85, 120);
            SetDamage(6, 9);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Fire, 5, 15);
            SetResistance(ResistanceType.Cold, 5, 20);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.Anatomy, 40.1, 50.0);
            SetSkill(SkillName.Fencing, 40.1, 50.0);
            SetSkill(SkillName.Macing, 40.1, 50.0);
            SetSkill(SkillName.Poisoning, 40.1, 50.0);
            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Swords, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Lumberjacking, 40.1, 50.0);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 40;

            AddItem(new ThighBoots(Utility.RandomRedHue()));
            AddItem(new Surcoat(Utility.RandomRedHue()));
            AddItem(new ExecutionersAxe());

            Utility.AssignRandomHair(this);
        }

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };
        public override bool AlwaysMurderer => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Meager);
        }
    }
}

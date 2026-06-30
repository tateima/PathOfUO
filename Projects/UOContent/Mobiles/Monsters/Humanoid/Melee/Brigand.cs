using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Brigand : BaseCreature
    {
        public bool IsArcher { get; set; }
        [Constructible]
        public Brigand() : base(AIType.AI_Melee)
        {
            SpeechHue = Utility.RandomDyedHue();
            IsArcher = Utility.RandomBool();
            Title = "the brigand";
            Hue = Race.Human.RandomSkinHue();

            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                AddItem(new Skirt(Utility.RandomNeutralHue()));
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                AddItem(new ShortPants(Utility.RandomNeutralHue()));
            }

            LevelRange = [1, 40];
            StrPerLevel = [2, 4];
            IntPerLevel = [2, 3];
            DexPerLevel = [2, 3];
            ResistancePerLevel = [1, 2];

            SetStr(25, 40);
            SetDex(15, 25);
            SetInt(25, 40);
            if (IsArcher)
            {
                SetSkill(SkillName.Archery, 40.0, 50.5);
                AddItem(
                    Utility.Random(2) switch
                    {
                        0 => new Crossbow(),
                        1 => new HeavyCrossbow(),
                        _ => new RepeatingCrossbow()
                    }
                );
                PackItem(new Bolt(Utility.RandomMinMax(50, 70)));
            }
            else
            {
                SetSkill(SkillName.Swords, 40.0, 50.5);
                AddItem(
                    Utility.Random(7) switch
                    {
                        0 => new Longsword(),
                        1 => new Cutlass(),
                        2 => new Broadsword(),
                        3 => new Axe(),
                        4 => new Club(),
                        5 => new Dagger(),
                        _ => new Spear() // 6
                    }
                );
            }
            SetDamage(5, 6);

            SetSkill(SkillName.Fencing, 40.0, 50.5);
            SetSkill(SkillName.Macing, 40.0, 50.5);
            SetSkill(SkillName.MagicResist, 25.0, 47.5);
            SetSkill(SkillName.Tactics, 40.0, 50.5);
            SetSkill(SkillName.Wrestling, 15.0, 37.5);

            Fame = 1000;
            Karma = -1000;

            AddItem(new Boots(Utility.RandomNeutralHue()));
            AddItem(new FancyShirt());
            AddItem(new Bandana());

            Utility.AssignRandomHair(this);
        }
        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };
        public override bool ClickTitle => false;

        public override bool AlwaysMurderer => true;

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.9)
            {
                c.DropItem(new SeveredHumanEars());
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
        }
    }
}

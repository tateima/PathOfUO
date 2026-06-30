using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Centaur : BaseCreature
    {
        [Constructible]
        public Centaur() : base(AIType.AI_Melee, FightMode.Aggressor)
        {
            Name = NameList.RandomName("centaur");
            Body = 101;
            BaseSoundID = 679;

            LevelRange = [25, 40];
            StrPerLevel = [2, 4];
            IntPerLevel = [1, 2];
            DexPerLevel = [4, 9];
            ResistancePerLevel = [1, 2];

            SetStr(70, 125);
            SetDex(60, 95);
            SetInt(25, 70);
            SetHits(90, 130);
            SetDamage(3, 7);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 10, 30);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.Anatomy, 40.0, 50.0);
            SetSkill(SkillName.Archery, 40.0, 50.0);
            SetSkill(SkillName.MagicResist, 40.0, 50.0);
            SetSkill(SkillName.Tactics, 40.0, 50.0);
            SetSkill(SkillName.Wrestling, 40.0, 50.0);

            Fame = 6500;
            Karma = 0;

            VirtualArmor = 50;
            AddItem(new Bow());
            PackItem(
                new Arrow(
                    Utility.RandomMinMax(
                        80,
                        90
                    )
                )
            ); // OSI it is different: in a sub backpack, this is probably just a limitation of their engine
        }

        public override string CorpseName => "a centaur corpse";

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };

        public override int Meat => 1;
        public override int Hides => 8;
        public override HideType HideType => HideType.Spined;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Gems);
        }
    }
}

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

            SetStr(202, 300);
            SetDex(104, 260);
            SetInt(91, 100);

            SetHits(130, 172);

            SetDamage(13, 24);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 35, 45);
            SetResistance(ResistanceType.Cold, 25, 35);
            SetResistance(ResistanceType.Poison, 45, 55);
            SetResistance(ResistanceType.Energy, 35, 45);

            SetSkill(SkillName.Anatomy, 95.1, 115.0);
            SetSkill(SkillName.Archery, 95.1, 100.0);
            SetSkill(SkillName.MagicResist, 50.3, 80.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 95.1, 100.0);

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

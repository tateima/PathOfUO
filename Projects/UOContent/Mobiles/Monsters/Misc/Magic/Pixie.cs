using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Pixie : BaseCreature
    {
        [Constructible]
        public Pixie() : base(AIType.AI_Mage, FightMode.Aggressor)
        {
            Name = NameList.RandomName("pixie");
            Body = 128;
            BaseSoundID = 0x467;

            SetStr(21, 30);
            SetDex(301, 400);
            SetInt(201, 250);

            SetHits(13, 18);

            SetDamage(9, 15);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 90.1, 100.0);
            SetSkill(SkillName.Meditation, 90.1, 100.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 10.1, 20.0);
            SetSkill(SkillName.Wrestling, 10.1, 12.5);

            Fame = 7000;
            Karma = 7000;

            VirtualArmor = 100;
            if (Utility.RandomDouble() < 0.02)
            {
                PackStatue();
            }
        }

        public override string CorpseName => "a pixie corpse";
        public override bool InitialInnocent => false;
        public override bool AlwaysAttackable => true;
        public override HideType HideType => HideType.Spined;
        public override int Hides => 5;
        public override int Meat => 1;

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };

        public override void GenerateLoot()
        {
            AddLoot(LootPack.LowScrolls);
            AddLoot(LootPack.Gems, 2);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.35)
            {
                c.DropItem(new PixieLeg());
            }
        }
    }
}

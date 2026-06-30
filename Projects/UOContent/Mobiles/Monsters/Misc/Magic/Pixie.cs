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

            LevelRange = [5, 65];
            StrPerLevel = [1, 2];
            IntPerLevel = [2, 9];
            DexPerLevel = [2, 5];
            ResistancePerLevel = [1, 3];

            SetStr(36, 55);
            SetDex(100, 180);
            SetInt(102, 180);

            SetHits(30, 96);

            SetDamage(2, 5);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 6, 30);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.EvalInt, 40.1, 50.0);
            SetSkill(SkillName.Magery, 55.1, 65.0);
            SetSkill(SkillName.Meditation, 40.1, 50.0);
            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 20.1, 30.0);
            SetSkill(SkillName.Wrestling, 10.1, 20.0);

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

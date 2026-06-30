using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Efreet : BaseCreature
    {
        [Constructible]
        public Efreet() : base(AIType.AI_Mage)
        {
            Body = 131;
            BaseSoundID = 768;

            LevelRange = [25, 42];
            StrPerLevel = [2, 6];
            IntPerLevel = [1, 6];
            DexPerLevel = [2, 6];
            ResistancePerLevel = [1, 3];

            SetStr(60, 125);
            SetDex(30, 65);
            SetInt(25, 70);
            SetHits(85, 140);
            SetDamage(3, 8);

            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Fire, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 10, 30);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.EvalInt, 45.1, 55.0);
            SetSkill(SkillName.Magery, 45.1, 55.0);
            SetSkill(SkillName.MagicResist, 45.1, 55.0);
            SetSkill(SkillName.Tactics, 45.1, 55.0);
            SetSkill(SkillName.Wrestling, 45.1, 55.0);

            Fame = 10000;
            Karma = -10000;

            VirtualArmor = 56;
        }

        public override string CorpseName => "an efreet corpse";
        public override string DefaultName => "an efreet";

        public override int TreasureMapLevel => Core.AOS ? 4 : 5;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Gems);

            if (Utility.RandomDouble() < 0.02)
            {
                switch (Utility.Random(5))
                {
                    case 0:
                        PackItem(new DaemonArms());
                        break;
                    case 1:
                        PackItem(new DaemonChest());
                        break;
                    case 2:
                        PackItem(new DaemonGloves());
                        break;
                    case 3:
                        PackItem(new DaemonLegs());
                        break;
                    case 4:
                        PackItem(new DaemonHelm());
                        break;
                }
            }
        }
    }
}

using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Bogle : BaseCreature
    {
        [Constructible]
        public Bogle() : base(AIType.AI_Mage)
        {
            Body = 153;
            BaseSoundID = 0x482;
            LevelRange = [9, 26];
            StrPerLevel = [1, 4];
            IntPerLevel = [1, 4];
            DexPerLevel = [1, 4];
            ResistancePerLevel = [2, 3];
            SetStr(50, 80);
            SetDex(30, 75);
            SetInt(55, 100);
            SetHits(75, 90);

            SetDamage(2, 7);

            SetResistance(ResistanceType.Physical, 15, 20);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 6, 10);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.EvalInt, 45.1, 50.0);
            SetSkill(SkillName.Magery, 45.1, 50.0);
            SetSkill(SkillName.MagicResist, 45.1, 50.0);
            SetSkill(SkillName.Tactics, 45.1, 50.0);
            SetSkill(SkillName.Wrestling, 45.1, 50.0);

            Fame = 4000;
            Karma = -4000;

            VirtualArmor = 28;
            PackItem(Loot.RandomWeapon());
            PackItem(new Bone());
        }

        public override string CorpseName => "a ghostly corpse";
        public override string DefaultName => "a bogle";

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager);
        }
    }
}

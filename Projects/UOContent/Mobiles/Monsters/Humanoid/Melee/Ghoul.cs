using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Ghoul : BaseCreature
    {
        [Constructible]
        public Ghoul() : base(AIType.AI_Melee)
        {
            Body = 153;
            BaseSoundID = 0x482;

            LevelRange = [5, 9];
            StrPerLevel = [1, 4];
            IntPerLevel = [1, 2];
            DexPerLevel = [1, 2];
            ResistancePerLevel = [1, 2];

            SetStr(40, 60);
            SetDex(20, 55);
            SetInt(15, 20);
            SetHits(35, 90);

            SetDamage(4, 8);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 20);
            SetResistance(ResistanceType.Cold, 10, 15);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.MagicResist, 45.1, 50.0);
            SetSkill(SkillName.Tactics, 45.1, 50.0);
            SetSkill(SkillName.Wrestling, 45.1, 50.0);

            Fame = 2500;
            Karma = -2500;

            VirtualArmor = 28;

            PackItem(Loot.RandomWeapon());
        }

        public override string CorpseName => "a ghostly corpse";
        public override string DefaultName => "a ghoul";

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Regular;

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager);
        }
    }
}

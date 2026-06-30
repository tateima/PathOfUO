using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Troll : BaseCreature
    {
        [Constructible]
        public Troll() : base(AIType.AI_Melee)
        {
            Body = Utility.RandomList(53, 54);
            BaseSoundID = 461;

            LevelRange = [12, 19];
            StrPerLevel = [2, 5];
            IntPerLevel = [1, 2];
            DexPerLevel = [3, 5];
            ResistancePerLevel = [1, 3];

            SetStr(50, 165);
            SetDex(20, 65);
            SetInt(15, 40);
            SetHits(75, 110);
            SetDamage(2, 8);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Fire, 5, 15);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 1, 5);
            SetResistance(ResistanceType.Energy, 1, 5);

            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 3500;
            Karma = -3500;

            VirtualArmor = 40;
        }

        public override string CorpseName => "a troll corpse";
        public override string DefaultName => "a troll";

        public override bool CanRummageCorpses => true;
        public override int TreasureMapLevel => 1;
        public override int Meat => 2;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
        }
    }
}

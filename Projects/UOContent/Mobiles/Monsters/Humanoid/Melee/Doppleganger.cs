using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Doppleganger : BaseCreature
    {
        [Constructible]
        public Doppleganger() : base(AIType.AI_Melee)
        {
            Body = 0x309;
            BaseSoundID = 0x451;

            LevelRange = [1, 20];
            StrPerLevel = [1, 3];
            IntPerLevel = [1, 3];
            DexPerLevel = [1, 3];
            ResistancePerLevel = [1, 2];

            SetStr(25, 40);
            SetDex(15, 25);
            SetInt(25, 40);

            SetDamage(1, 6);

            SetResistance(ResistanceType.Physical, 20, 30);
            SetResistance(ResistanceType.Fire, 1, 10);
            SetResistance(ResistanceType.Cold, 10, 15);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.MagicResist, 40.0, 50.5);
            SetSkill(SkillName.Tactics, 40.0, 50.5);
            SetSkill(SkillName.Wrestling, 40.0, 50.5);

            Fame = 1000;
            Karma = -1000;

            VirtualArmor = 55;
        }

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };
        public override string CorpseName => "a doppleganger corpse";
        public override string DefaultName => "a doppleganger";

        public override int Hides => 6;
        public override int Meat => 1;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
        }
    }
}

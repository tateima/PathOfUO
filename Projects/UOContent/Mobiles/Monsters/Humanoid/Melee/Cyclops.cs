using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Cyclops : BaseCreature
    {
        [Constructible]
        public Cyclops() : base(AIType.AI_Melee)
        {
            Body = 75;
            BaseSoundID = 604;

            LevelRange = [30, 40];
            StrPerLevel = [1, 8];
            IntPerLevel = [1, 2];
            DexPerLevel = [1, 9];
            ResistancePerLevel = [1, 2];

            SetStr(60, 155);
            SetDex(30, 55);
            SetInt(25, 30);
            SetHits(85, 100);
            SetDamage(1, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.MagicResist, 40.0, 50.5);
            SetSkill(SkillName.Tactics, 40.0, 50.5);
            SetSkill(SkillName.Wrestling, 40.0, 50.5);

            Fame = 4500;
            Karma = -4500;

            VirtualArmor = 48;
        }

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };
        public override string CorpseName => "a cyclopean corpse";
        public override string DefaultName => "a cyclopean warrior";

        public override int Meat => 4;
        public override int TreasureMapLevel => 3;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Average);
        }
    }
}

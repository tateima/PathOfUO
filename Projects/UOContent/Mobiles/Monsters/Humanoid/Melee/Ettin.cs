using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Ettin : BaseCreature
    {
        [Constructible]
        public Ettin() : base(AIType.AI_Melee)
        {
            Body = 18;
            BaseSoundID = 367;

            LevelRange = [10, 20];
            StrPerLevel = [1, 8];
            IntPerLevel = [1, 2];
            DexPerLevel = [1, 9];
            ResistancePerLevel = [1, 2];

            SetStr(60, 155);
            SetDex(30, 55);
            SetInt(25, 30);
            SetHits(85, 100);
            SetDamage(1, 8);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 20);
            SetResistance(ResistanceType.Fire, 5, 15);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 15);
            SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 3000;
            Karma = -3000;

            VirtualArmor = 38;
        }

        public override string CorpseName => "an ettins corpse";
        public override string DefaultName => "an ettin";

        public override bool CanRummageCorpses => true;
        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };
        public override int TreasureMapLevel => 1;
        public override int Meat => 4;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Potions);
        }
    }
}

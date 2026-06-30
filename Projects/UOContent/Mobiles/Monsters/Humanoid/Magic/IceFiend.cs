using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class IceFiend : BaseCreature
    {
        [Constructible]
        public IceFiend() : base(AIType.AI_Mage)
        {
            Body = 43;
            BaseSoundID = 357;
            LevelRange = [20, 40];
            StrPerLevel = [3, 6];
            IntPerLevel = [2, 3];
            DexPerLevel = [2, 5];
            ResistancePerLevel = [1, 3];

            SetStr(76, 95);
            SetDex(50, 100);
            SetInt(52, 90);

            SetHits(91, 110);

            SetDamage(1, 6);

            SetSkill(SkillName.EvalInt, 50.0, 60.5);
            SetSkill(SkillName.Magery, 50.0, 60.5);
            SetSkill(SkillName.MagicResist, 50.0, 60.5);
            SetSkill(SkillName.Tactics, 50.0, 60.5);
            SetSkill(SkillName.Wrestling, 50.0, 60.5);

            SetResistance(ResistanceType.Physical, 15, 30);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 20, 40);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 10, 20);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 60;
        }

        public override string CorpseName => "an ice fiend corpse";
        public override string DefaultName => "an ice fiend";
        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder };
        public override int TreasureMapLevel => 4;
        public override int Meat => 1;
        public override bool CanFly => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.MedScrolls, 2);
        }
    }
}

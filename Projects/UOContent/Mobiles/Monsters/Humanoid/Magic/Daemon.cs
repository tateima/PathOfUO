using ModernUO.Serialization;
using Server.Ethics;
using Server.Factions;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Daemon : BaseCreature
    {
        [Constructible]
        public Daemon() : base(AIType.AI_Mage)
        {
            Name = NameList.RandomName("daemon");
            Body = 9;
            BaseSoundID = 357;

            LevelRange = [30, 50];
            StrPerLevel = [1, 4];
            IntPerLevel = [2, 3];
            DexPerLevel = [1, 3];
            ResistancePerLevel = [1, 3];

            SetStr(76, 95);
            SetDex(50, 100);
            SetInt(52, 90);

            SetHits(91, 110);

            SetDamage(3, 7);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Fire, 10, 15);
            SetResistance(ResistanceType.Cold, 10, 25);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.EvalInt, 50.0, 60.5);
            SetSkill(SkillName.Magery, 50.0, 60.5);
            SetSkill(SkillName.MagicResist, 50.0, 60.5);
            SetSkill(SkillName.Tactics, 50.0, 60.5);
            SetSkill(SkillName.Wrestling, 50.0, 60.5);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 58;
        }

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder, OppositionGroup.CelestialsAndDaemons };
        public override string CorpseName => "a daemon corpse";

        public override Faction FactionAllegiance => Shadowlords.Instance;
        public override Ethic EthicAllegiance => Ethic.Evil;

        public override bool CanRummageCorpses => true;

        public override Poison PoisonImmune => Poison.Regular;

        public override int TreasureMapLevel => 4;

        public override int Meat => 1;

        public override bool CanFly => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Average, 2);
            AddLoot(LootPack.MedScrolls, 2);
        }
    }
}

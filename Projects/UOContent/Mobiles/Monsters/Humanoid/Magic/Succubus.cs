using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Succubus : BaseCreature
    {
        [Constructible]
        public Succubus() : base(AIType.AI_Mage)
        {
            Body = 149;
            BaseSoundID = 0x4B0;

            LevelRange = [25, 50];
            StrPerLevel = [1, 4];
            IntPerLevel = [1, 5];
            DexPerLevel = [1, 2];
            ResistancePerLevel = [1, 2];

            SetStr(50, 80);
            SetDex(20, 35);
            SetInt(50, 100);
            SetHits(70, 80);
            SetDamage(1, 5);


            SetDamageType(ResistanceType.Physical, 75);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 20, 30);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.EvalInt, 50.0, 60.5);
            SetSkill(SkillName.Magery, 50.0, 60.5);
            SetSkill(SkillName.Meditation, 50.0, 60.5);
            SetSkill(SkillName.MagicResist, 50.0, 60.5);
            SetSkill(SkillName.Tactics, 50.0, 60.5);
            SetSkill(SkillName.Wrestling, 50.0, 60.5);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 80;
        }

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder, OppositionGroup.CelestialsAndDaemons };
        public override string CorpseName => "a succubus corpse";
        public override string DefaultName => "a succubus";

        public override int Meat => 1;
        public override int TreasureMapLevel => 5;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.MedScrolls, 2);
        }

        private static MonsterAbility[] _abilities = { MonsterAbilities.DrainLifeAreaAttack };
        public override MonsterAbility[] GetMonsterAbilities() => _abilities;
    }
}

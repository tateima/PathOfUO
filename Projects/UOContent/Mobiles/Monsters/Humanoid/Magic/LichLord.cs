using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class LichLord : BaseCreature
    {
        [Constructible]
        public LichLord() : base(AIType.AI_Mage)
        {
            Body = 79;
            BaseSoundID = 412;

            LevelRange = [40, 60];
            StrPerLevel = [2, 3];
            IntPerLevel = [2, 6];
            DexPerLevel = [2, 3];
            ResistancePerLevel = [3, 4];

            SetStr(40, 70);
            SetDex(20, 35);
            SetInt(60, 120);
            SetHits(80, 120);
            SetDamage(2, 7);

            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Cold, 60);
            SetDamageType(ResistanceType.Energy, 40);

            SetResistance(ResistanceType.Physical, 10, 20);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 5, 20);
            SetResistance(ResistanceType.Poison, 5, 30);
            SetResistance(ResistanceType.Energy, 5, 20);

            SetSkill(SkillName.Necromancy, 50.0, 60.5);
            SetSkill(SkillName.SpiritSpeak, 50.0, 60.5);

            SetSkill(SkillName.EvalInt, 50.0, 60.5);
            SetSkill(SkillName.Magery, 50.0, 60.5);
            SetSkill(SkillName.MagicResist, 50.0, 60.5);
            SetSkill(SkillName.Tactics, 50.0, 60.5);
            SetSkill(SkillName.Wrestling, 50.0, 60.5);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 50;
            PackItem(new GnarledStaff());
            PackNecroReg(12, 40);
        }
        public override string CorpseName => "a lich's corpse";
        public override string DefaultName => "a lich lord";

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };

        public override bool CanRummageCorpses => true;
        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int TreasureMapLevel => 4;

        private static MonsterAbility[] _abilities = { MonsterAbilities.SummonSkeletonsCounter };
        public override MonsterAbility[] GetMonsterAbilities() => _abilities;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.MedScrolls, 2);
        }
    }
}

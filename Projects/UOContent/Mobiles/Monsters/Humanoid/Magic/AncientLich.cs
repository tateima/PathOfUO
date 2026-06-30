using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class AncientLich : BaseCreature
    {
        [Constructible]
        public AncientLich() : base(AIType.AI_Mage)
        {
            Name = NameList.RandomName("ancient lich");
            Body = 78;
            BaseSoundID = 412;

            LevelRange = [55, 75];
            StrPerLevel = [3, 5];
            IntPerLevel = [3, 9];
            DexPerLevel = [3, 4];
            ResistancePerLevel = [3, 5];

            SetStr(76, 95);
            SetDex(50, 70);
            SetInt(100, 160);

            SetHits(170, 270);

            SetDamage(2, 8);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Cold, 40);
            SetDamageType(ResistanceType.Energy, 40);

            SetResistance(ResistanceType.Physical, 15, 35);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.EvalInt, 50.0, 60.5);
            SetSkill(SkillName.Magery, 50.0, 60.5);
            SetSkill(SkillName.Meditation, 50.0, 60.5);
            SetSkill(SkillName.Poisoning, 50.0, 60.5);
            SetSkill(SkillName.MagicResist, 50.0, 60.5);
            SetSkill(SkillName.Tactics, 50.0, 60.5);
            SetSkill(SkillName.Wrestling, 50.0, 60.5);
            SetSkill(SkillName.Necromancy, 50.0, 60.5);
            SetSkill(SkillName.SpiritSpeak, 50.0, 60.5);

            Fame = 23000;
            Karma = -23000;

            VirtualArmor = 60;
            PackNecroReg(30, 275);
        }

        public override string CorpseName => "an ancient lich's corpse";

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };

        public override bool Unprovokable => true;
        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int TreasureMapLevel => 5;

        public override int GetIdleSound() => 0x19D;

        public override int GetAngerSound() => 0x175;

        public override int GetDeathSound() => 0x108;

        public override int GetAttackSound() => 0xE2;

        public override int GetHurtSound() => 0x28B;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 3);
            AddLoot(LootPack.MedScrolls, 2);
        }

        private static MonsterAbility[] _abilities = { MonsterAbilities.SummonLesserUndeadCounter };
        public override MonsterAbility[] GetMonsterAbilities() => _abilities;
    }
}

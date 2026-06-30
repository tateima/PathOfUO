using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class RottingCorpse : BaseCreature
    {
        [Constructible]
        public RottingCorpse() : base(AIType.AI_Melee)
        {
            Body = 155;
            BaseSoundID = 471;
            LevelRange = [9, 12];
            StrPerLevel = [2, 6];
            IntPerLevel = [4, 6];
            DexPerLevel = [5, 8];
            ResistancePerLevel = [1, 2];

            SetStr(60, 165);
            SetDex(60, 85);
            SetInt(35, 50);
            SetHits(85, 105);
            SetDamage(4, 10);

            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Cold, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 10, 25);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 10, 30);
            SetResistance(ResistanceType.Poison, 10, 30);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.Poisoning, 40.1, 50.0);
            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 40;
        }

        public override string CorpseName => "a rotting corpse";
        public override string DefaultName => "a rotting corpse";

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override Poison HitPoison => Poison.Lethal;
        public override int TreasureMapLevel => 5;

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
        }
    }
}

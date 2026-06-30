using ModernUO.Serialization;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class GoreFiend : BaseCreature
    {
        [Constructible]
        public GoreFiend() : base(AIType.AI_Melee)
        {
            Body = 305;
            BaseSoundID = 224;
            LevelRange = [7, 16];
            StrPerLevel = [2, 4];
            IntPerLevel = [1, 2];
            DexPerLevel = [1, 2];
            ResistancePerLevel = [1, 2];

            SetStr(41, 70);
            SetDex(30, 65);
            SetInt(26, 60);

            SetHits(75, 80);

            SetDamage(3, 6);

            SetDamageType(ResistanceType.Physical, 85);
            SetDamageType(ResistanceType.Poison, 15);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Fire, 5, 15);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 5, 15);
            SetResistance(ResistanceType.Energy, 10, 30);

            SetSkill(SkillName.MagicResist, 40.1, 55.0);
            SetSkill(SkillName.Tactics, 45.1, 55.0);
            SetSkill(SkillName.Wrestling, 50.1, 55.0);

            Fame = 1500;
            Karma = -1500;

            VirtualArmor = 24;
        }

        public override string CorpseName => "a gore fiend corpse";
        public override string DefaultName => "a gore fiend";

        public override bool BleedImmune => true;
        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder };


        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
        }

        public override int GetDeathSound() => 1218;
    }
}

using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class ArcaneDaemon : BaseCreature
    {
        [Constructible]
        public ArcaneDaemon() : base(AIType.AI_Mage)
        {
            Body = 0x310;
            BaseSoundID = 0x47D;
            LevelRange = [50, 70];
            StrPerLevel = [3, 5];
            IntPerLevel = [3, 5];
            DexPerLevel = [3, 5];
            ResistancePerLevel = [2, 4];

            SetStr(86, 135);
            SetDex(60, 90);
            SetInt(62, 120);

            SetHits(190, 220);

            SetDamage(2, 10);

            SetDamageType(ResistanceType.Physical, 80);
            SetDamageType(ResistanceType.Fire, 20);

            SetResistance(ResistanceType.Physical, 10, 30);
            SetResistance(ResistanceType.Fire, 15, 30);
            SetResistance(ResistanceType.Cold, 6, 20);
            SetResistance(ResistanceType.Poison, 15, 30);
            SetResistance(ResistanceType.Energy, 5, 20);

            SetSkill(SkillName.MagicResist, 50.0, 60.5);
            SetSkill(SkillName.Tactics, 50.0, 60.5);
            SetSkill(SkillName.Wrestling, 50.0, 60.5);
            SetSkill(SkillName.Magery, 50.0, 60.5);
            SetSkill(SkillName.EvalInt, 50.0, 60.5);
            SetSkill(SkillName.Meditation, 50.0, 60.5);

            Fame = 7000;
            Karma = -10000;

            VirtualArmor = 55;
        }

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder, OppositionGroup.CelestialsAndDaemons };
        public override string CorpseName => "an arcane daemon corpse";

        public override string DefaultName => "an arcane daemon";

        public override Poison PoisonImmune => Poison.Deadly;

        public override WeaponAbility GetWeaponAbility() => WeaponAbility.ConcussionBlow;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average, 2);
        }
    }
}

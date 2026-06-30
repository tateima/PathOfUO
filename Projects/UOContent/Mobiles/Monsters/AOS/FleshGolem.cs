using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class FleshGolem : BaseCreature
    {
        [Constructible]
        public FleshGolem() : base(AIType.AI_Melee)
        {
            Body = 304;
            BaseSoundID = 684;

            LevelRange = [13, 26];
            StrPerLevel = [3, 5];
            IntPerLevel = [1, 2];
            DexPerLevel = [2, 3];
            ResistancePerLevel = [1, 2];

            SetStr(61, 90);
            SetDex(40, 75);
            SetInt(36, 50);
            SetHits(85, 90);

            SetDamage(4, 7);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 30);
            SetResistance(ResistanceType.Fire, 5, 25);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 10, 30);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.MagicResist, 50.1, 55.0);
            SetSkill(SkillName.Tactics, 45.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 1000;
            Karma = -1800;

            VirtualArmor = 34;
        }

        public override string CorpseName => "a flesh golem corpse";

        public override string DefaultName => "a flesh golem";

        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 1;

        public override WeaponAbility GetWeaponAbility() => WeaponAbility.BleedAttack;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
        }
    }
}

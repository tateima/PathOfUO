using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Moloch : BaseCreature
    {
        [Constructible]
        public Moloch() : base(AIType.AI_Melee)
        {
            Body = 0x311;
            BaseSoundID = 0x300;

            LevelRange = [9, 14];
            StrPerLevel = [2, 5];
            IntPerLevel = [3, 5];
            DexPerLevel = [4, 9];
            ResistancePerLevel = [2, 4];

            SetStr(60, 145);
            SetDex(50, 65);
            SetInt(55, 80);
            SetHits(65, 85);
            SetDamage(4, 10);

            SetResistance(ResistanceType.Physical, 20, 30);
            SetResistance(ResistanceType.Fire, 10, 30);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 1, 12);
            SetResistance(ResistanceType.Energy, 3, 15);

            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 7500;
            Karma = -7500;

            VirtualArmor = 32;
        }

        public override string CorpseName => "a moloch corpse";

        public override string DefaultName => "a moloch";

        public override Poison PoisonImmune => Poison.Regular;

        public override WeaponAbility GetWeaponAbility() => WeaponAbility.ConcussionBlow;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
        }
    }
}

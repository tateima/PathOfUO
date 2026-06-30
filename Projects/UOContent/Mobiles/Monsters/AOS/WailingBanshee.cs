using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class WailingBanshee : BaseCreature
    {
        [Constructible]
        public WailingBanshee() : base(AIType.AI_Melee)
        {
            Body = 310;
            BaseSoundID = 0x482;
            LevelRange = [5, 12];
            StrPerLevel = [2, 4];
            IntPerLevel = [2, 4];
            DexPerLevel = [2, 4];
            ResistancePerLevel = [1, 2];

            SetStr(25, 60);
            SetDex(20, 35);
            SetInt(10, 30);

            SetHits(66, 70);

            SetDamage(2, 5);

            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Cold, 60);
            SetDamageType(ResistanceType.Poison, 20);

            SetResistance(ResistanceType.Physical, 10, 20);
            SetResistance(ResistanceType.Fire, 5, 20);
            SetResistance(ResistanceType.Cold, 10, 30);
            SetResistance(ResistanceType.Poison, 5, 20);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.MagicResist, 45.1, 50.0);
            SetSkill(SkillName.Tactics, 45.1, 50.0);
            SetSkill(SkillName.Wrestling, 45.1, 50.0);

            Fame = 1500;
            Karma = -1500;

            VirtualArmor = 19;
        }

        public override string CorpseName => "a wailing banshee corpse";

        public override string DefaultName => "a wailing banshee";

        public override bool BleedImmune => true;

        public override WeaponAbility GetWeaponAbility() => WeaponAbility.MortalStrike;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager);
        }
    }
}

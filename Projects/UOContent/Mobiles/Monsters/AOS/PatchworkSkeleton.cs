using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class PatchworkSkeleton : BaseCreature
    {
        [Constructible]
        public PatchworkSkeleton() : base(AIType.AI_Melee)
        {
            Body = 309;
            BaseSoundID = 0x48D;
            LevelRange = [5, 20];
            StrPerLevel = [2, 5];
            IntPerLevel = [1, 2];
            DexPerLevel = [2, 3];
            ResistancePerLevel = [1, 2];

            SetStr(36, 60);
            SetDex(31, 55);
            SetInt(6, 20);

            SetHits(58, 72);

            SetDamage(2, 8);

            SetDamageType(ResistanceType.Physical, 85);
            SetDamageType(ResistanceType.Cold, 15);

            SetResistance(ResistanceType.Physical, 15, 25);
            SetResistance(ResistanceType.Fire, 5, 15);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 5, 30);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.MagicResist, 70.1, 95.0);
            SetSkill(SkillName.Tactics, 55.1, 80.0);
            SetSkill(SkillName.Wrestling, 50.1, 70.0);

            Fame = 500;
            Karma = -500;

            VirtualArmor = 54;
        }

        public override string CorpseName => "a patchwork skeletal corpse";

        public override string DefaultName => "a patchwork skeleton";

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;

        public override int TreasureMapLevel => 1;

        public override WeaponAbility GetWeaponAbility() => WeaponAbility.Dismount;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager);
        }
    }
}

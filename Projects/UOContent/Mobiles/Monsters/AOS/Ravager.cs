using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Ravager : BaseCreature
    {
        [Constructible]
        public Ravager() : base(AIType.AI_Melee)
        {
            Body = 314;
            BaseSoundID = 357;
            LevelRange = [15, 35];
            StrPerLevel = [2, 5];
            IntPerLevel = [1, 2];
            DexPerLevel = [2, 3];
            ResistancePerLevel = [1, 2];

            SetStr(55, 70);
            SetDex(35, 56);
            SetInt(20, 30);

            SetHits(75, 89);

            SetDamage(1, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 30);
            SetResistance(ResistanceType.Fire, 10, 30);
            SetResistance(ResistanceType.Cold, 5, 20);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.MagicResist, 45.1, 50.0);
            SetSkill(SkillName.Tactics, 45.1, 50.0);
            SetSkill(SkillName.Wrestling, 45.1, 50.0);

            Fame = 3500;
            Karma = -3500;

            VirtualArmor = 54;
        }

        public override string CorpseName => "a ravager corpse";

        public override string DefaultName => "a ravager";

        public override WeaponAbility GetWeaponAbility() =>
            Utility.RandomBool() ? WeaponAbility.Dismount : WeaponAbility.CrushingBlow;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
        }
    }
}

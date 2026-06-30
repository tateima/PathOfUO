using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [TypeAlias("Server.Mobiles.FerelTreefellow")]

    [SerializationGenerator(0, false)]
    public partial class FeralTreefellow : BaseCreature
    {
        [Constructible]
        public FeralTreefellow() : base(AIType.AI_Melee, FightMode.Aggressor)
        {
            Body = 301;

            LevelRange = [55, 65];
            StrPerLevel = [4, 12];
            IntPerLevel = [3, 5];
            DexPerLevel = [3, 5];
            ResistancePerLevel = [2, 4];

            SetStr(106, 180);
            SetDex(50, 70);
            SetInt(42, 90);

            SetHits(210, 250);

            SetDamage(3, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Cold, 10, 25);
            SetResistance(ResistanceType.Poison, 10, 35);
            SetResistance(ResistanceType.Energy, 10, 25);

            SetSkill(SkillName.MagicResist, 40.1, 55.0); // Unknown
            SetSkill(SkillName.Tactics, 40.1, 55.0);     // Unknown
            SetSkill(SkillName.Wrestling, 40, 55.0);     // Unknown

            Fame = 12500;  // Unknown
            Karma = 12500; // Unknown

            VirtualArmor = 24;
            PackItem(new Log(Utility.RandomMinMax(23, 34)));
        }

        public override string CorpseName => "a treefellow corpse";

        public override string DefaultName => "a feral treefellow";

        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.DarknessAndLight };

        public override bool BleedImmune => true;

        public override WeaponAbility GetWeaponAbility() => WeaponAbility.Dismount;

        public override int GetIdleSound() => 443;

        public override int GetDeathSound() => 31;

        public override int GetAttackSound() => 672;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average); // Unknown
        }
    }
}

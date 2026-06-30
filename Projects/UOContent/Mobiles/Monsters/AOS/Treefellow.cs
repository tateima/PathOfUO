using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class Treefellow : BaseCreature
    {
        [Constructible]
        public Treefellow() : base(AIType.AI_Melee, FightMode.Aggressor)
        {
            Body = 301;
            LevelRange = [14, 28];
            StrPerLevel = [2, 6];
            IntPerLevel = [1, 2];
            DexPerLevel = [1, 3];
            ResistancePerLevel = [1, 2];
            SetStr(96, 120);
            SetDex(11, 35);
            SetInt(26, 40);

            SetHits(100, 125);

            SetDamage(2, 8);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 25);
            SetResistance(ResistanceType.Cold, 10, 20);
            SetResistance(ResistanceType.Poison, 5, 15);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.MagicResist, 40.1, 55.0);
            SetSkill(SkillName.Tactics, 40.1, 55.0);
            SetSkill(SkillName.Wrestling, 40.1, 55.0);

            Fame = 500;
            Karma = 1500;

            VirtualArmor = 24;
            PackItem(new Log(Utility.RandomMinMax(23, 34)));
        }

        public override string CorpseName => "a treefellow corpse";

        public override string DefaultName => "a treefellow";

        public override OppositionGroup[] OppositionGroups => new []{ OppositionGroup.DarknessAndLight };

        public override bool BleedImmune => true;

        public override WeaponAbility GetWeaponAbility() => WeaponAbility.Dismount;

        public override int GetIdleSound() => 443;

        public override int GetDeathSound() => 31;

        public override int GetAttackSound() => 672;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
        }
    }
}

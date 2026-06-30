using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class CrystalElemental : BaseCreature
    {
        [Constructible]
        public CrystalElemental() : base(AIType.AI_Mage)
        {
            Body = 300;
            BaseSoundID = 278;
            LevelRange = [22, 36];
            StrPerLevel = [2, 6];
            IntPerLevel = [1, 2];
            DexPerLevel = [2, 6];
            ResistancePerLevel = [1, 3];

            SetStr(80, 185);
            SetDex(40, 85);
            SetInt(35, 60);
            SetHits(95, 140);

            SetDamage(2, 8);

            SetDamageType(ResistanceType.Physical, 80);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 20, 35);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 15);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.EvalInt, 40.1, 50.0);
            SetSkill(SkillName.Magery, 40.1, 50.0);
            SetSkill(SkillName.Meditation, 40.1, 50.0);
            SetSkill(SkillName.MagicResist, 40.1, 50.0);
            SetSkill(SkillName.Tactics, 40.1, 50.0);
            SetSkill(SkillName.Wrestling, 40.1, 50.0);

            Fame = 6500;
            Karma = -6500;

            VirtualArmor = 54;
        }

        public override string CorpseName => "a crystal elemental corpse";

        public override string DefaultName => "a crystal elemental";

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int TreasureMapLevel => 1;

        public override WeaponAbility GetWeaponAbility() => WeaponAbility.BleedAttack;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Average);
        }
    }
}

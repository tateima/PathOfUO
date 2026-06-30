using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class TormentedMinotaur : BaseCreature
    {
        [Constructible]
        public TormentedMinotaur() : base(AIType.AI_Melee)
        {
            Body = 262;

            LevelRange = [53, 63];
            StrPerLevel = [3, 9];
            IntPerLevel = [1, 2];
            DexPerLevel = [3, 8];
            ResistancePerLevel = [1, 3];

            SetStr(80, 165);
            SetDex(40, 85);
            SetInt(25, 50);
            SetHits(95, 155);
            SetDamage(3, 9);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Fire, 10, 30);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 20);
            SetResistance(ResistanceType.Energy, 10, 25);

            SetSkill(SkillName.MagicResist, 45, 50);
            SetSkill(SkillName.Tactics, 45, 50.8);
            SetSkill(SkillName.Wrestling, 45.4, 50.1);

            Fame = 20000;
            Karma = -20000;
        }

        public override string CorpseName => "a tormented minotaur corpse";
        public override string DefaultName => "Tormented Minotaur";
        public override Poison PoisonImmune => Poison.Deadly;
        public override int TreasureMapLevel => 3;
        public override WeaponAbility GetWeaponAbility() => WeaponAbility.Dismount;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 10);
        }

        public override int GetDeathSound() => 0x596;

        public override int GetAttackSound() => 0x597;

        public override int GetIdleSound() => 0x598;

        public override int GetAngerSound() => 0x599;

        public override int GetHurtSound() => 0x59A;
    }
}

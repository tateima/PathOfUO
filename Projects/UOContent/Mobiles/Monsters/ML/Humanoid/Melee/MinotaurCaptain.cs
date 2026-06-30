using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class MinotaurCaptain : BaseCreature
    {
        [Constructible]
        public MinotaurCaptain() : base(AIType.AI_Melee) // NEED TO CHECK
        {
            Body = 280;

            LevelRange = [40, 50];
            StrPerLevel = [2, 8];
            IntPerLevel = [1, 2];
            DexPerLevel = [4, 6];
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

            SetSkill(SkillName.Meditation, 0);
            SetSkill(SkillName.EvalInt, 0);
            SetSkill(SkillName.Magery, 0);
            SetSkill(SkillName.Poisoning, 0);
            SetSkill(SkillName.Anatomy, 0);
            SetSkill(SkillName.MagicResist, 45, 50);
            SetSkill(SkillName.Tactics, 45, 50.8);
            SetSkill(SkillName.Wrestling, 45.4, 50.1);

            Fame = 7000;
            Karma = -7000;

            VirtualArmor = 28; // Don't know what it should be
        }

        public override string CorpseName => "a minotaur corpse";

        public override string DefaultName => "a minotaur captain";

        public override WeaponAbility GetWeaponAbility() => WeaponAbility.ParalyzingBlow;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich); // Need to verify
        }

        // Using Tormented Minotaur sounds - Need to veryfy
        public override int GetAngerSound() => 0x597;

        public override int GetIdleSound() => 0x596;

        public override int GetAttackSound() => 0x599;

        public override int GetHurtSound() => 0x59a;

        public override int GetDeathSound() => 0x59c;
    }
}

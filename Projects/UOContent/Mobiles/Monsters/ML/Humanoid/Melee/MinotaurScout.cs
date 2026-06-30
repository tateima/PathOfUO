using ModernUO.Serialization;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class MinotaurScout : BaseCreature
    {
        [Constructible]
        public MinotaurScout() : base(AIType.AI_Melee) // NEED TO CHECK
        {
            Body = 281;

            LevelRange = [30, 40];
            StrPerLevel = [2, 8];
            IntPerLevel = [1, 2];
            DexPerLevel = [4, 6];
            ResistancePerLevel = [1, 3];

            SetStr(60, 135);
            SetDex(70, 135);
            SetInt(25, 50);
            SetHits(75, 135);
            SetDamage(2, 6);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Fire, 10, 30);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 20);
            SetResistance(ResistanceType.Energy, 10, 25);

            // SetSkill( SkillName.Meditation, Unknown );
            // SetSkill( SkillName.EvalInt, Unknown );
            // SetSkill( SkillName.Magery, Unknown );
            // SetSkill( SkillName.Poisoning, Unknown );
            SetSkill(SkillName.Anatomy, 45.4, 50.1);
            SetSkill(SkillName.MagicResist, 45.4, 50.1);
            SetSkill(SkillName.Tactics, 45.4, 50.1);
            SetSkill(SkillName.Wrestling, 45.4, 50.1);

            Fame = 5000;
            Karma = -5000;

            VirtualArmor = 28; // Don't know what it should be
        }

        public override string CorpseName => "a minotaur corpse";

        public override string DefaultName => "a minotaur scout";

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

using ModernUO.Serialization;
using System;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class AnimatedWeapon : BaseCreature
    {
        [Constructible]
        public AnimatedWeapon(Mobile caster, int level) : base(AIType.AI_Melee)
        {
            Body = 692;
            StrPerLevel = [2, 4];
            IntPerLevel = [1, 1];
            DexPerLevel = [1, 2];
            ResistancePerLevel = [1, 2];
            LevelRange = [level, level];
            SetStr(10);
            SetDex(10);
            SetInt(10);

            SetHits(20);
            SetStam(10);
            SetMana(0);

            if (level >= 80)
            {
                SetDamage(5, 11);
            }
            else if (level >= 70)
            {
                SetDamage(4, 10);
            }
            else if (level >= 60)
            {
                SetDamage(3, 9);
            }
            else if (level >= 50)
            {
                SetDamage(2, 8);
            }
            else if (level >= 40)
            {
                SetDamage(1, 7);
            }
            else if (level >= 30)
            {
                SetDamage(1, 6);
            }
            else if (level >= 20)
            {
                SetDamage(1, 5);
            }
            else if (level >= 10)
            {
                SetDamage(1, 4);
            }
            else
            {
                SetDamage(1, 3);
            }

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Poison, 20);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 6, 25);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 10, 30);
            SetResistance(ResistanceType.Poison, 10, 15);
            SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.MagicResist, 40.0, 50.0);
            SetSkill(SkillName.Wrestling, 40.0, 50.0);
            SetSkill(SkillName.Anatomy, 40.0, 50.0);
            SetSkill(SkillName.Tactics, 40.0, 50.0);

            Fame = 0;
            Karma = 0;

            ControlSlots = 4;
        }

        public override string CorpseName => "an animated weapon corpse";
        public override bool DeleteCorpseOnDeath => true;
        public override bool IsHouseSummonable => true;

        public override double DispelDifficulty => 0.0;
        public override double DispelFocus => 20.0;

        public override string DefaultName => "an animated weapon";

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;

        public override double GetFightModeRanking(Mobile m, FightMode acqType, bool bPlayerOnly) =>
            m.Str / Math.Max(this.GetDistanceToSqrt(m), 1.0);

        public override int GetAngerSound() => 0x23A;

        public override int GetAttackSound() => 0x3B8;

        public override int GetHurtSound() => 0x23A;
    }
}

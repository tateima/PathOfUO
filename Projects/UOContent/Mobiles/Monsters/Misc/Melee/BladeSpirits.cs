using ModernUO.Serialization;
using System;
using Server.Collections;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class BladeSpirits : BaseCreature
    {
        [Constructible]
        public BladeSpirits(int level) : base(AIType.AI_Melee)
        {
            Body = 574;
            StrPerLevel = [1, 3];
            IntPerLevel = [1, 1];
            DexPerLevel = [2, 3];
            ResistancePerLevel = [1, 2];
            LevelRange = [level, level];
            SetStr(20);
            SetDex(20);
            SetInt(20);

            SetHits(30);
            SetStam(20);
            SetMana(0);

            if (level >= 80)
            {
                SetDamage(4, 10);
            }
            else if (level >= 70)
            {
                SetDamage(3, 9);
            }
            else if (level >= 60)
            {
                SetDamage(2, 8);
            }
            else if (level >= 50)
            {
                SetDamage(1, 7);
            }
            else if (level >= 40)
            {
                SetDamage(1, 6);
            }
            else if (level >= 30)
            {
                SetDamage(1, 5);
            }
            else if (level >= 20)
            {
                SetDamage(1, 4);
            }
            else if (level >= 10)
            {
                SetDamage(1, 3);
            }
            else
            {
                SetDamage(1, 2);
            }

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Poison, 20);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 10, 30);
            SetResistance(ResistanceType.Fire, 5, 15);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 5, 15);
            SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.MagicResist, 40.0, 50.0);
            SetSkill(SkillName.Tactics, 40.0, 50.0);
            SetSkill(SkillName.Wrestling, 40.0, 50.0);

            Fame = 0;
            Karma = 0;

            VirtualArmor = 40;
            ControlSlots = Core.SE ? 2 : 1;
        }

        public override string CorpseName => "a blade spirit corpse";
        public override string DefaultName => "a blade spirit";

        public override bool DeleteCorpseOnDeath => Core.AOS;
        public override bool IsHouseSummonable => true;

        public override double DispelDifficulty => 0.0;
        public override double DispelFocus => 20.0;

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;

        public override bool FollowsAcquireRules => Core.AOS || !Summoned || SummonMaster?.Player != true || Map != Map.Felucca;

        public override double GetFightModeRanking(Mobile m, FightMode acqType, bool bPlayerOnly) =>
            (m.Str + m.Skills.Tactics.Value) / Math.Max(this.GetDistanceToSqrt(m), 1.0);

        public override int GetAngerSound() => 0x23A;

        public override int GetAttackSound() => 0x3B8;

        public override int GetHurtSound() => 0x23A;

        public override void OnThink()
        {
            if (Core.SE && Summoned)
            {
                using var list = PooledRefList<Mobile>.Create();
                foreach (var m in GetMobilesInRange(5))
                {
                    if (m is EnergyVortex or BladeSpirits && ((BaseCreature)m).Summoned)
                    {
                        list.Add(m);
                    }
                }

                var amount = list.Count - 6;
                if (amount > 0)
                {
                    list.Shuffle();

                    while (amount > 0)
                    {
                        Dispel(list[amount--]);
                    }
                }
            }

            base.OnThink();
        }
    }
}

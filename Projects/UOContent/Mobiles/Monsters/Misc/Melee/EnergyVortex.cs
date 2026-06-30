using ModernUO.Serialization;
using System;
using Server.Collections;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class EnergyVortex : BaseCreature
    {
        [Constructible]
        public EnergyVortex(int level) : base(AIType.AI_Melee)
        {
            if (Core.SE && Utility.Random(500) == 0) // Per OSI FoF, it's a 1/500 chance.
            {
                // Llama vortex!
                Body = 0xDC;
                Hue = 0x76;
            }
            else
            {
                Body = 164;
            }
            StrPerLevel = [2, 4];
            IntPerLevel = [2, 3];
            DexPerLevel = [2, 4];
            ResistancePerLevel = [1, 2];
            LevelRange = [level, level];
            SetStr(50);
            SetDex(50);
            SetInt(30);

            SetHits(60);
            SetStam(50);
            SetMana(0);

            if (level >= 80)
            {
                SetDamage(5, 10);
            }
            else if (level >= 70)
            {
                SetDamage(4, 9);
            }
            else if (level >= 60)
            {
                SetDamage(3 ,8);
            }
            else if (level >= 50)
            {
                SetDamage(2, 7);
            }
            else if (level >= 40)
            {
                SetDamage(2, 6);
            }
            else if (level >= 30)
            {
                SetDamage(2, 5);
            }
            else if (level >= 20)
            {
                SetDamage(2, 4);
            }
            else if (level >= 10)
            {
                SetDamage(2, 3);
            }
            else
            {
                SetDamage(1, 2);
            }
            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Energy, 100);

            SetResistance(ResistanceType.Physical, 5, 30);
            SetResistance(ResistanceType.Fire, 5, 20);
            SetResistance(ResistanceType.Cold, 5, 20);
            SetResistance(ResistanceType.Poison, 5, 20);
            SetResistance(ResistanceType.Energy, 10, 30);

            SetSkill(SkillName.MagicResist, 40.0, 50.0);
            SetSkill(SkillName.Tactics, 40.0, 50.0);
            SetSkill(SkillName.Wrestling, 40.0, 50.0);

            Fame = 0;
            Karma = 0;

            VirtualArmor = 40;
            ControlSlots = Core.SE ? 2 : 1;
        }

        public override string CorpseName => "an energy vortex corpse";
        public override bool DeleteCorpseOnDeath => Summoned;
        public override bool AlwaysMurderer => true; // Or Llama vortices will appear gray.

        public override double DispelDifficulty => 80.0;
        public override double DispelFocus => 20.0;

        public override string DefaultName => "an energy vortex";

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;

        public override bool FollowsAcquireRules => Core.AOS || !Summoned || SummonMaster?.Player != true || Map != Map.Felucca;

        public override double GetFightModeRanking(Mobile m, FightMode acqType, bool bPlayerOnly) =>
            (m.Int + m.Skills.Magery.Value) / Math.Max(this.GetDistanceToSqrt(m), 1.0);

        public override int GetAngerSound() => 0x15;

        public override int GetAttackSound() => 0x28;

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

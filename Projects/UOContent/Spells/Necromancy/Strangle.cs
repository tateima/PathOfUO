using System;
using System.Collections.Generic;
using Server.Targeting;

namespace Server.Spells.Necromancy
{
    public class StrangleSpell : NecromancerSpell, ISpellTargetingMobile
    {
        private static readonly SpellInfo m_Info = new(
            "Strangle",
            "In Bal Nox",
            209,
            9031,
            Reagent.DaemonBlood,
            Reagent.NoxCrystal
        );

        private static readonly Dictionary<Mobile, InternalTimer> m_Table = new();

        public StrangleSpell(Mobile caster, Item scroll = null) : base(caster, scroll, m_Info)
        {
        }

        public override TimeSpan CastDelayBase => TimeSpan.FromSeconds(2.0);

        public override double RequiredSkill => 65.0;
        public override int RequiredMana => 29;

        public void Target(Mobile m)
        {
            if (m == null)
            {
                return;
            }

            if (CheckHSequence(m))
            {
                SpellHelper.Turn(Caster, m);

                // SpellHelper.CheckReflect( (int)this.Circle, Caster, ref m );
                // Irrelevent after AoS

                /* Temporarily chokes off the air suply of the target with poisonous fumes.
                 * The target is inflicted with poison damage over time.
                 * The amount of damage dealt each "hit" is based off of the caster's Spirit Speak skill and the Target's current Stamina.
                 * The less Stamina the target has, the more damage is done by Strangle.
                 * Duration of the effect is Spirit Speak skill level / 10 rounds, with a minimum number of 4 rounds.
                 * The first round of damage is dealt after 5 seconds, and every next round after that comes 1 second sooner than the one before, until there is only 1 second between rounds.
                 * The base damage of the effect lies between (Spirit Speak skill level / 10) - 2 and (Spirit Speak skill level / 10) + 1.
                 * Base damage is multiplied by the following formula: (3 - (target's current Stamina / target's maximum Stamina) * 2).
                 * Example:
                 * For a target at full Stamina the damage multiplier is 1,
                 * for a target at 50% Stamina the damage multiplier is 2 and
                 * for a target at 20% Stamina the damage multiplier is 2.6
                 */

                m.Spell?.OnCasterHurt();

                m.PlaySound(0x22F);
                m.FixedParticles(0x36CB, 1, 9, 9911, 67, 5, EffectLayer.Head);
                m.FixedParticles(0x374A, 1, 17, 9502, 1108, 4, (EffectLayer)255);

                if (!m_Table.TryGetValue(m, out var timer))
                {
                    m_Table[m] = timer = new InternalTimer(m, Caster);
                    timer.Start();
                }

                HarmfulSpell(m);
            }

            // Calculations for the buff bar
            var spiritlevel = Caster.Skills.SpiritSpeak.Value / 10;
            if (spiritlevel < 4)
            {
                spiritlevel = 4;
            }

            var d_MinDamage = 4;
            var d_MaxDamage = ((int)spiritlevel + 1) * 3;
            if (!HasReagents())
            {
                d_MinDamage = 2;
                d_MaxDamage = (int)(((int)spiritlevel + 1) * 1.5);
            }

            var args = $"{d_MinDamage}\t{d_MaxDamage}";

            var i_Count = (int)spiritlevel;
            var i_MaxCount = i_Count;
            var i_HitDelay = 5;
            var i_Length = i_HitDelay;

            while (i_Count > 1)
            {
                --i_Count;
                if (i_HitDelay > 1)
                {
                    if (i_MaxCount < 5)
                    {
                        --i_HitDelay;
                    }
                    else
                    {
                        var delay = (int)Math.Ceiling((1.0 + 5 * i_Count) / i_MaxCount);

                        i_HitDelay = delay <= 5 ? delay : 5;
                    }
                }

                i_Length += i_HitDelay;
            }

            var t_Duration = TimeSpan.FromSeconds(i_Length);
            t_Duration *= ReagentsScale();
            BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.Strangle, 1075794, 1075795, t_Duration, m, args));

            FinishSequence();
        }

        public override void OnCast()
        {
            Caster.Target = new SpellTargetMobile(this, TargetFlags.Harmful, Core.ML ? 10 : 12);
        }

        public static bool RemoveCurse(Mobile m)
        {
            if (!m_Table.Remove(m, out var timer))
            {
                return false;
            }

            timer.Stop();
            m.SendLocalizedMessage(1061687); // You can breath normally again.
            return true;
        }

        private class InternalTimer : Timer
        {
            private readonly Mobile m_From;
            private readonly double m_MaxBaseDamage;
            private readonly int m_MaxCount;
            private readonly double m_MinBaseDamage;
            private readonly Mobile m_Target;
            private int m_Count;
            private int m_HitDelay;

            private DateTime m_NextHit;

            public InternalTimer(Mobile target, Mobile from) : base(TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(0.1))
            {

                m_Target = target;
                m_From = from;

                var spiritLevel = from.Skills.SpiritSpeak.Value / 10;

                m_MinBaseDamage = spiritLevel - 2;
                m_MaxBaseDamage = spiritLevel + 1;

                m_HitDelay = 5;
                m_NextHit = Core.Now + TimeSpan.FromSeconds(m_HitDelay);

                m_Count = (int)spiritLevel;

                if (m_Count < 4)
                {
                    m_Count = 4;
                }

                m_MaxCount = m_Count;
            }

            protected override void OnTick()
            {
                if (!m_Target.Alive)
                {
                    m_Table.Remove(m_Target);
                    Stop();
                }

                if (!m_Target.Alive || Core.Now < m_NextHit)
                {
                    return;
                }

                --m_Count;

                if (m_HitDelay > 1)
                {
                    if (m_MaxCount < 5)
                    {
                        --m_HitDelay;
                    }
                    else
                    {
                        var delay = (int)Math.Ceiling((1.0 + 5 * m_Count) / m_MaxCount);

                        if (delay <= 5)
                        {
                            m_HitDelay = delay;
                        }
                        else
                        {
                            m_HitDelay = 5;
                        }
                    }
                }

                if (m_Count == 0)
                {
                    m_Target.SendLocalizedMessage(1061687); // You can breath normally again.
                    m_Table.Remove(m_Target);
                    Stop();
                }
                else
                {
                    m_NextHit = Core.Now + TimeSpan.FromSeconds(m_HitDelay);

                    var damage = m_MinBaseDamage + Utility.RandomDouble() * (m_MaxBaseDamage - m_MinBaseDamage);

                    damage *= 3 - (double)m_Target.Stam / m_Target.StamMax * 2;

                    if (damage < 1)
                    {
                        damage = 1;
                    }

                    if (!m_Target.Player)
                    {
                        damage *= 1.75;
                    }

                    AOS.Damage(m_Target, m_From, (int)damage, 0, 0, 0, 100, 0);

                    if (Utility.RandomDouble() >= 0.60
                    ) // OSI: randomly revealed between first and third damage tick, guessing 60% chance
                    {
                        m_Target.RevealingAction();
                    }
                }
            }
        }
    }
}

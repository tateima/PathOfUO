using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Spells.Necromancy
{
    public class EvilOmenSpell : NecromancerSpell, ISpellTargetingMobile
    {
        private static readonly SpellInfo m_Info = new(
            "Evil Omen",
            "Pas Tym An Sanct",
            203,
            9031,
            Reagent.BatWing,
            Reagent.NoxCrystal
        );

        private static readonly Dictionary<Mobile, DefaultSkillMod> m_Table = new();

        public EvilOmenSpell(Mobile caster, Item scroll = null)
            : base(caster, scroll, m_Info)
        {
        }

        public override TimeSpan CastDelayBase => TimeSpan.FromSeconds(0.75);

        public override double RequiredSkill => 20.0;
        public override int RequiredMana => 11;

        public void Target(Mobile m)
        {
            if (!(m is BaseCreature || m is PlayerMobile))
            {
                Caster.SendLocalizedMessage(1060508); // You can't curse that.
            }
            else if (CheckHSequence(m))
            {
                SpellHelper.Turn(Caster, m);

                /* Curses the target so that the next harmful event that affects them is magnified.
                 * Damage to the target's hit points is increased 25%,
                 * the poison level of the attack will be 1 higher
                 * and the Resist Magic skill of the target will be fixed on 50.
                 *
                 * The effect lasts for one harmful event only.
                 */

                m.Spell?.OnCasterHurt();

                m.PlaySound(0xFC);
                m.FixedParticles(0x3728, 1, 13, 9912, 1150, 7, EffectLayer.Head);
                m.FixedParticles(0x3779, 1, 15, 9502, 67, 7, EffectLayer.Head);

                if (!m_Table.ContainsKey(m))
                {
                    var mod = new DefaultSkillMod(SkillName.MagicResist, false, 50.0);

                    if (m.Skills.MagicResist.Base > 50.0)
                    {
                        m.AddSkillMod(mod);
                    }

                    m_Table[m] = mod;
                }

                var duration = TimeSpan.FromSeconds(Caster.Skills.SpiritSpeak.Value / 12 + 1.0);

                if (!HasReagents())
                {
                    duration *= 0.5;
                }

                Timer.DelayCall(duration, mob => TryEndEffect(mob), m);

                HarmfulSpell(m);

                BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.EvilOmen, 1075647, 1075648, duration, m));
            }

            FinishSequence();
        }

        public override void OnCast()
        {
            Caster.Target = new SpellTargetMobile(this, TargetFlags.Harmful, Core.ML ? 10 : 12);
        }

        public static bool TryEndEffect(Mobile m)
        {
            if (!m_Table.Remove(m, out var mod))
            {
                return false;
            }

            mod?.Remove();

            return true;
        }
    }
}

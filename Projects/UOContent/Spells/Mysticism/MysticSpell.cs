using System;
using Server.Talent;
using Server.Mobiles;

namespace Server.Spells.Mysticism
{
    public abstract class MysticSpell : Spell
    {
        private BaseTalent m_DarkAffinity;
        public BaseTalent DarkAffinity
        {
            get { return m_DarkAffinity; }
            set { m_DarkAffinity = value; }
        }
        private BaseTalent m_NatureAffinity;
        public BaseTalent NatureAffinity
        {
            get { return m_NatureAffinity; }
            set { m_NatureAffinity = value; }
        }
        public MysticSpell(Mobile caster, Item scroll, SpellInfo info)
            : base(caster, scroll, info)
        {
            if (Caster is PlayerMobile player)
            {
                DarkAffinity = player.GetTalent(typeof(DarkAffinity));
                NatureAffinity = player.GetTalent(typeof(NatureAffinity));
            }
        }

        public abstract double RequiredSkill { get; }
        public abstract int RequiredMana { get; }

        public override SkillName CastSkill => SkillName.Mysticism;

        /*
         * As per OSI Publish 64:
         * Imbuing is not the only skill associated with Mysticism now.
         * Players can use EITHER their Focus skill or Imbuing skill.
         * Evaluate Intelligence no longer has any effect on a Mystic’s spell power.
         */
        public override double GetDamageSkill(Mobile m) => Math.Max(m.Skills.Imbuing.Value, m.Skills.Focus.Value);

        public override int GetDamageFixed(Mobile m) => Math.Max(m.Skills.Imbuing.Fixed, m.Skills.Focus.Fixed);

        public override void GetCastSkills(out double min, out double max)
        {
            // As per Mysticism page at the UO Herald Playguide
            // This means that we have 25% success chance at min Required Skill

            min = RequiredSkill - 12.5;
            max = RequiredSkill + 37.5;
        }

        public override int GetMana() => RequiredMana;

        public override bool CheckCast()
        {
            if (!base.CheckCast())
            {
                return false;
            }

            var mana = ScaleMana(RequiredMana);

            if (Caster.Mana < mana)
            {
                Caster.SendLocalizedMessage(
                    1060174,
                    mana.ToString()
                ); // You must have at least ~1_MANA_REQUIREMENT~ Mana to use this ability.
                return false;
            }

            if (Caster.Skills[CastSkill].Value < RequiredSkill)
            {
                Caster.SendLocalizedMessage(
                    1063013,
                    $"{RequiredSkill:F1}\t{CastSkill.ToString()}\t "
                ); // You need at least ~1_SKILL_REQUIREMENT~ ~2_SKILL_NAME~ skill to use that ability.
                return false;
            }

            return true;
        }

        public override void OnBeginCast()
        {
            base.OnBeginCast();

            SendCastEffect();
        }

        public virtual void SendCastEffect()
        {
            Caster.FixedEffect(0x37C4, 10, (int)(GetCastDelay().TotalSeconds * 28), 0x66C, 3);
        }

        public bool CheckDarkAffinity()
        {
            return m_DarkAffinity != null;
        }

        public void DarkAffinityPower(ref int value)
        {
            if (CheckDarkAffinity())
            {
                value += DarkAffinity.Level * 2;
            }
        }

        public bool CheckNatureAffinity()
        {
            return m_NatureAffinity != null;
        }
        public void NatureAffinityPower(ref int value)
        {
            if (CheckNatureAffinity())
            {
                value += NatureAffinity.Level;
            }
        }

        public static double GetBaseSkill(Mobile m) => m.Skills.Mysticism.Value;

        public static double GetBoostSkill(Mobile m) => Math.Max(m.Skills.Imbuing.Value, m.Skills.Focus.Value);
    }
}

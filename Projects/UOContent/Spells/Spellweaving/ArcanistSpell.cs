using Server.Engines.MLQuests;
using Server.Items;
using Server.Mobiles;
using Server.Talent;

namespace Server.Spells.Spellweaving
{
    public abstract class ArcanistSpell : Spell
    {
        private BaseTalent m_LightAffinity;
        public BaseTalent LightAffinity
        {
            get { return m_LightAffinity; }
            set { m_LightAffinity = value; }
        }
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
        private BaseTalent m_FireAffinity;
        public BaseTalent FireAffinity
        {
            get { return m_FireAffinity; }
            set { m_FireAffinity = value; }
        }
        private int m_CastTimeFocusLevel;

        public ArcanistSpell(Mobile caster, Item scroll, SpellInfo info) : base(caster, scroll, info)
        {
            if (Caster is PlayerMobile player)
            {
                LightAffinity = player.GetTalent(typeof(LightAffinity));
                DarkAffinity = player.GetTalent(typeof(DarkAffinity));
                NatureAffinity = player.GetTalent(typeof(NatureAffinity));
                FireAffinity = player.GetTalent(typeof(FireAffinity));
            }
        }

        public abstract double RequiredSkill { get; }
        public abstract int RequiredMana { get; }

        public override SkillName CastSkill => SkillName.Spellweaving;
        public override SkillName DamageSkill => SkillName.Spellweaving;

        public override bool ClearHandsOnCast => false;

        public virtual int FocusLevel => m_CastTimeFocusLevel;

        public bool CheckLightAffinity()
        {
            return m_LightAffinity != null;
        }

        public void LightAffinityPower(ref int value)
        {
            if (CheckLightAffinity())
            {
                value += LightAffinity.Level;
            }
        }

        public bool CheckDarkAffinity()
        {
            return m_DarkAffinity != null;
        }

        public void DarkAffinityPower(ref int value)
        {
            if (CheckDarkAffinity())
            {
                value += DarkAffinity.Level*2;
            }
        }

        public bool CheckFireAffinity()
        {
            return m_FireAffinity != null;
        }

        public void FireAffinityPower(ref int value)
        {
            if (CheckFireAffinity())
            {
                value += FireAffinity.Level;
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
        public static int GetFocusLevel(Mobile from)
        {
            var focus = FindArcaneFocus(from);

            return focus?.Deleted != false ? 0 : focus.StrengthBonus;
        }

        public static ArcaneFocus FindArcaneFocus(Mobile from) =>
            from.Holding as ArcaneFocus ?? from.Backpack?.FindItemByType<ArcaneFocus>();

        public static bool CheckExpansion(Mobile from) =>
            from is not PlayerMobile || from.NetState?.SupportsExpansion(Expansion.ML) == true;

        public override bool CheckCast()
        {
            if (!base.CheckCast())
            {
                return false;
            }

            var caster = Caster;

            if (!CheckExpansion(caster))
            {
                // You must upgrade to the Mondain's Legacy Expansion Pack before using that ability
                caster.SendLocalizedMessage(1072176);
                return false;
            }

            if (caster is PlayerMobile mobile)
            {
                var context = MLQuestSystem.GetContext(mobile);

                if (context?.Spellweaving != true)
                {
                    // You must have completed the epic arcanist quest to use this ability.
                    mobile.SendLocalizedMessage(1073220);
                    return false;
                }
            }

            var mana = ScaleMana(RequiredMana);

            if (caster.Mana < mana)
            {
                // You must have at least ~1_MANA_REQUIREMENT~ Mana to use this ability.
                caster.SendLocalizedMessage(1060174, mana.ToString());
                return false;
            }

            if (caster.Skills[CastSkill].Value < RequiredSkill)
            {
                // You need at least ~1_SKILL_REQUIREMENT~ ~2_SKILL_NAME~ skill to use that ability.
                caster.SendLocalizedMessage(1063013, $"{RequiredSkill:F1}\t{"#1044114"}");
                return false;
            }

            return true;
        }

        public override void GetCastSkills(out double min, out double max)
        {
            min = RequiredSkill - 12.5; // per 5 on Friday, 2/16/07
            max = RequiredSkill + 37.5;
        }

        public override int GetMana() => RequiredMana;

        public override void DoFizzle()
        {
            Caster.PlaySound(0x1D6);
            Caster.NextSpellTime = Core.TickCount;
        }

        public override void DoHurtFizzle()
        {
            Caster.PlaySound(0x1D6);
        }

        public override void OnDisturb(DisturbType type, bool message)
        {
            base.OnDisturb(type, message);

            if (message)
            {
                Caster.PlaySound(0x1D6);
            }
        }

        public override void OnBeginCast()
        {
            base.OnBeginCast();

            SendCastEffect();
            m_CastTimeFocusLevel = GetFocusLevel(Caster);
        }

        public virtual void SendCastEffect()
        {
            Caster.FixedEffect(0x37C4, 10, (int)(GetCastDelay().TotalSeconds * 28), 4, 3);
        }

        public virtual bool CheckResisted(Mobile m)
        {
            var percent =
                (50 + 2 * (GetResistSkill(m) - GetDamageSkill(Caster))) /
                100; // TODO: According to the guide this is it.. but.. is it correct per OSI?

            return percent switch
            {
                <= 0   => false,
                >= 1.0 => true,
                _      => percent >= Utility.RandomDouble()
            };
        }
    }
}

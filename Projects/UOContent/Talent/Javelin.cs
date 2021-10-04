using Server.Mobiles;
using Server.Items;
using Server.Targeting;
using System;

namespace Server.Talent
{
    public class Javelin : BaseTalent, ITalent
    {
        public Javelin() : base()
        {
            RequiredWeaponSkill = SkillName.Fencing;
            RequiredWeapon = new Type[] { typeof(BaseSpear) };
            TalentDependency = typeof(SpearSpecialist);
            DisplayName = "Javelin";
            CanBeUsed = true;
            Description = "Throw copy of equipped spear at target. 45s min cooldown.";
            ImageID = 370;
            GumpHeight = 75;
            AddEndY = 65;
            MaxLevel = 10;
        }

        public override void OnUse(Mobile mobile)
        {
            BaseWeapon weapon = mobile.Weapon as BaseWeapon;
            if (!OnCooldown && weapon is BaseSpear)
            {
                mobile.SendMessage("Whom do you wish to throw a spear at?");
                mobile.Target = new InternalTarget(mobile, this);
            } else
            {
                mobile.SendMessage("You do not have a spear equipped.");
            }
        }
        private class InternalTarget : Target
        {
            public TimerExecutionToken _hitTimerToken;
            private Mobile m_Mobile;
            private Mobile m_Target;
            private Javelin m_Javelin;
            public InternalTarget(Mobile mobile, Javelin javelin) : base(
                8,
                false,
                TargetFlags.None
            )
            {
                m_Mobile = mobile;
                m_Javelin = javelin;
            }

            public void CheckHit()
            {
                if (m_Mobile.Weapon is BaseSpear spear && spear.CheckHit(m_Mobile, m_Target))
                {
                    spear.OnHit(m_Mobile, m_Target);
                } else
                {
                    m_Javelin.ExpireTalentCooldown();
                    m_Javelin._talentTimerToken.Cancel();
                }
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted != null)
                {
                    if (targeted == m_Mobile || !((Mobile)targeted).CanBeHarmful(from, false) ||
              Core.AOS && !((Mobile)targeted).InLOS(from))
                    {
                        from.SendMessage("Thou cannot throw a spear at this target");
                        return;
                    } else if (targeted is Mobile target)
                    {
                        bool targetInRange = false;
                        foreach(Mobile mobile in from.GetMobilesInRange(m_Javelin.Level))
                        {
                            if (mobile == target) {
                                targetInRange = true;
                                break;
                            }
                        }
                        if (targetInRange)
                        {
                            m_Target = target;
                            from.RevealingAction();
                            m_Javelin.OnCooldown = true;
                            Timer.StartTimer(TimeSpan.FromSeconds(45), m_Javelin.ExpireTalentCooldown, out m_Javelin._talentTimerToken);
                            Effects.SendMovingEffect(from, target, 0x1BFE, 18, 2, false, false, 0x3B5);
                            Timer.StartTimer(TimeSpan.FromSeconds(2), CheckHit, out _hitTimerToken);
                        } else
                        {
                            from.SendMessage("Your target is too far away.");
                        }
                       
                    }
                }
            }
        }
    }
}

using Server.Mobiles;
using Server.Misc;
using Server.Spells;
using Server.Spells.Fourth;
using System;
using System.Collections.Generic;

namespace Server.Talent
{
    public class DryThunderstorm : BaseTalent, ITalent
    {
        private Mobile m_Mobile;

        private int m_RemainingBolts;
        public int RemainingBolts
        {
            get
            {
                return m_RemainingBolts;
            }
            set
            {
                m_RemainingBolts = value;
            }
        }
        public DryThunderstorm() : base()
        {
            TalentDependency = typeof(SpellMind);
            DisplayName = "Thunderstorm";
            CanBeUsed = true;
            Description = "Conjures up a storm that will strike lightning (Level * 5) times to nearby enemies. 2 min cooldown.";
            ImageID = 371;
        }
        public override void OnUse(Mobile mobile)
        {
            if (mobile.Mana < 40)
            {
                mobile.SendMessage("You require 40 mana to conjure a storm.");
            }
            else if (!Activated && !OnCooldown)
            {
                m_Mobile = mobile;
                Activated = true;
                mobile.Mana -= 40;
                RemainingBolts = (Level * 5) + Utility.Random(1, Level);
                Timer.StartTimer(TimeSpan.FromSeconds(Utility.Random(7,10)), CheckStorm, out _talentTimerToken);
                mobile.PlaySound(0x5CE);
            }
        }
        public void CheckStorm()
        {
            if (RemainingBolts > 0)
            {
                List<Mobile> mobiles = (List<Mobile>)m_Mobile.GetMobilesInRange(8);
                foreach (Mobile mobile in mobiles)
                {
                    if (mobile == m_Mobile || (mobile is PlayerMobile && mobile.Karma > 0) || !mobile.CanBeHarmful(m_Mobile, false) ||
                            Core.AOS && !mobile.InLOS(m_Mobile))
                    {
                        continue;
                    }
                    double damage;
                    LightningSpell lightning = new LightningSpell(m_Mobile);
                    if (Core.AOS)
                    {
                        damage = lightning.GetNewAosDamage(23, 1, 4, mobile);
                    }
                    else
                    {
                        damage = Utility.Random(12, 9);

                        if (lightning.CheckResisted(mobile))
                        {
                            damage *= 0.75;
                            mobile.SendLocalizedMessage(501783); // You feel yourself resisting magical energy.
                        }
                        damage *= lightning.GetDamageScalar(mobile);
                    }
                    mobile.BoltEffect(0);
                    SpellHelper.Damage(lightning, mobile, damage);
                    RemainingBolts--;
                    break;
                }
                Timer.StartTimer(TimeSpan.FromSeconds(Utility.Random(7, 10)), CheckStorm, out _talentTimerToken);
            }
            else
            {
                Activated = false;
                OnCooldown = true;
                Timer.StartTimer(TimeSpan.FromSeconds(120), ExpireTalentCooldown, out _talentTimerToken);
            }
        }
    }
}


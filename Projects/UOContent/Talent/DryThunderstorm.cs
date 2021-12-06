using System;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Fourth;

namespace Server.Talent
{
    public class DryThunderstorm : BaseTalent
    {
        private Mobile m_Mobile;

        public DryThunderstorm()
        {
            TalentDependency = typeof(SpellMind);
            DisplayName = "Thunderstorm";
            CanBeUsed = true;
            Description =
                "Conjures up a storm that will strike lightning (Level * 5) times to nearby enemies. 2 min cooldown.";
            ImageID = 371;
        }

        public int RemainingBolts { get; set; }

        public override void OnUse(Mobile from)
        {
            if (from.Mana < 60)
            {
                from.SendMessage("You require 60 mana to conjure a storm.");
            }
            else if (!Activated && !OnCooldown)
            {
                m_Mobile = from;
                Activated = true;
                from.Mana -= 60;
                RemainingBolts = Level * 5 + Utility.Random(1, Level);
                Timer.StartTimer(TimeSpan.FromSeconds(Utility.Random(7, 10)), CheckStorm, out _talentTimerToken);
                from.PlaySound(0x5CE);
            }
        }

        public void CheckStorm()
        {
            if (RemainingBolts > 0)
            {
                foreach (var mobile in m_Mobile.GetMobilesInRange(8))
                {
                    if (mobile == m_Mobile || mobile is PlayerMobile && mobile.Karma > 0 ||
                        !mobile.CanBeHarmful(m_Mobile, false) ||
                        Core.AOS && !mobile.InLOS(m_Mobile))
                    {
                        continue;
                    }

                    double damage;
                    var lightning = new LightningSpell(m_Mobile);
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
                    m_Mobile.DoHarmful(mobile);
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

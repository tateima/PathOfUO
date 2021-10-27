using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Talent
{
    public class IceBolt : BaseTalent, ITalent
    {
        public TimerExecutionToken _slowTimerToken;
        private BaseCreature m_SlowedCreature;
        public IceBolt() : base()
        {
            TalentDependency = typeof(CrossbowSpecialist);
            RequiredWeapon = new Type[] { typeof(Crossbow), typeof(HeavyCrossbow), typeof(RepeatingCrossbow) };
            RequiredWeaponSkill = SkillName.Archery;
            CanBeUsed = true;
            DisplayName = "Ice bolt";
            Description = "Fire bolt of ice from weapon that slows target and does minor cold damage.";
            ImageID = 378;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public void IceDamage(Mobile attacker, Mobile target, int damage, int modifier)
        {
            if (Core.AOS)
            {
                AOS.Damage(target, AOS.Scale(damage, Level * modifier), 0, 0, 100, 0, 0);
            }
            else
            {
                target.Damage(AOS.Scale(damage, Level * modifier), attacker);
            }
        }

        public void ExpireIceEffect()
        {
            m_SlowedCreature.ActiveSpeed *= 2;
        }
        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (Activated)
            {
                if (target is BaseCreature creature)
                {
                    creature.ActiveSpeed /= 2;
                    m_SlowedCreature = creature;
                    Timer.StartTimer(TimeSpan.FromSeconds(Level * 5), ExpireIceEffect, out _talentTimerToken);
                    IceDamage(attacker, target, damage, 1);
                }
                if (target is PlayerMobile player)
                {
                    IceDamage(attacker, target, damage, 2);
                    player.Slow(Level);
                }
                if (target is PlayerMobile || target is BaseCreature)
                {
                    Timer.StartTimer(TimeSpan.FromSeconds(120), ExpireTalentCooldown, out _talentTimerToken);
                }
            }
        }
    }
}

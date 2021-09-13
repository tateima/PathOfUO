using Server.Mobiles;
using Server.Spells;
using System;

namespace Server.Talent
{
    public class WyvernAspect : BaseTalent, ITalent
    {
        public TimerExecutionToken _activatedTimerToken;
        public WyvernAspect() : base()
        {
            TalentDependency = typeof(VenomBlood);
            DisplayName = "Wyvern Aspect";
            CanBeUsed = true;
            Description = "Poison damage suffered by user also damages between 1-7 surrounding enemies for 5-35 seconds. 2 min cooldown";
            ImageID = 375;
            MaxLevel = 7;
            GumpHeight = 75;
            AddEndY = 105;
        }

        public override void OnUse(Mobile mobile)
        {
            if (!OnCooldown && !Activated)
            {
                OnCooldown = true;
                Activated = true;
                Timer.StartTimer(TimeSpan.FromSeconds(Level*5), ExpireActivated, out _activatedTimerToken);
                Timer.StartTimer(TimeSpan.FromSeconds(120), ExpireTalentCooldown, out _talentTimerToken);
            }
        }

        public void ExpireActivated()
        {
            Activated = false;
        }
        public override void ExpireTalentCooldown()
        {
            OnCooldown = false;
        }
    }
}

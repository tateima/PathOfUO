using System;

namespace Server.Talent
{
    public class WyvernAspect : BaseTalent
    {
        public WyvernAspect()
        {
            TalentDependencies = new[] { typeof(VenomBlood) };
            DisplayName = "Wyvern Aspect";
            CanBeUsed = true;
            Description =
                "Poison damage suffered by user also damages between 1-7 surrounding enemies for 5-35 seconds.";
            AdditionalDetail = "Wyvern aspect receives bonuses from viper aspect level.";
            CooldownSeconds = 120;
            ManaRequired = 25;
            ImageID = 375;
            MaxLevel = 7;
            GumpHeight = 75;
            AddEndY = 105;
        }

        public override bool HasSkillRequirement(Mobile mobile) => mobile.Skills.Poisoning.Base >= 70;

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown && !Activated && from.Mana > ManaRequired && HasSkillRequirement(from))
            {
                ApplyManaCost(from);
                OnCooldown = true;
                Activated = true;
                Timer.StartTimer(TimeSpan.FromSeconds(Level * 5), ExpireActivated, out _);
                Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
            }
            else
            {
                from.SendMessage(FailedRequirements);
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

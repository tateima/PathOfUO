using System;
using Server.Items;

namespace Server.Talent
{
    public class ThingAMaBob : BaseTalent
    {
        public ThingAMaBob()
        {
            TalentDependencies = new[] { typeof(MerchantPorter) };
            DisplayName = "Thing-a-ma-bob";
            CanBeUsed = true;
            Description = "Create device that casts random spells at targets. Can glitch and heal them instead.";
            CooldownSeconds = 3600;
            ImageID = 177;
            GumpHeight = 230;
            AddEndY = 90;
            MaxLevel = 1;
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown && HasSkillRequirement(from))
            {
                var current = @from.Backpack?.FindItemByType<ThingAMaBobDevice>() ?? @from.BankBox?.FindItemByType<ThingAMaBobDevice>();
                if (current != null)
                {
                    from.SendMessage("You already have a Thing-a-ma-bob");
                }
                else
                {
                    OnCooldown = true;
                    var device = new ThingAMaBobDevice();
                    from.AddToBackpack(device);
                    Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
                }
            }
            else
            {
                from.SendMessage(FailedRequirements);
            }
        }
    }
}

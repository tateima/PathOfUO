using System;
using Server.Items;

namespace Server.Talent
{
    public class WhizzyGig : BaseTalent
    {
        public WhizzyGig()
        {
            TalentDependency = typeof(MerchantPorter);
            DisplayName = "Whizzy-gig";
            CanBeUsed = true;
            Description = "Create device that heals targets. Can glitch and damage instead.";
            ImageID = 173;
            GumpHeight = 230;
            AddEndY = 75;
            MaxLevel = 1;
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown)
            {
                var current = @from.Backpack?.FindItemByType<WhizzyGigDevice>() ?? @from.BankBox?.FindItemByType<WhizzyGigDevice>();
                if (current != null)
                {
                    from.SendMessage("You already have a Whizzy-gig");
                }
                else
                {
                    OnCooldown = true;
                    var device = new WhizzyGigDevice();
                    from.AddToBackpack(device);
                    Timer.StartTimer(TimeSpan.FromMinutes(60), ExpireTalentCooldown, out _talentTimerToken);
                }
            }
        }
    }
}

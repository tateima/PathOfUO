using Server.Mobiles;
using Server.Spells;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System;

namespace Server.Talent
{
    public class WhizzyGig : BaseTalent, ITalent
    {
        public WhizzyGig() : base()
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

        public override void OnUse(Mobile mobile)
        {
            if (!OnCooldown)
            {
                OnCooldown = true;
                WhizzyGigDevice device = new WhizzyGigDevice();
                mobile.AddToBackpack(device);
                Timer.StartTimer(TimeSpan.FromMinutes(60), ExpireTalentCooldown, out _talentTimerToken);
            }
        }
    }
}

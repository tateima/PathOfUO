using Server.Mobiles;
using Server.Spells;
using Server.Gumps;
using Server.Network;
using Server.Talent.Devices;
using System;

namespace Server.Talent
{
    public class ThingAMaBob : BaseTalent, ITalent
    {
        public ThingAMaBob() : base()
        {
            TalentDependency = typeof(MerchantPorter);
            DisplayName = "Thing-a-ma-bob";
            CanBeUsed = true;
            Description = "Create device that casts random spells at targets. Can glitch and heal them instead.";
            ImageID = 177;
            GumpHeight = 230;
            AddEndY = 90;
            MaxLevel = 1;
        }

        public override void OnUse(Mobile mobile)
        {
            if (!OnCooldown)
            {
                OnCooldown = true;
                ThingAMaBobDevice device = new ThingAMaBobDevice();
                mobile.AddToBackpack(device);
                Timer.StartTimer(TimeSpan.FromMinutes(60), ExpireTalentCooldown, out _talentTimerToken);
            }
        }
    }
}

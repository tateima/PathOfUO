using Server.Mobiles;
using Server.Spells;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System;

namespace Server.Talent
{
    public class Telewarper : BaseTalent, ITalent
    {
        public Telewarper() : base()
        {
            TalentDependency = typeof(MerchantPorter);
            DisplayName = "Telewarper disc";
            CanBeUsed = true;
            Description = "Create device that teleports user. Can glitch and summon creatures.";
            ImageID = 169;
            GumpHeight = 230;
            AddEndY = 90;
            MaxLevel = 1;
        }

        public override void OnUse(Mobile mobile)
        {
            if (!OnCooldown)
            {
                OnCooldown = true;
                TelewarperDevice device = new TelewarperDevice();
                mobile.AddToBackpack(device);
                Timer.StartTimer(TimeSpan.FromMinutes(60), ExpireTalentCooldown, out _talentTimerToken);
            }
        }
    }
}

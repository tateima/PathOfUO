using Server.Mobiles;
using Server.Spells;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System;

namespace Server.Talent
{
    public class Polymeter : BaseTalent, ITalent
    {
        public Polymeter() : base()
        {
            TalentDependency = typeof(MerchantPorter);
            DisplayName = "Poly gadget";
            CanBeUsed = true;
            Description = "Create device that polymorphs creatures into rabbits. Can glitch and make them stronger.";
            ImageID = 156;
            GumpHeight = 230;
            AddEndY = 105;
            MaxLevel = 1;
        }

        public override void OnUse(Mobile mobile)
        {
            if (!OnCooldown)
            {
                OnCooldown = true;
                PolymeterDevice device = new PolymeterDevice();
                mobile.AddToBackpack(device);
                Timer.StartTimer(TimeSpan.FromMinutes(60), ExpireTalentCooldown, out _talentTimerToken);
            }
        }
    }
}

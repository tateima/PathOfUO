using System;
using Server.Items;

namespace Server.Talent
{
    public class Telewarper : BaseTalent
    {
        public Telewarper()
        {
            TalentDependency = typeof(MerchantPorter);
            DisplayName = "Telewarper disc";
            CanBeUsed = true;
            Description = "Create device that teleports user. Can glitch and summon creatures.";
            CooldownSeconds = 3600;
            ImageID = 169;
            GumpHeight = 230;
            AddEndY = 90;
            MaxLevel = 1;
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown)
            {
                var current = @from.Backpack?.FindItemByType<TelewarperDevice>() ?? @from.BankBox?.FindItemByType<TelewarperDevice>();
                if (current != null)
                {
                    from.SendMessage("You already have a Telewarper disc");
                }
                else
                {
                    OnCooldown = true;
                    var device = new TelewarperDevice();
                    from.AddToBackpack(device);
                    Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
                }
            }
        }
    }
}

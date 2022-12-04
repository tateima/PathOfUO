using System;
using Server.Items;

namespace Server.Talent
{
    public class Polymeter : BaseTalent
    {
        public Polymeter()
        {
            TalentDependency = typeof(MerchantPorter);
            DisplayName = "Poly gadget";
            CanBeUsed = true;
            Description = "Create device that polymorphs creatures into rabbits. Can glitch and make them stronger.";
            CooldownSeconds = 3600;
            ImageID = 156;
            GumpHeight = 230;
            AddEndY = 105;
            MaxLevel = 1;
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown && HasSkillRequirement(from))
            {
                var current = @from.Backpack?.FindItemByType<PolymeterDevice>() ?? @from.BankBox?.FindItemByType<PolymeterDevice>();
                if (current != null)
                {
                    from.SendMessage("You already have a Poly gadget");
                }
                else
                {
                    OnCooldown = true;
                    var device = new PolymeterDevice();
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

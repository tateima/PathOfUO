using System;
using Server.Items;
using Server.Mobiles;
using Server.Pantheon;

namespace Server.Talent
{
    public class Deception : BaseTalent
    {
        public Deception()
        {
            DeityAlignment = Deity.Alignment.Greed;
            RequiresDeityFavor = true;
            CanBeUsed = true;
            DisplayName = "Deception";
            Description = "Turns user invisible, allowing them to move hidden for 1 minute.";
            StamRequired = 30;
            CooldownSeconds = 600;
            ImageID = 174;
            GumpHeight = 75;
            AddEndY = 100;
        }
        public override void OnUse(Mobile from)
        {
            if (!OnCooldown)
            {
                if (from.Stam > StamRequired + 1)
                {
                    ApplyStaminaCost(from);
                    OnCooldown = true;
                    Activated = true;
                    from.Hidden = true;
                    Timer.StartTimer(TimeSpan.FromSeconds(60), ExpireBuff);
                    Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
                }
                else
                {
                    from.SendMessage($"You need {StamRequired.ToString()} stamina to gain {DisplayName} for 2 minutes.");
                }
            }
        }

        private void ExpireBuff() => Activated = false;
    }
}

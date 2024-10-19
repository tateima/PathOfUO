using System;
using Server.Pantheon;

namespace Server.Talent
{
    public class Heroism : BaseTalent
    {
        public Heroism()
        {
            DisplayName = "Heroism";
            DeityAlignment = Deity.Alignment.Order;
            RequiresDeityFavor = true;
            CanBeUsed = true;
            Description =
                "Cannot be frozen, feared, paralysed or blinded for 60 seconds.";
            AdditionalDetail = "The duration of this effect increases by 5 seconds per level. Each save gives you a minor stamina boost.";
            ImageID = 418;
            CooldownSeconds = 120;
            StamRequired = 35;
            GumpHeight = 230;
            AddEndY = 70;
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown && HasSkillRequirement(from))
            {
                if (from.Stam > StamRequired + 1)
                {
                    ApplyStaminaCost(from);
                    from.SendSound(from.Female ? 0x31C : 0x431);
                    Activated = true;
                    OnCooldown = true;
                    Timer.StartTimer(TimeSpan.FromSeconds(60 + Level * 5), ExpireBuff);
                    Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
                }
                else
                {
                    from.SendMessage($"You need {StamRequired.ToString()} stamina to use {DisplayName}.");
                }
            }
            else
            {
                from.SendMessage(FailedRequirements);
            }
        }

        public bool CheckSave(Mobile from)
        {
            if (Activated)
            {
                from.Stam += Utility.RandomMinMax(1, 10);
            }
            return Activated;
        }

        public void ExpireBuff()
        {
            Activated = false;
        }
    }
}

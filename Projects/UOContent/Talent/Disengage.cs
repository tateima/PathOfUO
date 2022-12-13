using System;

namespace Server.Talent
{
    public class Disengage : BaseTalent
    {
        public Disengage()
        {
            TalentDependencies = new[] { typeof(ArcherFocus) };
            CanBeUsed = true;
            DisplayName = "Disengage";
            StamRequired = 15;
            CooldownSeconds = 120;
            Description = "Leaps backwards from enemies.";
            AdditionalDetail = "The distance ranges between two and eight yards.";
            ImageID = 380;
            GumpHeight = 85;
            AddEndY = 85;
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown && from.Stam >= StamRequired && HasSkillRequirement(from))
            {
                var attackerPosition = from.Location;
                if (from.Direction != Direction.Running)
                {
                    var distance = Level + Utility.Random(1, 3);
                    var newLocation = CalculatePushbackFromAnchor(attackerPosition, distance, from);
                    while (!from.InLOS(newLocation))
                    {
                        newLocation = CalculatePushbackFromAnchor(attackerPosition, 1, from);
                    }
                    from.MoveToWorld(newLocation, from.Map);
                    ApplyStaminaCost(from);
                    OnCooldown = true;
                    from.PlaySound(0x525);
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

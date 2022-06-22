using System;

namespace Server.Talent
{
    public class EscapeDeath : BaseTalent
    {
        public EscapeDeath()
        {
            TalentDependency = typeof(KeenSenses);
            HasBeforeDeathSave = true;
            DisplayName = "Escape death";
            CooldownSeconds = 300;
            Description = "Avoid a deathly blow and be healed.";
            AdditionalDetail = $"Each level increases the healing and stamina restoration by 10 points. {AdditionalDetail}";
            ImageID = 150;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override void CheckBeforeDeathEffect(Mobile target)
        {
            if (!OnCooldown)
            {
                target.SendSound(0x200);
                OnCooldown = true;
                target.Hits = Level * 10;
                target.Stam = Level * 10;
                target.FixedEffect(0x37B9, 10, 16);
                Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
            }
        }
    }
}

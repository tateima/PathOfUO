using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class EscapeDeath : BaseTalent, ITalent
    {
        public EscapeDeath() : base()
        {
            TalentDependency = typeof(KeenSenses);
            HasBeforeDeathSave = true;
            DisplayName = "Escape death";
            Description = "Avoid a deathly blow. 5 minute cooldown.";
            ImageID = 39877;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override void CheckBeforeDeathEffect(Mobile target)
        {
            if (!OnCooldown)
            {
                OnCooldown = true;
                target.Hits = Level * 5;
                target.FixedEffect(0x37B9, 10, 16);
                Timer.StartTimer(TimeSpan.FromSeconds(300), ExpireTalentCooldown, out _talentTimerToken);
            };
        }

    }
}

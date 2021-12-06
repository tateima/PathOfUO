using System;

namespace Server.Talent
{
    public class BoneBreaker : BaseTalent
    {
        public BoneBreaker()
        {
            TalentDependency = typeof(IronSkin);
            DisplayName = "Bone breaker";
            CanBeUsed = true;
            Description = "Next hit paralyzes target for 3s per level. Level also reduces cooldown by 10s. 2m cooldown";
            ImageID = 134;
            GumpHeight = 75;
            AddEndY = 100;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (Activated)
            {
                Activated = false;
                OnCooldown = true;
                attacker.SendSound(0x125);
                target.Paralyze(TimeSpan.FromSeconds(Level * 3));
                Timer.StartTimer(TimeSpan.FromSeconds(120 - Level * 10), ExpireTalentCooldown, out _talentTimerToken);
            }
        }
    }
}

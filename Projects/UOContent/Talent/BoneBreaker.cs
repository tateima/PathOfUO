using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class BoneBreaker : BaseTalent, ITalent
    {
        public BoneBreaker() : base()
        {
            TalentDependency = typeof(IronSkin);
            DisplayName = "Bone breaker";
            CanBeUsed = true;
            Description = "Next hit paralyzes target for 1s per level. Level also reduces cooldown by 5s. 5m cooldown";
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
                target.Paralyze(TimeSpan.FromSeconds(Level * 2));
                Timer.StartTimer(TimeSpan.FromSeconds(60-(Level*5)), ExpireTalentCooldown, out _talentTimerToken);
            }
        }
    }
}

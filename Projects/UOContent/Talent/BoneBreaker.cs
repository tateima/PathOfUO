using System;
using Server.Items;

namespace Server.Talent
{
    public class BoneBreaker : BaseTalent
    {
        public BoneBreaker()
        {
            RequiredWeapon = new[] { typeof(BaseWeapon) };
            TalentDependencies = new[] { typeof(IronSkin) };
            DisplayName = "Bone breaker";
            CanBeUsed = true;
            Description = "Next hit paralyzes target for 3s per level. Level also reduces cooldown by 10s.";
            StamRequired = 6;
            CooldownSeconds = 80;
            ImageID = 134;
            GumpHeight = 75;
            AddEndY = 100;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
            if (Activated && attacker.Stam > StamRequired + 1)
            {
                Activated = false;
                OnCooldown = true;
                ApplyStaminaCost(attacker);
                target.PlaySound(0x125);
                target.Paralyze(TimeSpan.FromSeconds(Level * 3));
                Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds - Level * 10), ExpireTalentCooldown, out _talentTimerToken);
            }
        }
    }
}

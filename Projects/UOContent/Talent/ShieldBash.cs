using System;
using Server.Items;

namespace Server.Talent
{
    public class ShieldBash : BaseTalent
    {
        public ShieldBash()
        {
            RequiredWeapon = new[] { typeof(BaseShield) };
            CanBeUsed = true;
            TalentDependency = typeof(ShieldFocus);
            DisplayName = "Shield bash";
            Description = "Stun target if hits for 2 second per level. 30 second cooldown.";
            ImageID = 351;
            GumpHeight = 75;
            AddEndY = 90;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (Activated && attacker.Weapon is BaseWeapon weapon && attacker.FindItemOnLayer(Layer.TwoHanded) is BaseShield)
            {
                if (weapon.CheckHit(attacker, target))
                {
                    Activated = false;
                    OnCooldown = true;
                    target.SendSound(0x140);
                    target.FixedEffect(0x37B9, 10, 16);
                    target.Paralyze(TimeSpan.FromSeconds(Level * 2));
                    Timer.StartTimer(TimeSpan.FromSeconds(30), ExpireTalentCooldown, out _talentTimerToken);
                }
            }
        }
    }
}

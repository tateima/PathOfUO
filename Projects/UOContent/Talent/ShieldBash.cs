using Server.Items;
using System;
namespace Server.Talent
{
    public class ShieldBash : BaseTalent, ITalent
    {
        public ShieldBash() : base()
        {
            RequiredWeapon = new Type[] { typeof(BaseShield) };
            CanBeUsed = true;
            TalentDependency = typeof(ShieldFocus);
            DisplayName = "Shield bash";
            Description = "Stun target if hits for 2 second per level. 30 second cooldown.";
            ImageID = 39889;
        }
        public override void CheckHitEffect(Mobile defender, Mobile attacker, int damage)
        {
            if (Activated && defender.Weapon != null && defender.Weapon is BaseWeapon weapon)
            {
                if (weapon.CheckHit(attacker, defender))
                {
                    Activated = false;
                    OnCooldown = true;
                    attacker.SendSound(0x140);
                    attacker.FixedEffect(0x37B9, 10, 16);
                    attacker.Paralyze(TimeSpan.FromSeconds(Level * 2));
                    Timer.StartTimer(TimeSpan.FromSeconds(30), ExpireTalentCooldown, out _talentTimerToken);
                }
            }
        }
    }
}

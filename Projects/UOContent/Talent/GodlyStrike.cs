using System;
using Server.Items;
using Server.Pantheon;

namespace Server.Talent
{
    public class GodlyStrike : BaseTalent
    {
        public GodlyStrike()
        {
            RequiredWeapon = new[] { typeof(BaseWeapon) };
            DisplayName = "Godly strike";
            DeityAlignment = Deity.Alignment.Order;
            RequiresDeityFavor = true;
            CanBeUsed = true;
            Description = "Next hit paralyzes target for 5s per level and is a guaranteed critical strike.";
            AdditionalDetail = "Each level decreases the cooldown by 15 seconds";
            StamRequired = 20;
            CooldownSeconds = 150;
            ImageID = 419;
            GumpHeight = 75;
            AddEndY = 100;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (Activated && attacker.Stam > StamRequired + 1)
            {
                Activated = false;
                OnCooldown = true;
                ApplyStaminaCost(attacker);
                target.PlaySound(0x206);
                target.Paralyze(TimeSpan.FromSeconds(Level * 5));
                CriticalStrike(attacker, target, damage);
                Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds - Level * 10), ExpireTalentCooldown, out _talentTimerToken);
            }
        }
    }
}

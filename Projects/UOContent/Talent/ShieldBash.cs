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
            HasDefenseEffect = true;
            TalentDependency = typeof(ShieldFocus);
            DisplayName = "Shield bash";
            CooldownSeconds = 15;
            Description = "Stun target for 2 second per level. 15 second cooldown.";
            AdditionalDetail =
                "This talent is a defensive one and is triggered on a successful counter attack while active. This talent causes 1-X+ damage where X is the talent level.";
            ImageID = 351;
            GumpHeight = 75;
            AddEndY = 90;
        }

        public override bool HasSkillRequirement(Mobile mobile) => mobile.Skills.Parry.Value >= 35.0;

        public override void CheckDefenseEffect(Mobile defender, Mobile attacker, int damage)
        {
            if (Activated && defender.Weapon is BaseWeapon weapon && defender.FindItemOnLayer(Layer.TwoHanded) is BaseShield && defender.Stam >= 11)
            {
                if (weapon.CheckHit(defender, attacker))
                {
                    ApplyStaminaCost(defender);
                    Activated = false;
                    OnCooldown = true;
                    defender.SendSound(0x140);
                    attacker.FixedEffect(0x37B9, 10, 16);
                    attacker.Paralyze(TimeSpan.FromSeconds(Level * 2));
                    attacker.Damage(Utility.Random(Level), defender);
                    Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
                }
            }
        }
    }
}

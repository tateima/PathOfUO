using System;
using Server.Items;

namespace Server.Talent
{
    public class HolyAvenger : BaseTalent
    {
        public HolyAvenger()
        {
            BlockedBy = new[] { typeof(GreaterFireElemental) };
            TalentDependency = typeof(GuardianLight);
            HasDefenseEffect = true;
            DisplayName = "Holy avenger";
            CooldownSeconds = 7;
            Description =
                "Increased damage to holy spells, adds reflective and area of affect damage in combat. Requires 75+ chivalry.";
            AdditionalDetail = "Each level increases reflective damage by 3% and decreases cooldown by 1 second. This talent will also slightly increase attack speed and damage done to unholy creatures.";
            AddEndAdditionalDetailsY = 100;
            ImageID = 293;
            AddEndY = 120;
        }

        public override bool HasSkillRequirement(Mobile mobile) => mobile.Skills[SkillName.Chivalry].Base >= 75;

        public override void CheckDefenseEffect(Mobile defender, Mobile target, int damage)
        {
            if (!OnCooldown)
            {
                OnCooldown = true;
                var modifier = Level * 3;
                // reflect 3% attacker damage per level back to them
                var reflected = AOS.Scale(damage, modifier);
                if (reflected < 1)
                {
                    reflected = 1;
                }
                target.Damage(reflected, defender);
                defender.PlaySound(0x213);
                Effects.SendLocationParticles(
                    EffectItem.Create(
                        new Point3D(target.X, target.Y, target.Z),
                        target.Map,
                        EffectItem.DefaultDuration
                    ),
                    0x37C4,
                    1,
                    29,
                    0x47D,
                    2,
                    9502,
                    0
                );
                Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds - Level), ExpireTalentCooldown, out _talentTimerToken);
            }
        }
    }
}

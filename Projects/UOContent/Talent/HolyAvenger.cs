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
            Description =
                "Increased damage to holy spells, adds reflective and area of affect damage in combat. Requires 75+ chivalry.";
            ImageID = 293;
            AddEndY = 120;
        }

        public override bool HasSkillRequirement(Mobile mobile) => mobile.Skills[SkillName.Chivalry].Base >= 75;

        public override void CheckDefenseEffect(Mobile defender, Mobile target, int damage)
        {
            if (!OnCooldown)
            {
                OnCooldown = true;
                var modifier = Level * 2;
                // reflect 2% attacker damage per level back to them
                target.Damage(AOS.Scale(damage, modifier));
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
                Timer.StartTimer(TimeSpan.FromSeconds((double)15 - Level), ExpireTalentCooldown, out _talentTimerToken);
            }
        }
    }
}

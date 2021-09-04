using Server.Mobiles;
using Server.Items;
using Server.Gumps;
using System;

namespace Server.Talent
{
    public class HolyAvenger : BaseTalent, ITalent
    {
        public HolyAvenger() : base()
        {
            BlockedBy = new Type[] { typeof(GreaterFireElemental) };
            TalentDependency = typeof(GuardianLight);
            HasDefenseEffect = true;
            DisplayName = "Holy avenger";
            Description = "Increased damage to holy spells, adds reflective and area of affect damage in combat.";
            ImageID = 30016;
        }
        public override bool HasSkillRequirement(Mobile mobile)
        {
            return mobile.Skills[SkillName.Chivalry].Base >= 75;
        }
        public override void CheckDefenseEffect(Mobile defender, Mobile attacker, int damage)
        {
            if (!OnCooldown)
            {
                OnCooldown = true;
                int modifier = (Level * 2);
                // reflect 2% attacker damage per level back to them
                attacker.Damage(AOS.Scale(damage, modifier));
                defender.PlaySound(0x213);
                Effects.SendLocationParticles(
                        EffectItem.Create(new Point3D(attacker.X, attacker.Y, attacker.Z), attacker.Map, EffectItem.DefaultDuration),
                        0x37C4,
                        1,
                        29,
                        0x47D,
                        2,
                        9502,
                        0
                    );
                Timer.StartTimer(TimeSpan.FromSeconds((double)15-Level), ExpireTalentCooldown, out _talentTimerToken);
            }
            
        }
    }
}

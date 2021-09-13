using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class ElementalHunter : BaseTalent, ITalent
    {
        public ElementalHunter() : base()
        {
            BlockedBy = new Type[] { typeof(HumanoidHunter) };
            TalentDependency = typeof(ExperiencedHunter);
            DisplayName = "Elemental hunter";
            Description = "Increases damage to elementals and heals damage from them.";
            ImageID = 175;
        }
        public override void CheckDefenseEffect(Mobile defender, Mobile target, int damage)
        {
            if (IsMobileType(OppositionGroup.ElementalGroup, target.GetType()))
            {
                defender.Heal(AOS.Scale(damage, Level));
            }
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (IsMobileType(OppositionGroup.ElementalGroup, target.GetType()))
            {
                target.Damage(Level, attacker);
            }
        }
    }
}

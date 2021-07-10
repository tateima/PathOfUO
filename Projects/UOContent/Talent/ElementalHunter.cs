using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            ImageID = 30091;
        }
        public void CheckDefenseEffect(Mobile defender, Mobile target, int damage)
        {
            if (IsMobileType(target, OppositionGroup.ElementalGroup, target.GetType()))
            {
                defender.Heal((damage / 100) * Level);
            }
        }

        public void CheckHitEffect(Mobile attacker, Mobile target)
        {
            if (IsMobileType(target, OppositionGroup.ElementalGroup, target.GetType()))
            {
                target.Damage(Level, attacker);
            }
        }
    }
}

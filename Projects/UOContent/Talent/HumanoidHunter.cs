using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class HumanoidHunter : BaseTalent, ITalent
    {
        public HumanoidHunter() : base()
        {
            BlockedBy = new Type[] { typeof(ElementalHunter) };
            TalentDependency = typeof(ExperiencedHunter);
            DisplayName = "Humanoid hunter";
            Description = "Increases damage to humanoids and lowers damage from them.";
            ImageID = 40158;
        }

        public void CheckDefenseEffect(Mobile defender, Mobile target, int damage)
        {
            if (IsMobileType(target, OppositionGroup.HumanoidGroup, target.GetType()))
            {
                defender.Heal((damage/100)*Level);
            }
        }

        public void CheckHitEffect(Mobile attacker, Mobile target)
        {
            if (IsMobileType(target, OppositionGroup.HumanoidGroup, target.GetType()) || target is PlayerMobile)
            {
                target.Damage(Level, attacker);
            }
        }


    }
}

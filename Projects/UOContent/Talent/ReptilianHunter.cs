using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class ReptilianHunter : BaseTalent, ITalent
    {
        public ReptilianHunter() : base()
        {
            BlockedBy = new Type[] { typeof(ArachnidHunter) };
            TalentDependency = typeof(ExperiencedHunter);
            DisplayName = "Reptilian hunter";
            Description = "Increases damage to reptiles and heals damage from them.";
            ImageID = 49994;
        }

        public void CheckDefenseEffect(Mobile defender, Mobile target, int damage)
        {
            if (IsMobileType(target, OppositionGroup.ReptilianGroup, target.GetType()))
            {
                defender.Heal((damage / 100) * Level);
            }
        }

        public void CheckHitEffect(Mobile attacker, Mobile target)
        {
            if (IsMobileType(target, OppositionGroup.ReptilianGroup, target.GetType()))
            {
                target.Damage(Level, attacker);
            }
        }

    }
}

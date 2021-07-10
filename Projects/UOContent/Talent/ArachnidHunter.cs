using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class ArachnidHunter : BaseTalent, ITalent
    {
        public ArachnidHunter() : base()
        {
            BlockedBy = new Type[] { typeof(ReptilianHunter) };
            TalentDependency = typeof(ExperiencedHunter);
            DisplayName = "Arachnid hunter";
            Description = "Increases damage to arachnids and heals damage from them.";
            ImageID = 30213;
        }
        public void CheckDefenseEffect(Mobile defender, Mobile target, int damage)
        {
            if (IsMobileType(target, OppositionGroup.ArachnidGroup, target.GetType()))
            {
                defender.Heal((damage / 100) * Level);
            }
        }

        public void CheckHitEffect(Mobile attacker, Mobile target)
        {
            if (IsMobileType(target, OppositionGroup.ArachnidGroup, target.GetType()))
            {
                target.Damage(Level, attacker);
            }
        }
    }
}

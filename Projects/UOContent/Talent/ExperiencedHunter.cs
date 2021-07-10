using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class ExperiencedHunter : BaseTalent, ITalent
    {
        public ExperiencedHunter() : base()
        {
            DisplayName = "Experienced hunter";
            Description = "Increases damage to animals.";
            ImageID = 39867;
        }

        public void CheckHitEffect(Mobile attacker, Mobile target)
        {
            if (IsMobileType(target, OppositionGroup.AnimalGroup, target.GetType()))
            {
                target.Damage(Level, attacker);
            }
        }
    }
}

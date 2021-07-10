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
            Description = "Increases damage to reptiles and lowers damage from them.";
            ImageID = 49994;
        }

    }
}

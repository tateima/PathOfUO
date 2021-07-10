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
            Description = "Increases damage to arachnids and lowers damage from them.";
            ImageID = 30213
                ;
        }

    }
}

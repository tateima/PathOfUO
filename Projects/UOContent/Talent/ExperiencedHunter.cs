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
            TalentDependency = typeof(HunterOfWild);
            DisplayName = "Experienced hunter";
            Description = "Increases damage to animals.";
            ImageID = 39867;
        }

    }
}

using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class FastLearner : BaseTalent, ITalent
    {
        public FastLearner() : base()
        {
            TalentDependency = typeof(DivineIntellect);
            DisplayName = "Fast learner";
            Description = "Increases your experience gain from sources.";
            ImageID = 30223;
        }

    }
}

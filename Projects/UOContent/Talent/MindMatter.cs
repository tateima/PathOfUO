using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class MindMatter : BaseTalent, ITalent
    {
        public MindMatter() : base()
        {
            TalentDependency = typeof(FastLearner);
            DisplayName = "Mind matter";
            Description = "Increases your resistance to magic effects.";
            ImageID = 30101;
        }

    }
}

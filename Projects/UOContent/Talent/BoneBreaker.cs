using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class BoneBreaker : BaseTalent, ITalent
    {
        public BoneBreaker() : base()
        {
            TalentDependency = typeof(IronSkin);
            DisplayName = "Bone breaker";
            Description = "Next hit paralyzes target for 20 seconds. 5 minute cooldown";
            ImageID = 30228;
        }

    }
}

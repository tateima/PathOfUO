using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class PainManagement : BaseTalent, ITalent
    {
        public PainManagement() : base()
        {
            TalentDependency = typeof(BoneBreaker);
            DisplayName = "Pain management";
            Description = "Decrease bleeding effects and poison damage.";
            ImageID = 30104;
        }

    }
}

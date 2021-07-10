using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class KeenSenses : BaseTalent, ITalent
    {
        public KeenSenses() : base()
        {
            TalentDependency = typeof(KeenEye);
            DisplayName = "Keen senses";
            Description = "Chance of dodging incoming attacks.";
            ImageID = 30200;
        }

    }
}

using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class RangerCommand : BaseTalent, ITalent
    {

        public RangerCommand() : base()
        {
            TalentDependency = typeof(NatureAffinity);
            DisplayName = "Ranger command";
            Description = "Increase power of pets.";
            ImageID = 30039;
        }

    }
}

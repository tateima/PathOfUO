using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class StrongTools : BaseTalent, ITalent
    {
        public StrongTools() : base()
        {
            TalentDependency = typeof(CraftFocus);
            DisplayName = "Strong tools";
            Description = "Your tools are less likely to lose durability on use.";
            ImageID = 30099;
        }

    }
}

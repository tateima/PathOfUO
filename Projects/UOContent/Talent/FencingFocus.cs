using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class FencingFocus : BaseTalent, ITalent
    {
        public FencingFocus() : base()
        {
            BlockedBy = new Type[] { typeof(ArcherFocus) };
            DisplayName = "Fencing focus";
            Description = "Chance of getting a critical strike with fencing weapons.";
            ImageID = 30199;
        }

    }
}

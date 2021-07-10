using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class ShieldFocus : BaseTalent, ITalent
    {
        public ShieldFocus() : base()
        {
            DisplayName = "Shield focus";
            Description = "Decreases damage taken with shield equipped.";
            ImageID = 30233;
        }

    }
}

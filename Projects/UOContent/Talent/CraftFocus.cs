using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class CraftFocus : BaseTalent, ITalent
    {
        public CraftFocus() : base()
        {
            DisplayName = "Craft focus";
            Description = "Increases durability and damage done for crafted items.";
            ImageID = 30119;
        }

    }
}

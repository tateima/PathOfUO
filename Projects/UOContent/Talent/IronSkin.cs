using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class IronSkin : BaseTalent, ITalent
    {
        public IronSkin() : base()
        {
            TalentDependency = typeof(GiantsHeritage);
            DisplayName = "Iron skin";
            Description = "Increases armor rating.";
            ImageID = 30214;
        }

    }
}

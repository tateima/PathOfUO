using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class GiantsHeritage : BaseTalent, ITalent
    {
        public GiantsHeritage() : base()
        {
            TalentDependency = typeof(DivineStrength);
            DisplayName = "Giant's Heritage";
            Description = "Increases hit points and carrying capacity.";
            ImageID = 30036;
        }

    }
}

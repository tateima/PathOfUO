using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class KeenEye : BaseTalent, ITalent
    {
        public KeenEye() : base()
        {
            TalentDependency = typeof(DivineDexterity);
            DisplayName = "Keen eyes";
            Description = "Increased chance of finding special loot on corpses.";
            ImageID = 30212;
        }

    }
}

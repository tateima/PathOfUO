using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class LoreTeacher : BaseTalent, ITalent
    {
        public LoreTeacher() : base()
        {
            TalentDependency = typeof(LoreSeeker);
            DisplayName = "Lore teacher";
            Description = "Party members receive a experience increase.";
            ImageID = 39876;
        }

    }
}

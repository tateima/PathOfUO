using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class LoreTeacher : BaseTalent, ITalent
    {
        public LoreTeacher() : base()
        {
            TalentDependency = typeof(LoreDisciples);
            DisplayName = "Lore teacher";
            Description = "Increases skill levels for disciple followers.";
            ImageID = 39876;
        }

    }
}

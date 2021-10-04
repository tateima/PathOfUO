using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class BugFixer : BaseTalent, ITalent
    {
        public BugFixer() : base()
        {
            TalentDependency = typeof(Inventive);
            DisplayName = "Bug fixer";
            Description = "Reduces chances of device failure.";
            ImageID = 353;
            GumpHeight = 70;
            AddEndY = 75;
        }
    }
}

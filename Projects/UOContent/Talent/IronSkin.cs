using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class IronSkin : BaseTalent, ITalent
    {
        public IronSkin() : base()
        {
            TalentDependency = typeof(GiantsHeritage);
            DisplayName = "Iron skin";
            Description = "Increases armor rating.";
            ImageID = 119;
            GumpHeight = 70;
            AddEndY = 65;
        }
    }
}

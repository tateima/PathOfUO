using System;
using Server.Mobiles;

namespace Server.Talent
{
    public class FastLearner : BaseTalent, ITalent
    {
        public FastLearner() : base()
        {
            TalentDependency = typeof(DivineIntellect);
            DisplayName = "Fast learner";
            Description = "Increases your experience gain from sources by 2% per level.";
            ImageID = 328;
            GumpHeight = 85;
            AddEndY = 80;
        }

    }
}

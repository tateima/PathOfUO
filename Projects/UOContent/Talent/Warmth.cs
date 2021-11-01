using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class Warmth : BaseTalent, ITalent
    {
        public Warmth() : base()
        {
            TalentDependency = typeof(DragonAspect);
            DisplayName = "Warmth";
            Description = "Reduces chance to be frozen by 15% per level.";
            ImageID = 387;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override int ModifySpellMultiplier() {
            return Level * 15;
        }
    }
}

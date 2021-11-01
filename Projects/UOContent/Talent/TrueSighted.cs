using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class TrueSighted : BaseTalent, ITalent
    {
        public TrueSighted() : base()
        {
            TalentDependency = typeof(KeenEye);
            DisplayName = "Truesighted";
            Description = "Reduces penalty from blindness by 15% per level.";
            ImageID = 387;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override int ModifySpellMultiplier() {
            return Level * 15;
        }
    }
}

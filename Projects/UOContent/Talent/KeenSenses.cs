using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class KeenSenses : BaseTalent, ITalent
    {
        public KeenSenses() : base()
        {
            TalentDependency = typeof(KeenEye);
            DisplayName = "Keen senses";
            Description = "Chance of dodging incoming attacks.";
            ImageID = 117;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public bool CheckDodge()
        {
            return (Utility.Random(100) < Level * 2);
        }

    }
}

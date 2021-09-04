using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class HandFinesse : BaseTalent, ITalent
    {
        public HandFinesse() : base()
        {
            TalentDependency = typeof(EscapeDeath);
            DisplayName = "Hand finesse";
            Description = "Decrease time between attacks and reduce damage to weapons on hit.";
            ImageID = 30203;
        }
    }
}

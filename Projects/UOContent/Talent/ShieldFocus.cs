using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class ShieldFocus : BaseTalent, ITalent
    {
        public ShieldFocus() : base()
        {
            DisplayName = "Shield focus";
            Description = "Decreases damage taken with shield equipped.";
            ImageID = 30233;
        }

    }
}

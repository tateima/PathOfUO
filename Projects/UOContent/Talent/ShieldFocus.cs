using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class ShieldFocus : BaseTalent, ITalent
    {
        public ShieldFocus() : base()
        {
            DisplayName = "Shield focus";
            Description = "Decreases damage taken by spells and attack while shield equipped.";
            ImageID = 146;
            GumpHeight = 70;
            AddEndY = 65;
        }

    }
}

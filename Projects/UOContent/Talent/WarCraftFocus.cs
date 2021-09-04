using System;
using Server.Items;


namespace Server.Talent
{
    public class WarCraftFocus : BaseTalent, ITalent
    {
        public WarCraftFocus() : base()
        {
            DisplayName = "Warcraft focus";
            Description = "Increases durability and damage done for crafted weapons and armor.";
            ImageID = 30119;
            MaxLevel = 10;
        }

    }
}

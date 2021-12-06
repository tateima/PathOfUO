namespace Server.Talent
{
    public class WarCraftFocus : BaseTalent
    {
        public WarCraftFocus()
        {
            DisplayName = "Warcraft focus";
            Description = "Increases durability and damage done for crafted weapons and armor.";
            ImageID = 355;
            MaxLevel = 10;
            GumpHeight = 85;
            AddEndY = 80;
        }
    }
}

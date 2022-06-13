namespace Server.Talent
{
    public class WarCraftFocus : BaseTalent
    {
        public WarCraftFocus()
        {
            DisplayName = "Warcraft focus";
            Description = "Increases durability and damage done for crafted weapons and armor.";
            ImageID = 355;
            MaxLevel = 5;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public bool CheckSuccess() => Utility.Random(100) < Level;
    }
}

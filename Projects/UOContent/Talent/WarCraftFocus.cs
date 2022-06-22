namespace Server.Talent
{
    public class WarCraftFocus : BaseTalent
    {
        public WarCraftFocus()
        {
            DisplayName = "Warcraft focus";
            Description = "Increases durability and damage done for crafted weapons and armor.";
            AdditionalDetail = $"The chance of success increases by 1% per level. The durability and damage to crafted items increases by 1 point per level. {PassiveDetail}";
            ImageID = 355;
            MaxLevel = 5;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public bool CheckSuccess() => Utility.Random(100) < Level;
    }
}

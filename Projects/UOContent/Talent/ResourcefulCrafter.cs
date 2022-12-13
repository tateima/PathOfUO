namespace Server.Talent
{
    public class ResourcefulCrafter : BaseTalent
    {
        public ResourcefulCrafter()
        {
            TalentDependencies = new[] { typeof(WarCraftFocus) };
            DisplayName = "Efficient crafting";
            Description = "Reduce material costs for crafting.";
            AdditionalDetail = $"The cost of crafted items decreases by 1 point per level. {PassiveDetail}";
            ImageID = 37;
            GumpHeight = 85;
            AddEndY = 80;
        }
    }
}

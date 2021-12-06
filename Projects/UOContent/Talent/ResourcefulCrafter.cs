namespace Server.Talent
{
    public class ResourcefulCrafter : BaseTalent
    {
        public ResourcefulCrafter()
        {
            TalentDependency = typeof(WarCraftFocus);
            DisplayName = "Efficient crafting";
            Description = "Reduce material costs for crafting.";
            ImageID = 37;
            GumpHeight = 85;
            AddEndY = 80;
        }
    }
}

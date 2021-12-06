namespace Server.Talent
{
    public class EfficientSmelter : BaseTalent
    {
        public EfficientSmelter()
        {
            TalentDependency = typeof(ResourcefulHarvester);
            DisplayName = "Metal worker";
            Description = "1% Chance per level on harvesting to receive extra resources.";
            ImageID = 356;
            MaxLevel = 6;
            GumpHeight = 85;
            AddEndY = 90;
            MaxLevel = 10;
        }
    }
}

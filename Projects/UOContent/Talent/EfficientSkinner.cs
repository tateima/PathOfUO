namespace Server.Talent
{
    public class EfficientSkinner : BaseTalent
    {
        public EfficientSkinner()
        {
            TalentDependency = typeof(ResourcefulHarvester);
            DisplayName = "Skinner";
            Description = "1% Chance per level to receive extra hide material when skinning creatures.";
            ImageID = 359;
            MaxLevel = 6;
            GumpHeight = 85;
            AddEndY = 85;
            MaxLevel = 10;
        }
    }
}

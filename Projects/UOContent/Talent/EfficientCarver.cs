namespace Server.Talent
{
    public class EfficientCarver : BaseTalent
    {
        public EfficientCarver()
        {
            TalentDependency = typeof(ResourcefulHarvester);
            DisplayName = "Carver";
            Description = "1% Chance per level to receive extra plank materials when carving.";
            ImageID = 357;
            MaxLevel = 6;
            GumpHeight = 85;
            AddEndY = 80;
            MaxLevel = 10;
        }
    }
}

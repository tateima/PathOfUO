namespace Server.Talent
{
    public class ResourcefulHarvester : BaseTalent
    {
        public ResourcefulHarvester()
        {
            DisplayName = "Resourceful";
            Description = "Chance on harvesting to receive extra resources.";
            ImageID = 124;
            MaxLevel = 6;
            GumpHeight = 85;
            AddEndY = 55;
        }
    }
}

namespace Server.Talent
{
    public class ResourcefulHarvester : BaseTalent
    {
        public ResourcefulHarvester()
        {
            DisplayName = "Resourceful";
            Description = "2% Chance per level on harvesting to receive extra resources.";
            ImageID = 124;
            MaxLevel = 6;
            GumpHeight = 85;
            AddEndY = 55;
        }

        public override int ModifySpellMultiplier() => Level * 2;
    }
}

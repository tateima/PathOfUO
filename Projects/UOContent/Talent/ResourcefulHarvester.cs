namespace Server.Talent
{
    public class ResourcefulHarvester : BaseTalent
    {
        public ResourcefulHarvester()
        {
            DisplayName = "Resourceful";
            Description = "Chance when harvesting to receive extra resources.";
            AdditionalDetail = $"The chance for this increases by 2% per level. {PassiveDetail}";
            ImageID = 124;
            MaxLevel = 6;
            GumpHeight = 85;
            AddEndY = 55;
        }

        public override int ModifySpellMultiplier() => Level * 2;
    }
}

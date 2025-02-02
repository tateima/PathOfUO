namespace Server.Talent
{
    public class TycoonCrafter : BaseTalent
    {
        public TycoonCrafter()
        {
            TalentDependencies = new[] { typeof(ResourcefulCrafter) };
            DisplayName = "Tycoon Crafter";
            Description = "Increases value of crafted armor and weapon items sold to vendor.";
            AdditionalDetail = $"The value of these items increase by 5% per level. {PassiveDetail}";
            ImageID = 354;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override int ModifySpellMultiplier() => Level * 2;
    }
}

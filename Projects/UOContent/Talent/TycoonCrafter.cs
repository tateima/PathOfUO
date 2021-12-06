namespace Server.Talent
{
    public class TycoonCrafter : BaseTalent
    {
        public TycoonCrafter()
        {
            TalentDependency = typeof(ResourcefulCrafter);
            DisplayName = "Tycoon Crafter";
            Description = "Increases value of crafted armor and weapon items sold to vendor.";
            ImageID = 354;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override double ModifySpellScalar() => Level / 100 * 2; // 2% per point
    }
}

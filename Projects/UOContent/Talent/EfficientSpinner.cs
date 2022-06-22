namespace Server.Talent
{
    public class EfficientSpinner : BaseTalent
    {
        public EfficientSpinner()
        {
            TalentDependency = typeof(ResourcefulHarvester);
            DisplayName = "Thread master";
            Description = "Chance to receive extra tailoring materials when spinning.";
            AdditionalDetail = $"The chance for this increases by 3% per level. {PassiveDetail}";
            ImageID = 358;
            MaxLevel = 6;
            GumpHeight = 85;
            AddEndY = 80;
            MaxLevel = 10;
        }
        public override int ModifySpellMultiplier() => Level * 3;
    }
}

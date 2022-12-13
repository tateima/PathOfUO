namespace Server.Talent
{
    public class BugFixer : BaseTalent
    {
        public BugFixer()
        {
            TalentDependencies = new[] { typeof(Inventive) };
            DisplayName = "Bug fixer";
            Description = "Reduces chances of device failure.";
            AdditionalDetail = "The chance of failure decreases by 1% per level.";
            ImageID = 353;
            GumpHeight = 70;
            AddEndY = 75;
        }
    }
}

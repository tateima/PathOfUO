namespace Server.Talent
{
    public class BugFixer : BaseTalent
    {
        public BugFixer()
        {
            TalentDependency = typeof(Inventive);
            DisplayName = "Bug fixer";
            Description = "Reduces chances of device failure.";
            ImageID = 353;
            GumpHeight = 70;
            AddEndY = 80;
        }
    }
}

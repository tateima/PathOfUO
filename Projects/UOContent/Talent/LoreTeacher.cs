namespace Server.Talent
{
    public class LoreTeacher : BaseTalent
    {
        public LoreTeacher()
        {
            TalentDependency = typeof(LoreDisciples);
            DisplayName = "Lore teacher";
            Description = "Increases skill levels for disciple followers.";
            ImageID = 185;
            GumpHeight = 85;
            AddEndY = 80;
        }
    }
}

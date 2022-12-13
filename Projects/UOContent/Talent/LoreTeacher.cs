namespace Server.Talent
{
    public class LoreTeacher : BaseTalent
    {
        public LoreTeacher()
        {
            TalentDependencies = new[] { typeof(LoreDisciples) };
            DisplayName = "Lore teacher";
            Description = "Increases skill and stat levels for disciple followers.";
            AdditionalDetail = "The skill increases by 2 points per level and stats by 1%.";
            ImageID = 185;
            GumpHeight = 85;
            AddEndY = 80;
            MaxLevel = 7;
        }
    }
}

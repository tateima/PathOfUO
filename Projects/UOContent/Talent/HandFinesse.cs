namespace Server.Talent
{
    public class HandFinesse : BaseTalent
    {
        public HandFinesse()
        {
            TalentDependency = typeof(EscapeDeath);
            DisplayName = "Hand finesse";
            Description = "Decrease time between attacks and reduce damage to weapons on hit.";
            ImageID = 130;
            GumpHeight = 75;
            AddEndY = 105;
        }
    }
}

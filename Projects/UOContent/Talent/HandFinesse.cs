namespace Server.Talent
{
    public class HandFinesse : BaseTalent
    {
        public HandFinesse()
        {
            TalentDependency = typeof(EscapeDeath);
            DisplayName = "Hand finesse";
            Description = "Decrease time between attacks and reduce damage to weapons on hit.";
            AdditionalDetail = $"Each level decreases swing time by 3 points and increases save checks on weapon durability by 1%. {PassiveDetail}";
            AddEndAdditionalDetailsY = 100;
            ImageID = 130;
            GumpHeight = 75;
            AddEndY = 105;
        }
    }
}

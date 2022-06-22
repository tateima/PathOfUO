using Server.Pantheon;

namespace Server.Talent
{
    public class Wisdom : BaseTalent
    {
        public Wisdom()
        {
            DeityAlignment = Deity.Alignment.Charity;
            RequiresDeityFavor = true;
            DisplayName = "Wisdom";
            Description = "All experience gains are increased.";
            AdditionalDetail = $"Each level increases experience gain by 30%. {PassiveDetail}";
            ImageID = 424;
            GumpHeight = 75;
            AddEndY = 100;
        }
    }
}

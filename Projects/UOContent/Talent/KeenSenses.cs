namespace Server.Talent
{
    public class KeenSenses : BaseTalent
    {
        public KeenSenses()
        {
            TalentDependency = typeof(KeenEye);
            DisplayName = "Keen senses";
            Description = "Chance of dodging incoming attacks.";
            ImageID = 117;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public bool CheckDodge() => Utility.Random(100) < Level * 2;
    }
}

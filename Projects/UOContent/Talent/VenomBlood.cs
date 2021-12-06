namespace Server.Talent
{
    public class VenomBlood : BaseTalent
    {
        public VenomBlood()
        {
            TalentDependency = typeof(ViperAspect);
            BlockedBy = new[] { typeof(DragonAspect) };
            DisplayName = "Venom Blood";
            Description = "Poison damage can no longer kill you and you can heal while poisoned.";
            ImageID = 374;
            GumpHeight = 75;
            AddEndY = 75;
            MaxLevel = 1;
        }
    }
}

namespace Server.Talent
{
    public class VenomBlood : BaseTalent
    {
        public VenomBlood()
        {
            TalentDependencies = new[] { typeof(ViperAspect) };
            BlockedBy = new[] { typeof(DragonAspect) };
            DisplayName = "Venom Blood";
            Description = "Poison damage can no longer kill you and you can heal while poisoned.";
            ImageID = 374;
            GumpHeight = 75;
            AddEndY = 100;
            MaxLevel = 1;
        }

        public override bool HasSkillRequirement(Mobile mobile) => mobile.Skills.Poisoning.Base >= 60;
    }
}

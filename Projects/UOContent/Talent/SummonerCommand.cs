namespace Server.Talent
{
    public class SummonerCommand : BaseTalent
    {
        public SummonerCommand()
        {
            TalentDependencies = new[] { typeof(DarkAffinity) };
            DisplayName = "Summoner command";
            Description = "Increases power of minions and summoned creatures. Requires 70+ necromancy.";
            AdditionalDetail = $"The strength of creatures increases by 3% per level. {PassiveDetail}";
            ImageID = 136;
            GumpHeight = 85;
            AddEndY = 75;
        }

        public override bool HasSkillRequirement(Mobile mobile) => mobile.Skills[SkillName.Necromancy].Base >= 70;
    }
}

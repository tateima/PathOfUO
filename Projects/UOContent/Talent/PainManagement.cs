namespace Server.Talent
{
    public class PainManagement : BaseTalent
    {
        public PainManagement()
        {
            TalentDependencies = new[] { typeof(BoneBreaker) };
            DisplayName = "Pain management";
            Description = "Decrease bleeding effects and poison damage.";
            AdditionalDetail = $"Each level improves the effect by 2%. {PassiveDetail}";
            ImageID = 186;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override int ModifySpellMultiplier() => Level * 2; // 2% per point
    }
}

namespace Server.Talent
{
    public class PainManagement : BaseTalent
    {
        public PainManagement()
        {
            TalentDependency = typeof(BoneBreaker);
            DisplayName = "Pain management";
            Description = "Decrease bleeding effects and poison damage.";
            ImageID = 186;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override int ModifySpellMultiplier() => Level * 2; // 2% per point
    }
}

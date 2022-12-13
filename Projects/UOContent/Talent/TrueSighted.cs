namespace Server.Talent
{
    public class TrueSighted : BaseTalent
    {
        public TrueSighted()
        {
            TalentDependencies = new[] { typeof(KeenEye) };
            DisplayName = "True sighted";
            Description = "Reduces penalty from blindness by 15% per level.";
            AdditionalDetail = $"{PassiveDetail}";
            ImageID = 387;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override int ModifySpellMultiplier() => Level * 15;
    }
}

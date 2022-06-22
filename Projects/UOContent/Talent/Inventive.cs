namespace Server.Talent
{
    public class Inventive : BaseTalent
    {
        public Inventive()
        {
            TalentDependency = typeof(MerchantPorter);
            DisplayName = "Inventive";
            Description = "Increases potency of devices and inventions.";
            AdditionalDetail = "This potency is increased by 15% per level.";
            ImageID = 29;
            MaxLevel = 6;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override int ModifySpellMultiplier() => Level * 15;
    }
}

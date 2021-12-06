namespace Server.Talent
{
    public class Inventive : BaseTalent
    {
        public Inventive()
        {
            TalentDependency = typeof(MerchantPorter);
            DisplayName = "Inventive";
            Description = "Increases potency of devices and inventions.";
            ImageID = 29;
            MaxLevel = 6;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override int ModifySpellMultiplier() => Level * 15;
    }
}

namespace Server.Talent
{
    public class SmoothTalker : BaseTalent
    {
        public SmoothTalker()
        {
            BlockedBy = new[] { typeof(MerchantPorter) };
            DisplayName = "Smooth talker";
            Description = "Receive discounts on items from vendors, scales with level.";
            AdditionalDetail = $"This scale increases by 2% per point. {PassiveDetail}";
            ImageID = 366;
            GumpHeight = 85;
            AddEndY = 80;
            MaxLevel = 10;
        }

        public override double ModifySpellScalar() => Level * 2; // 2% per point
    }
}

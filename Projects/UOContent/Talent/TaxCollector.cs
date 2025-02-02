using Server.Gumps;

namespace Server.Talent
{
    public class TaxCollector : BaseTalent
    {
        public TaxCollector()
        {
            CanBeUsed = true;
            TalentDependencies = new[] { typeof(SmoothTalker) };
            DisplayName = "Land Lord";
            Description = "Receive tax payments from a maximum of 10 vendors every 3h, can result in gold loss.";
            AdditionalDetail = $"The chance of loss decreases by 1% per level. The tax received increases by 5% per level. {PassiveDetail}";
            ImageID = 364;
            GumpHeight = 85;
            AddEndY = 100;
            MaxLevel = 10;
        }

        public bool VendorCantPay() => Utility.Random(100) < 15 - Level;

        public override double ModifySpellScalar() => Level / 100 * 2; // 2% per point

        public override void OnUse(Mobile from)
        {
            from.SendGump(new TaxCollectorGump(from));
        }
    }
}

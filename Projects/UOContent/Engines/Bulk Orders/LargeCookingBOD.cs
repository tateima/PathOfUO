namespace Server.Engines.BulkOrders
{
    [Serializable(0, false)]
    public partial class LargeCookingBOD : LargeBOD
    {
        [Constructible]
        public LargeCookingBOD()
        {
            LargeBulkEntry[] targetEntries = LargeBulkEntry.ConvertEntries(this, LargeBulkEntry.OldWorldCooking);
            if (Core.ML && Utility.RandomBool()) {
                targetEntries = LargeBulkEntry.ConvertEntries(this, LargeBulkEntry.MLCooking);
            } else if (Core.SE && Utility.RandomBool()) {
                targetEntries = LargeBulkEntry.ConvertEntries(this, LargeBulkEntry.SECooking);
            }
            var hue = 0x483;
            var amountMax = Utility.RandomList(10, 15, 20, 20);
            var reqExceptional = false;

            var material = BulkMaterialType.None;

            Hue = hue;
            AmountMax = amountMax;
            Entries = targetEntries;
            RequireExceptional = reqExceptional;
            Material = material;
        }

        public LargeCookingBOD(int amountMax, bool reqExceptional, BulkMaterialType mat, LargeBulkEntry[] entries)
            : base(0x483, amountMax, reqExceptional, mat, entries)
        {
        }

        public override int ComputeFame() => CookingRewardCalculator.Instance.ComputeFame(this);

        public override int ComputeGold() => CookingRewardCalculator.Instance.ComputeGold(this);

        public override RewardGroup GetRewardGroup() =>
            TailorRewardCalculator.Instance.LookupRewards(CookingRewardCalculator.Instance.ComputePoints(this));
    }
}

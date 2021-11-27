using System;
using System.Collections.Generic;
using Server.Engines.Craft;

namespace Server.Engines.BulkOrders
{
    [Serializable(0, false)]
    public partial class SmallCookingBOD : SmallBOD
    {
        private SmallCookingBOD(SmallBulkEntry entry, BulkMaterialType mat, int amountMax, bool reqExceptional)
            : base(0x44E, 0, amountMax, entry.Type, entry.Number, entry.Graphic, reqExceptional, mat)
        {
        }

        [Constructible]
        public SmallCookingBOD()
        {
            SmallBulkEntry[] targetEntries = SmallBulkEntry.OldWorldCooking;
            if (Core.ML && Utility.RandomBool()) {
                targetEntries = SmallBulkEntry.MLCooking;
            } else if (Core.SE && Utility.RandomBool()) {
                targetEntries = SmallBulkEntry.SECooking;
            }
            var entries = targetEntries;
            if (entries.Length <= 0)
            {
                return;
            }
            var hue = 0x44E;
            var amountMax = Utility.RandomList(10, 15, 20);

            var material = BulkMaterialType.None;
            var reqExceptional = false;

            var entry = entries.RandomElement();

            Hue = hue;
            AmountMax = amountMax;
            Type = entry.Type;
            Number = entry.Number;
            Graphic = entry.Graphic;
            RequireExceptional = reqExceptional;
            Material = material;
        }

        public SmallCookingBOD(
            int amountCur, int amountMax, Type type, int number, int graphic, bool reqExceptional,
            BulkMaterialType mat
        ) : base(0x44E, amountCur, amountMax, type, number, graphic, reqExceptional, mat)
        {
        }

        public override int ComputeFame() => CookingRewardCalculator.Instance.ComputeFame(this);

        public override int ComputeGold() => CookingRewardCalculator.Instance.ComputeGold(this);

        public override RewardGroup GetRewardGroup() =>
            CookingRewardCalculator.Instance.LookupRewards(CookingRewardCalculator.Instance.ComputePoints(this));

        public static SmallCookingBOD CreateRandomFor(Mobile m)
        {
            var entries = SmallBulkEntry.OldWorldCooking;

            if (entries.Length <= 0)
            {
                return null;
            }

            var theirSkill = m.Skills.Cooking.Base;

            int amountMax = theirSkill switch
            {
                >= 70.1 => Utility.RandomList(10, 15, 20, 20),
                >= 50.1 => Utility.RandomList(10, 15, 15, 20),
                _       => Utility.RandomList(10, 10, 15, 20)
            };

            var material = BulkMaterialType.None;
            var reqExceptional = false;
            var system = DefCooking.CraftSystem;
            var validEntries = new List<SmallBulkEntry>();

            for (var i = 0; i < entries.Length; ++i)
            {
                var item = system.CraftItems.SearchFor(entries[i].Type);

                if (item != null)
                {
                    var chance = item.GetSuccessChance(m, null, system, false, out var allRequiredSkills);

                    if (allRequiredSkills && chance >= 0.0)
                    {
                        if (chance > 0.0)
                        {
                            validEntries.Add(entries[i]);
                        }
                    }
                }
            }

            if (validEntries.Count <= 0)
            {
                return null;
            }

            var entry = validEntries.RandomElement();
            return new SmallCookingBOD(entry, material, amountMax, reqExceptional);
        }
    }
}

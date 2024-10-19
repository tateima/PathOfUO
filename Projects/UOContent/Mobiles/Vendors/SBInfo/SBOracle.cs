using System.Collections.Generic;
using Server.Items;

namespace Server.Mobiles
{
    public class SBOracle : SBInfo
    {
        public override IShopSellInfo SellInfo { get; } = new InternalSellInfo();

        public override List<GenericBuyInfo> BuyInfo { get; } = new InternalBuyInfo();

        public class InternalBuyInfo : List<GenericBuyInfo>
        {
            public InternalBuyInfo()
            {
                Add(new GenericBuyInfo(typeof(Arrow), 2, 20, 0xF3F, 0));
                Add(new GenericBuyInfo(typeof(Bolt), 5, 20, 0x1BFB, 0));
                Add(new GenericBuyInfo(typeof(Backpack), 15, 20, 0x9B2, 0));
                Add(new GenericBuyInfo(typeof(Torch), 8, 20, 0xF6B, 0));
                Add(new GenericBuyInfo(typeof(Bedroll), 5, 20, 0xA59, 0));
            }
        }

        public class InternalSellInfo : GenericSellInfo
        {
            public InternalSellInfo()
            {
                Add(typeof(Arrow), 1);
                Add(typeof(Bolt), 2);
                Add(typeof(Backpack), 7);
                Add(typeof(Torch), 4);
            }
        }
    }
}

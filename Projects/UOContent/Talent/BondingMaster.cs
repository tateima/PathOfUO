using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class BondingMaster : BaseTalent, ITalent
    {

        public BondingMaster() : base()
        {
            TalentDependency = typeof(RangerCommand);
            DisplayName = "Bonding master";
            Description = "Increase bond slot by one per level.";
            ImageID = 153;
            AddEndY = 85;
            MaxLevel = 7;
        }
        public override void UpdateMobile(Mobile mobile)
        {
            mobile.FollowersMax = CalculateResetValue(mobile.FollowersMax);
            mobile.FollowersMax = CalculateNewValue(mobile.FollowersMax);
        }

    }
}

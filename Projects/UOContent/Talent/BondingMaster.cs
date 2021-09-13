using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }
        public override void UpdateMobile(Mobile mobile)
        {
            mobile.FollowersMax = CalculateResetValue(mobile.FollowersMax);
            mobile.FollowersMax = CalculateNewValue(mobile.FollowersMax);
        }

    }
}

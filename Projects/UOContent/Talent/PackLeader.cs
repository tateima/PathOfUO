using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class PackLeader : BaseTalent, ITalent
    {

        public PackLeader() : base()
        {
            TalentDependency = typeof(BondingMaster);
            DisplayName = "Pack leader";
            Description = "Increases your stats by your total followers while out of stables.";
            MaxLevel = 1;
            ImageID = 381;
        }
        public override void UpdateMobile(Mobile mobile)
        {
            if (mobile is PlayerMobile player)
            {
                player.RemoveStatMod("PackLeader");
                player.AddStatMod(new StatMod(StatType.All, "PackLeader", player.AllFollowers.Count, TimeSpan.Zero));
            }
           
        }

    }
}

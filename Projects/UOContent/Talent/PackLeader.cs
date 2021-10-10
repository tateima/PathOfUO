using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class PackLeader : BaseTalent, ITalent
    {
        private PlayerMobile m_Player;
        public PackLeader() : base()
        {
            TalentDependency = typeof(BondingMaster);
            DisplayName = "Pack leader";
            Description = "Increases your stats by your total followers while out of stables.";
            MaxLevel = 1;
            ImageID = 381;
        }

        public void UpdateStats()
        {
            m_Player.RemoveStatMod("PackLeader");
            m_Player.AddStatMod(new StatMod(StatType.All, "PackLeader", m_Player.AllFollowers.Count, TimeSpan.Zero));
            Timer.StartTimer(TimeSpan.FromSeconds(60), UpdateStats, out _talentTimerToken);
        }
        public override void UpdateMobile(Mobile mobile)
        {
            if (mobile is PlayerMobile player)
            {
                m_Player = player;
                player.RemoveStatMod("PackLeader");
                player.AddStatMod(new StatMod(StatType.All, "PackLeader", player.AllFollowers.Count, TimeSpan.Zero));
                Timer.StartTimer(TimeSpan.FromSeconds(60), UpdateStats, out _talentTimerToken);
            }
           
        }
    }
}

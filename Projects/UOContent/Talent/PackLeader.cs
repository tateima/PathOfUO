using System;
using Server.Mobiles;

namespace Server.Talent
{
    public class PackLeader : BaseTalent
    {
        private PlayerMobile _player;

        public PackLeader()
        {
            StatModNames = new[] { "PackLeader" };
            TalentDependencies = new[] { typeof(BondingMaster) };
            DisplayName = "Pack leader";
            Description = "Increases your stats by your total followers while out of stables.";
            AdditionalDetail = "This talent modification will refresh every 60 seconds.";
            MaxLevel = 1;
            ImageID = 381;
        }

        public override void UpdateMobile(Mobile mobile)
        {
            if (mobile is PlayerMobile player)
            {
                _player = player;
                ResetMobileMods(_player);
                AddStats();
                Timer.StartTimer(TimeSpan.FromSeconds(60), UpdateStats, out _talentTimerToken);
            }
        }

        public void AddStats()
        {
            _player.AddStatMod(new StatMod(StatType.All, StatModNames[0], _player.AllFollowers.Count, TimeSpan.Zero));
        }

        public void UpdateStats()
        {
            ResetMobileMods(_player);
            AddStats();
            Timer.StartTimer(TimeSpan.FromSeconds(60), UpdateStats, out _talentTimerToken);
        }
    }
}

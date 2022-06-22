using System;
using  System.Collections.Generic;
using System.Collections.Concurrent;
using Server.Mobiles;
using Server.Pantheon;

namespace Server.Talent
{
    public class Leadership : BaseTalent
    {
        private ConcurrentDictionary<Serial, List<DefaultSkillMod>> _followerSkillMods;
        private PlayerMobile _player;

        public Leadership()
        {
            _followerSkillMods = new ConcurrentDictionary<Serial, List<DefaultSkillMod>>();
            StatModNames = new[] { "Leadership" };
            DeityAlignment = Deity.Alignment.Charity;
            RequiresDeityFavor = true;
            DisplayName = "Leadership";
            Description = "Increases the power of your followers and they dont lose loyalty to you.";
            AdditionalDetail = $"Each level increases the stats by 5 points and skills by 10 points. {PassiveDetail}";
            MaxLevel = 1;
            ImageID = 426;
        }

        public override void UpdateMobile(Mobile mobile)
        {
            if (mobile is PlayerMobile player)
            {
                _player = player;
                ResetDeityPower();
                foreach (var follower in _player.AllFollowers)
                {
                    ResetMobileMods(follower);
                    follower.AddStatMod(new StatMod(StatType.All, StatModNames[0], Level*5, TimeSpan.Zero));
                    List<Skill> skills = new List<Skill>();
                    GetTopSkills(follower, ref skills, 3);
                    List<DefaultSkillMod> skillMods = GetTopDefaultSkillMods(skills, 10.0 * Level);
                    foreach (var skillMod in skillMods)
                    {
                        follower.AddSkillMod(skillMod);
                    }
                    _followerSkillMods.TryAdd(
                        follower.Serial,
                        skillMods
                    );
                }
            }
        }

        public override void ResetDeityPower()
        {
            foreach (var follower in _player.AllFollowers)
            {
                ResetMobileMods(follower);
                List<DefaultSkillMod> currentMods;
                if (_followerSkillMods.TryRemove(follower.Serial, out currentMods))
                {
                    foreach (var skillMod in currentMods)
                    {
                        follower.RemoveSkillMod(skillMod);
                    }
                }
            }
        }
    }
}

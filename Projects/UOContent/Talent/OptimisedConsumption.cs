using System;
using System.Linq;
using Server.Gumps;

namespace Server.Talent
{
    public class OptimisedConsumption : BaseTalent
    {
        public OptimisedConsumption()
        {
            TalentDependencies = new[] { typeof(WarCraftFocus) };
            DisplayName = "Consumable focus";
            Description = "Increases effectiveness of consumed goods. Consuming alcohol also improves your fighting skills.";
            AdditionalDetail = $"Non-alcoholic beverages increase your mana. Food will increase your stats as well, but only if you are incredibly hungry. Potions are also enhanced by this talent.  All increases are calculated by 2 points per level. {PassiveDetail}";
            ImageID = 125;
            GumpHeight = 75;
            AddEndY = 100;
        }

        public override void UpdateMobile(Mobile mobile)
        {
            if (mobile.BAC > 0 && !Activated)
            {
                Activated = true;
                var group = SkillsGumpGroup.Groups.FirstOrDefault(group => group.Name == "Combat Ratings");
                if (group != null)
                {
                    Timer drunkTimer = new DrunkTimer(mobile, group.Skills.Select(skill => new DefaultSkillMod(skill, "OptimisedConsumption", true, Level * 2)).Cast<SkillMod>().ToArray());
                    drunkTimer.Start();
                }
            }
        }

        private class DrunkTimer : Timer
        {
            private readonly Mobile _drunk;
            private readonly SkillMod[] _skillMods;

            public DrunkTimer(Mobile drunk, SkillMod[] skillMods)
                : base(TimeSpan.FromSeconds(5.0), TimeSpan.FromSeconds(5.0))
            {
                _drunk = drunk;
                _skillMods = skillMods;
                foreach (SkillMod skillMod in skillMods)
                {
                    _drunk.AddSkillMod(skillMod);
                }
            }

            protected override void OnTick()
            {
                if (_drunk.Deleted || _drunk.Map == Map.Internal)
                {
                    Stop();
                }
                else if (_drunk.Alive && _drunk.BAC <= 0)
                {
                    foreach (var mod in _skillMods)
                    {
                        _drunk.RemoveSkillMod(mod);
                    }

                    Stop();
                }
            }
        }
    }
}

using System;
using System.Linq;
using Server.Gumps;

namespace Server.Talent
{
    public class OptimisedConsumption : BaseTalent
    {
        public OptimisedConsumption()
        {
            TalentDependency = typeof(WarCraftFocus);
            DisplayName = "Consumable focus";
            Description = "Increases effectiveness of consumed goods. Consuming alcohol also improves your fighting skills.";
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
                    Timer drunkTimer = new DrunkTimer(mobile, group.Skills.Select(skill => new DefaultSkillMod(skill, true, Level * 3)).Cast<SkillMod>().ToArray());
                    drunkTimer.Start();
                }
            }
        }

        private class DrunkTimer : Timer
        {
            private readonly Mobile m_Drunk;
            private readonly SkillMod[] m_SkillMods;

            public DrunkTimer(Mobile drunk, SkillMod[] skillMods)
                : base(TimeSpan.FromSeconds(5.0), TimeSpan.FromSeconds(5.0))
            {
                m_Drunk = drunk;
                m_SkillMods = skillMods;
            }

            protected override void OnTick()
            {
                if (m_Drunk.Deleted || m_Drunk.Map == Map.Internal)
                {
                    Stop();
                }
                else if (m_Drunk.Alive && m_Drunk.BAC <= 0)
                {
                    foreach (var mod in m_SkillMods)
                    {
                        m_Drunk.RemoveSkillMod(mod);
                    }

                    Stop();
                }
            }
        }
    }
}

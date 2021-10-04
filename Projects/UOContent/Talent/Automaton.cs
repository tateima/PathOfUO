using Server.Mobiles;
using Server.Spells;
using Server.Spells.Eighth;
using Server.Network;
using Server.Gumps;
using System;
using System.Linq;

namespace Server.Talent
{
    public class Automaton : BaseTalent, ITalent
    {
        private Mobile m_Automaton;
        public Automaton() : base()
        {
            BlockedBy = new Type[] { typeof(SmoothTalker) };
            TalentDependency = typeof(Inventive);
            DisplayName = "Automaton";
            MobilePercentagePerPoint = 10;
            CanBeUsed = true;
            Description = "Construct an automaton to assist you for 2 minutes. Requires 80 crafting skill points. 5 minute cooldown.";
            ImageID = 352;
            GumpHeight = 230;
            AddEndY = 145;
        }
        public override void OnUse(Mobile engineer)
        {
            if (!OnCooldown)
            {
                engineer.RevealingAction();
                // its a talent, no need for animation timer, just a single animation is fine
                engineer.Animate(269, 7, 1, true, false, 0);
                AutomatonConstruct automaton = new AutomatonConstruct();
                switch(Utility.Random(1, 7))
                {
                    case 1:
                        break;
                    case 2:
                        automaton.Body = 113;
                        break;
                    case 3:
                        automaton.Body = 111;
                        break;
                    case 4:
                        automaton.Body = 166;
                        break;
                    case 5:
                        automaton.Body = 110;
                        break;
                    case 6:
                        automaton.Body = 107;
                        break;
                    case 7:
                        automaton.Body = 109;
                        break;
                    case 8:
                        automaton.Body = 108;
                        break;
                }
                double score = EngineeringScore(engineer);
                MobilePercentagePerPoint += (int)(score / 100);
                automaton = (AutomatonConstruct)ScaleMobile((Mobile)automaton);
                if (automaton.Backpack != null)
                {
                    for (var x = automaton.Backpack.Items.Count - 1; x >= 0; x--)
                    {
                        var item = automaton.Backpack.Items[x];
                        item.Delete();
                    }
                }
                m_Automaton = automaton;
                SpellHelper.Summon(automaton, engineer, 0x042, TimeSpan.FromMinutes(2), false, false);
                Timer.StartTimer(TimeSpan.FromMinutes(5), ExpireTalentCooldown, out _talentTimerToken);
                OnCooldown = true;
            }
        }

        public double EngineeringScore(Mobile mobile)
        {
            double score = 0.0;
            SkillsGumpGroup group = SkillsGumpGroup.Groups.Where(group => group.Name == "Crafting").FirstOrDefault();
            if (group != null)
            {
                foreach (SkillName skill in group.Skills)
                {
                    score += mobile.Skills[skill].Value;
                }
            }
            return score;
        }

        public override bool HasSkillRequirement(Mobile mobile)
        {
            return EngineeringScore(mobile) >= 80.0;
        }
    }
}

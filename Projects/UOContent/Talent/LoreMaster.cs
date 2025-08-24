using System.Linq;
using Server.Gumps;

namespace Server.Talent
{
    public class LoreMaster : BaseTalent
    {
        public LoreMaster()
        {
            TalentDependencies = new[] { typeof(LoreTeacher) };
            DisplayName = "Lore master";
            Description = "Unlocks chance for stronger disciples.";
            AdditionalDetail = "Each level increases the chance of the following disciples spawning by 1% when using the lore disciples talent. Noble Lord, Archer Guard, Warrior Guard or Mage Guard.";
            ImageID = 433;
            GumpHeight = 85;
            AddEndY = 80;
            MaxLevel = 3;
        }

        public override bool HasSkillRequirement(Mobile mobile) {
            var group = SkillsGumpGroup.Groups.FirstOrDefault(group => group.Name == "Lore & Knowledge");
            int numberOfMasteries = 0;
            if (group is not null)
            {
                foreach (var skillName in group.Skills)
                {
                    if (mobile.Skills[skillName].Base >= 90)
                    {
                        numberOfMasteries++;
                    }
                }
            }
            return numberOfMasteries >= 3;
        }
    }
}

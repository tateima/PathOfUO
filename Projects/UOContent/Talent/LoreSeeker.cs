using Server.Gumps;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class LoreSeeker : BaseTalent, ITalent
    {
        public LoreSeeker() : base()
        {
            DisplayName = "Lore giver";
            Description = "Expose weaknesses in enemies on hit. Need 70 or above in any lore skill.";
            ImageID = 39903;
        }

        public override bool HasSkillRequirement(Mobile mobile) {
            SkillsGumpGroup group = SkillsGumpGroup.Groups.Where(group => group.Name == "Lore & Knowledge").First();
            bool valid = false;
            foreach (SkillName skill in group.Skills)
            {
                if (mobile.Skills[skill].Base >= 70)
                {
                    valid = true;
                    break;
                }
            }
            return valid;
        }

    }
}

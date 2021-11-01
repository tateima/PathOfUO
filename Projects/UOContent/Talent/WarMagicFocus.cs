using Server.Mobiles;
using Server.Items;
using System;

namespace Server.Talent
{
    public class WarMagicFocus : BaseTalent, ITalent
    {
        public WarMagicFocus() : base()
        {
            TalentDependency = typeof(PlanarShift);
            DisplayName = "War Magic";
            Description = "Decrease chance for spell fizzle on hit by 15% per level. Requires 20-100 magery.";
            ImageID = 373;
            AddEndY = 100;
        }

        public override int ModifySpellMultiplier()
        {
            return Level * 15;
        }
        public override bool HasSkillRequirement(Mobile mobile)
        {
            switch(Level)
            {
                case 0:
                    return mobile.Skills.Magery.Value >= 20.0;
                    break;
                case 1:
                    return mobile.Skills.Magery.Value >= 40.0;
                    break;
                case 2:
                    return mobile.Skills.Magery.Value >= 60.0;
                    break;
                case 3:
                    return mobile.Skills.Magery.Value >= 80.0;
                    break;
                case 4:
                    return mobile.Skills.Magery.Value >= 100.0;
                    break;
                case 5:
                    return true;
                    break;
            }
            return false;
        }
    }
}


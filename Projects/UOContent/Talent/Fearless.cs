using Server.Items;
using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class Fearless : BaseTalent, ITalent
    {
        public Fearless() : base()
        {
            DisplayName = "Fearless";
            Description = "Decreases chance to be feared by 2% per point. Requires at least 80+ melee skill.";
            ImageID = 401;
            GumpHeight = 85;
            AddEndY = 90;
        }
        public bool CheckFearSave(Mobile from) {
            return HasSkillRequirement(from) && Utility.Random(100) < Level * 2;
        }
        public override bool HasSkillRequirement(Mobile mobile)
        {
            return  mobile.Skills[SkillName.Swords].Base >= 80 || mobile.Skills[SkillName.Macing].Base >= 80 || mobile.Skills[SkillName.Fencing].Base >= 80 
                    || mobile.Skills[SkillName.Chivalry].Base >= 80 || mobile.Skills[SkillName.Wrestling].Base >= 80;
        }
    }
}

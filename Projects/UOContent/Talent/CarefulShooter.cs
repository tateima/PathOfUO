using System;
using Server.Items;

namespace Server.Talent
{
    public class CarefulShooter : BaseTalent, ITalent
    {
        public CarefulShooter() : base()
        {
            TalentDependency = typeof(ArcherFocus);
            RequiredWeaponSkill = SkillName.Archery;
            RequiredWeapon = new Type[] { typeof(BaseRanged) };
            DisplayName = "Careful shooter";
            Description = "Lowers chance for arrow to be lost on miss.";
            ImageID = 171;
            GumpHeight = 85;
            AddEndY = 75;
        }
    }
}

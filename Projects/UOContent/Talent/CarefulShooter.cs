using Server.Items;

namespace Server.Talent
{
    public class CarefulShooter : BaseTalent
    {
        public CarefulShooter()
        {
            TalentDependency = typeof(ArcherFocus);
            RequiredWeaponSkill = SkillName.Archery;
            RequiredWeapon = new[] { typeof(BaseRanged) };
            DisplayName = "Careful shooter";
            Description = "Lowers chance for arrow to be lost on miss.";
            AdditionalDetail = $"{PassiveDetail}";
            ImageID = 171;
            GumpHeight = 85;
            AddEndY = 75;
        }
    }
}

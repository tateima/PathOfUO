namespace Server.Talent
{
    public class Fearless : BaseTalent
    {
        public Fearless()
        {
            DisplayName = "Fearless";
            Description = "Decreases chance to be feared by 2% per point. Requires at least 80+ melee skill.";
            ImageID = 411;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override bool HasSkillRequirement(Mobile mobile) =>
            mobile.Skills[SkillName.Swords].Base >= 80 || mobile.Skills[SkillName.Macing].Base >= 80 ||
            mobile.Skills[SkillName.Fencing].Base >= 80
            || mobile.Skills[SkillName.Chivalry].Base >= 80 || mobile.Skills[SkillName.Wrestling].Base >= 80;

        public bool CheckFearSave(Mobile from) => HasSkillRequirement(from) && Utility.Random(100) < Level * 2;
    }
}

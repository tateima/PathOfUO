namespace Server.Talent
{
    public class RangerCommand : BaseTalent
    {
        public RangerCommand()
        {
            TalentDependency = typeof(NatureAffinity);
            DisplayName = "Ranger command";
            Description = "Decreases stat and skill loss of tamed creatures by 1% per level.";
            AdditionalDetail = "This talent requires at least 30 animal taming and animal lore.";
            ImageID = 188;
            AddEndY = 95;
        }

        public override bool HasSkillRequirement(Mobile mobile) => mobile.Skills[SkillName.AnimalTaming].Base >= 30 &&
                                                                   mobile.Skills[SkillName.AnimalLore].Base >= 30;
        public override double ModifySpellScalar() => Level / 100.0;
    }
}

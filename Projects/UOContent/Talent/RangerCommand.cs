namespace Server.Talent
{
    public class RangerCommand : BaseTalent
    {
        public RangerCommand()
        {
            TalentDependencies = new[] { typeof(NatureAffinity) };
            DisplayName = "Ranger command";
            Description = "Improves stat and skills loss/gain of tamed creatures.";
            AdditionalDetail = "After taming, losses will be reduced by 1% per level. When levelling, tamed creatures will receive increased gains by 0.1% per level. It will also reduce the chance of your tames losing their special suffixes (e.g Veteran/Heroic). This talent requires at least 30 animal taming and animal lore.";
            ImageID = 188;
            AddEndY = 115;
            AddEndAdditionalDetailsY = 120;
        }

        public override bool HasSkillRequirement(Mobile mobile) => mobile.Skills[SkillName.AnimalTaming].Base >= 30 &&
                                                                   mobile.Skills[SkillName.AnimalLore].Base >= 30;
        public override double ModifySpellScalar() => Level / 100.0;
    }
}

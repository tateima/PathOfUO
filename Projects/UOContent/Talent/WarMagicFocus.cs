namespace Server.Talent
{
    public class WarMagicFocus : BaseTalent
    {
        public WarMagicFocus()
        {
            TalentDependency = typeof(PlanarShift);
            DisplayName = "War Magic";
            Description = "Decrease chance for spell fizzle on hit by 8% per level. Requires 20-100 magery.";
            AdditionalDetail = $"{PassiveDetail}";
            ImageID = 373;
            AddEndY = 100;
        }

        public override int ModifySpellMultiplier() => Level * 8;

        public override bool HasSkillRequirement(Mobile mobile)
        {
            return Level switch
            {
                0 => mobile.Skills.Magery.Value >= 20.0,
                1 => mobile.Skills.Magery.Value >= 40.0,
                2 => mobile.Skills.Magery.Value >= 60.0,
                3 => mobile.Skills.Magery.Value >= 80.0,
                4 => mobile.Skills.Magery.Value >= 100.0,
                5 => true,
                _ => false
            };
        }
    }
}

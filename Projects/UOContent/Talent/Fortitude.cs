using System;

namespace Server.Talent
{
    public class Fortitude : BaseTalent
    {
        public Fortitude()
        {
            StatModNames = new[] { "Fortitude" };
            DisplayName = "Fortitude";
            Description = "Increases all stats by 5 and prevent death from starvation. Requires 85+ cooking.";
            ImageID = 405;
            GumpHeight = 85;
            MaxLevel = 1;
            AddEndY = 90;
        }

        public override bool HasSkillRequirement(Mobile mobile) => mobile.Skills.Cooking.Base >= 85;


        public override void UpdateMobile(Mobile mobile)
        {
            ResetMobileMods(mobile);
            mobile.AddStatMod(new StatMod(StatType.All, StatModNames[0], 5, TimeSpan.Zero));
        }
    }
}

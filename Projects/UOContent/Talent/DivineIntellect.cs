using System;

namespace Server.Talent
{
    public class DivineIntellect : BaseTalent
    {
        public DivineIntellect()
        {
            StatModNames = new[] { "DivineInt" };
            BlockedBy = Array.Empty<Type>();
            DisplayName = "Divine intellect";
            Description = "Increases intellect by 2 per level.";
            AdditionalDetail = $"{PassiveDetail}";
            ImageID = 132;
            GumpHeight = 70;
            AddEndY = 55;
        }

        public override void UpdateMobile(Mobile mobile)
        {
            ResetMobileMods(mobile);
            mobile.AddStatMod(new StatMod(StatType.Int, StatModNames[0], Level * 2, TimeSpan.Zero));
        }
    }
}

using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class EfficientCarver : BaseTalent, ITalent
    {
        public EfficientCarver() : base()
        {
            TalentDependency = typeof(ResourcefulHarvester);
            DisplayName = "Carver";
            Description = "0.5% Chance per level to receive extra plank materials when carving.";
            ImageID = 357;
            MaxLevel = 6;
            GumpHeight = 85;
            AddEndY = 80;
            MaxLevel = 10;
        }
    }
}

using Server.Mobiles;
using Server.Items;
using System;

namespace Server.Talent
{
    public class PlateWarmaster : BaseTalent, ITalent
    {
        public PlateWarmaster() : base()
        {
            DisplayName = "Plate warmaster";
            Description = "Reduces damage while wearing plate. Increases Str by 2 per Level";
            ImageID = 393;
            GumpHeight = 85;
            AddEndY = 80;
            UpdateOnEquip = true;
        }

        public override void UpdateMobile(Mobile mobile)
        {
            if (BaseArmor.FullPlate(mobile)) {
                mobile.RemoveStatMod("PlateWarmaster");
                mobile.AddStatMod(new StatMod(StatType.Str, "PlateWarmaster", Level*2, TimeSpan.Zero));
            }
        }
    }
}

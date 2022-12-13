using System;
using Server.Items;

namespace Server.Talent
{
    public class PlateWarmaster : BaseTalent
    {
        public PlateWarmaster()
        {
            StatModNames = new[] { "PlateWarmaster" };
            DisplayName = "Plate warmaster";
            Description = "Reduces damage while wearing plate. Increases Str by 4 per Level";
            AdditionalDetail = $"{PassiveDetail}";
            ImageID = 393;
            GumpHeight = 85;
            AddEndY = 75;
            UpdateOnEquip = true;
        }

        public override void UpdateMobile(Mobile mobile)
        {
            ResetMobileMods(mobile);
            if (Items.BaseArmor.FullPlate(mobile))
            {
                mobile.AddStatMod(new StatMod(StatType.Str, StatModNames[0], Level * 4, TimeSpan.Zero));
            }
        }
        public override int CheckDamageAbsorptionEffect(Mobile defender, Mobile attacker, int damage)
        {
            if (Items.BaseArmor.FullPlate(defender)) {
                damage -= Level;
            }
            return damage;
        }
    }
}

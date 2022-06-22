using System;
using Server.Items;

namespace Server.Talent
{
    public class LeatherWarmaster : BaseTalent
    {
        public LeatherWarmaster()
        {
            StatModNames = new[] { "LeatherWarmaster" };
            DisplayName = "Leather warmaster";
            Description = "Reduces damage while wearing leather. Increases Dex by 4 per Level";
            AdditionalDetail = $"{PassiveDetail}";
            ImageID = 386;
            GumpHeight = 85;
            AddEndY = 90;
            UpdateOnEquip = true;
        }

        public override void UpdateMobile(Mobile mobile)
        {
            ResetMobileMods(mobile);
            if (Items.BaseArmor.FullLeather(mobile))
            {
                mobile.AddStatMod(new StatMod(StatType.Dex, StatModNames[0], Level * 4, TimeSpan.Zero));
            }
        }

        public override int CheckDamageAbsorptionEffect(Mobile defender, Mobile attacker, int damage)
        {
            if (Items.BaseArmor.FullLeather(defender))
            {
                damage -= Level;
            }

            return damage;
        }
    }
}

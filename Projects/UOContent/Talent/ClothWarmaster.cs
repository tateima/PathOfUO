using System;
using Server.Items;

namespace Server.Talent
{
    public class ClothWarmaster : BaseTalent
    {
        public ClothWarmaster()
        {
            StatModNames = new[] { "ClothWarmaster" };
            DisplayName = "Cloth warmaster";
            Description = "Reduces damage while wearing cloth. Increases Int by 4 per Level";
            AdditionalDetail = $"{PassiveDetail}";
            UpdateOnEquip = true;
            ImageID = 395;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override void UpdateMobile(Mobile mobile)
        {
            ResetMobileMods(mobile);
            if (Items.BaseArmor.FullCloth(mobile))
            {
                mobile.AddStatMod(new StatMod(StatType.Int, StatModNames[0], Level * 4, TimeSpan.Zero));
            }
        }
        public override int CheckDamageAbsorptionEffect(Mobile defender, Mobile attacker, int damage)
        {
            if (Items.BaseArmor.FullCloth(defender)) {
                damage -= Level;
            }
            return damage;
        }
    }
}

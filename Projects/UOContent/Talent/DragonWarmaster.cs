using System;
using Server.Items;

namespace Server.Talent
{
    public class DragonWarmaster : BaseTalent
    {
        public DragonWarmaster()
        {
            StatModNames = new[] { "DragonWarmasterStr", "DragonWarmasterInt" };
            DisplayName = "Dragon warmaster";
            Description = "Unlocks dragon armour proficiency";
            AdditionalDetail = $"Reduces damage while wearing dragon armor. Increases Str by 3 and Int by 1 per Level. {PassiveDetail}";
            ImageID = 397;
            GumpHeight = 85;
            AddEndY = 75;
            UpdateOnEquip = true;
        }

        public override void UpdateMobile(Mobile mobile)
        {
            ResetMobileMods(mobile);
            if (Items.BaseArmor.FullDragon(mobile))
            {
                mobile.AddStatMod(new StatMod(StatType.Str, StatModNames[0], Level * 3, TimeSpan.Zero));
                mobile.AddStatMod(new StatMod(StatType.Int, StatModNames[1], Level, TimeSpan.Zero));
            }
        }

        public override int CheckDamageAbsorptionEffect(Mobile defender, Mobile attacker, int damage)
        {
            if (Items.BaseArmor.FullDragon(defender))
            {
                damage -= Level;
            }
            return damage;
        }
    }
}

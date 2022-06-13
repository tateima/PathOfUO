using System;
using Server.Items;

namespace Server.Talent
{
    public class DragonWarmaster : BaseTalent
    {
        public DragonWarmaster()
        {
            DisplayName = "Dragon warmaster";
            Description = "Reduces damage while wearing dragon armor. Increases Str by 3 and reduces Int by 1 per Level";
            ImageID = 397;
            GumpHeight = 85;
            AddEndY = 80;
            UpdateOnEquip = true;
        }

        public override void UpdateMobile(Mobile mobile)
        {
            if (BaseArmor.FullDragon(mobile))
            {
                mobile.RemoveStatMod("DragonWarmasterStr");
                mobile.RemoveStatMod("DragonWarmasterInt");
                mobile.AddStatMod(new StatMod(StatType.Str, "ChainWarmasterStr", Level * 3, TimeSpan.Zero));
                mobile.AddStatMod(new StatMod(StatType.Int, "ChainWarmasterInt", -Level, TimeSpan.Zero));
            }
        }

        public override int CheckDamageAbsorptionEffect(Mobile defender, Mobile attacker, int damage)
        {
            if (BaseArmor.FullDragon(defender))
            {
                damage -= Level;
            }
            return damage;
        }
    }
}

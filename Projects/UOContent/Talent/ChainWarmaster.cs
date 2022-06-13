using System;
using Server.Items;

namespace Server.Talent
{
    public class ChainWarmaster : BaseTalent
    {
        public ChainWarmaster()
        {
            DisplayName = "Chain warmaster";
            Description = "Reduces damage while wearing chain or ringmail. Increases Str by 1 and Dex by 1 per Level";
            ImageID = 394;
            GumpHeight = 85;
            UpdateOnEquip = true;
            AddEndY = 80;
        }

        public override void UpdateMobile(Mobile mobile)
        {
            if (BaseArmor.FullChain(mobile) || BaseArmor.FullRing(mobile))
            {
                mobile.RemoveStatMod("ChainWarmasterStr");
                mobile.RemoveStatMod("ChainWarmasterDex");
                mobile.AddStatMod(new StatMod(StatType.Str, "ChainWarmasterStr", Level, TimeSpan.Zero));
                mobile.AddStatMod(new StatMod(StatType.Dex, "ChainWarmasterDex", Level, TimeSpan.Zero));
            }
        }

        public override int CheckDamageAbsorptionEffect(Mobile defender, Mobile attacker, int damage)
        {
            if (BaseArmor.FullChain(defender) || BaseArmor.FullRing(defender))
            {
                damage -= Level;
            }
            return damage;
        }
    }
}

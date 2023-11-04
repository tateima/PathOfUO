using System;
using Server.Items;

namespace Server.Talent
{
    public class ChainWarmaster : BaseTalent
    {
        public ChainWarmaster()
        {
            StatModNames = new[] { "ChainWarmasterStr", "ChainWarmasterDex" };
            DisplayName = "Chain warmaster";
            Description = "Unlocks chain and ring armour proficiency";
            AdditionalDetail = $"Reduces damage while wearing chain or ringmail. Increases Str by 2 and Dex by 2 per Level. {PassiveDetail}";
            ImageID = 394;
            GumpHeight = 85;
            UpdateOnEquip = true;
            AddEndY = 80;
        }

        public override void UpdateMobile(Mobile mobile)
        {
            ResetMobileMods(mobile);
            if (Items.BaseArmor.FullChain(mobile) || Items.BaseArmor.FullRing(mobile))
            {
                mobile.AddStatMod(new StatMod(StatType.Str, StatModNames[0], Level * 2, TimeSpan.Zero));
                mobile.AddStatMod(new StatMod(StatType.Dex, StatModNames[1], Level * 2, TimeSpan.Zero));
            }
        }

        public override int CheckDamageAbsorptionEffect(Mobile defender, Mobile attacker, int damage)
        {
            if (Items.BaseArmor.FullChain(defender) || Items.BaseArmor.FullRing(defender))
            {
                damage -= Level;
            }
            return damage;
        }
    }
}

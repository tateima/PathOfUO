using System;
using Server.Items;

namespace Server.Talent
{
    public class TwoHandedMaceSpecialist : BaseTalent
    {
        public TwoHandedMaceSpecialist()
        {
            TalentDependencies = new[] { typeof(MacefightingFocus) };
            RequiredWeapon = new[] { typeof(WarHammer), typeof(BaseStaff), typeof(Tessen), typeof(Tetsubo), typeof(Nunchaku) };
            DisplayName = "Warmonger";
            MaxLevel = 5;
            Description = "Increases damage and hit to two handed mace fighting weapons.";
            AdditionalDetail = $"{PassiveDetail} This talent increases damage of two handed mace weapons by 5% per level.";
            ImageID = 196;
            GumpHeight = 75;
            AddEndY = 85;
            IncreaseHitChance = true;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
            damage += AOS.Scale(damage, Level * 5);
            damage += AOS.Scale(damage, WeaponMasterModifier(attacker));
        }
    }
}

using System;
using Server.Items;

namespace Server.Talent
{
    public class SpearSpecialist : BaseTalent
    {
        public SpearSpecialist()
        {
            RequiredWeaponSkill = SkillName.Fencing;
            RequiredWeapon = new[] { typeof(Lance), typeof(Kama), typeof(Lajatang), typeof(Sai), typeof(Tekagi), typeof(BaseSpear) };
            IncreaseHitChance = true;
            TalentDependencies = new[] { typeof(FencingFocus) };
            MaxLevel = 5;
            DisplayName = "Spear specialist";
            Description = "Increases damage and hit chance of spear weapons.";
            AdditionalDetail = $"{PassiveDetail} The chance to hit and damage increases 5% per level for spear weapons.";
            ImageID = 197;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
            damage += AOS.Scale(damage, Level * 5);
            damage += AOS.Scale(damage, WeaponMasterModifier(attacker));
        }
    }
}

using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Talent
{
    public class SwordSpecialist : BaseTalent
    {
        public SwordSpecialist()
        {
            RequiredWeaponSkill = SkillName.Swords;
            RequiredWeapon = new[] { typeof(BaseSword) };
            TalentDependencies = new[] { typeof(SwordsmanshipFocus) };
            DisplayName = "Sword specialist";
            IncreaseHitChance = true;
            MaxLevel = 5;
            Description = "Increases damage and hit chance with sword weapons.";
            AdditionalDetail = $"{PassiveDetail} The chance to hit and damage increases 5% per level for sword weapons.";
            ImageID = 49;
            GumpHeight = 85;
            AddEndY = 80;
            AddEndAdditionalDetailsY = 80;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
            damage += AOS.Scale(damage, Level * 5);
            damage += AOS.Scale(damage, WeaponMasterModifier(attacker));
        }
    }
}

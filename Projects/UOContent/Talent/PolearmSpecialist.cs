using System;
using Server.Items;

namespace Server.Talent
{
    public class PolearmSpecialist : BaseTalent
    {
        public PolearmSpecialist()
        {
            TalentDependencies = new[] { typeof(SwordsmanshipFocus) };
            RequiredWeapon = new[] { typeof(BasePoleArm) };
            RequiredWeaponSkill = SkillName.Swords;
            IncreaseHitChance = true;
            MaxLevel = 5;
            DisplayName = "Polearm specialist";
            Description = "Increases damage and hit chance of pole arm weapons.";
            AdditionalDetail = $"{PassiveDetail} The chance to hit and damage increases 5% per level for pole arm weapons.";
            AddEndAdditionalDetailsY = 80;
            ImageID = 350;
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

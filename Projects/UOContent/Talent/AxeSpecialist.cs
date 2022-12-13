using Server.Items;

namespace Server.Talent
{
    public class AxeSpecialist : BaseTalent
    {
        public AxeSpecialist()
        {
            TalentDependencies = new[] { typeof(SwordsmanshipFocus) };
            RequiredWeaponSkill = SkillName.Swords;
            RequiredWeapon = new[] { typeof(BaseAxe) };
            IncreaseHitChance = true;
            MaxLevel = 5;
            DisplayName = "Axe specialist";
            Description = "Increases damage and hit chance of axe weapons.";
            AdditionalDetail = $"{PassiveDetail} The chance to hit and damage increases 5% per level for axe weapons.";
            AddEndAdditionalDetailsY = 80;
            ImageID = 122;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
            damage += AOS.Scale(damage, WeaponMasterModifier(attacker));
        }
    }
}

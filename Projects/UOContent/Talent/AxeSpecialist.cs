using Server.Items;

namespace Server.Talent
{
    public class AxeSpecialist : BaseTalent
    {
        public AxeSpecialist()
        {
            TalentDependency = typeof(SwordsmanshipFocus);
            RequiredWeaponSkill = SkillName.Swords;
            RequiredWeapon = new[] { typeof(BaseAxe) };
            IncreaseHitChance = true;
            DisplayName = "Axe specialist";
            Description = "Increases damage and hit chance of axe weapons.";
            AdditionalDetail = $"{PassiveDetail} The chance to hit increases 1% per level. This talent causes 1-X damage where X is the talent level.";
            AddEndAdditionalDetailsY = 80;
            ImageID = 122;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
            damage += Utility.RandomMinMax(1, Level);
        }
    }
}

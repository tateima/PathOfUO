using Server.Items;

namespace Server.Talent
{
    public class SwordSpecialist : BaseTalent
    {
        public SwordSpecialist()
        {
            RequiredWeaponSkill = SkillName.Swords;
            RequiredWeapon = new[] { typeof(BaseSword) };
            TalentDependency = typeof(SwordsmanshipFocus);
            DisplayName = "Sword specialist";
            IncreaseHitChance = true;
            Description = "Increases damage and hit chance with sword weapons.";
            AdditionalDetail = $"{PassiveDetail} The chance to hit increases 1% per level. This talent causes 1-X damage where X is the talent level.";
            ImageID = 49;
            GumpHeight = 85;
            AddEndY = 80;
            AddEndAdditionalDetailsY = 80;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
            damage += Utility.RandomMinMax(1, Level);
        }
    }
}

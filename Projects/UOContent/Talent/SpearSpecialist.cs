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
            TalentDependency = typeof(FencingFocus);
            DisplayName = "Spear specialist";
            Description = "Increases damage and hit chance of spear weapons.";
            AdditionalDetail = $"{PassiveDetail} The chance to hit increases 1% per level. This talent causes (1-X) * 2 damage where X is the talent level.";
            ImageID = 197;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
            // 2 damage per point because 2H
            damage += Utility.RandomMinMax(1, Level) * 2;
        }
    }
}

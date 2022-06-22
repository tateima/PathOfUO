using Server.Items;

namespace Server.Talent
{
    public class PolearmSpecialist : BaseTalent
    {
        public PolearmSpecialist()
        {
            TalentDependency = typeof(SwordsmanshipFocus);
            RequiredWeapon = new[] { typeof(BasePoleArm) };
            RequiredWeaponSkill = SkillName.Swords;
            IncreaseHitChance = true;
            DisplayName = "Polearm specialist";
            Description = "Increases damage and hit chance of pole arm weapons.";
            AdditionalDetail = $"{PassiveDetail} The chance to hit increases 1% per level. This talent causes (1-X) * 2 damage where X is the talent level.";
            AddEndAdditionalDetailsY = 80;
            ImageID = 350;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            target.Damage(Utility.RandomMinMax(1, Level) * 2, attacker);
        }
    }
}

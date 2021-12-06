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
            Description = "Increases damage and hit chance of polearm weapons.";
            ImageID = 350;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            target.Damage(Level * 2, attacker);
        }
    }
}

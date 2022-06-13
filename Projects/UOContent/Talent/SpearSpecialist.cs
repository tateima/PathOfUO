using Server.Items;

namespace Server.Talent
{
    public class SpearSpecialist : BaseTalent
    {
        public SpearSpecialist()
        {
            RequiredWeaponSkill = SkillName.Fencing;
            RequiredWeapon = new[] { typeof(BaseSpear) };
            IncreaseHitChance = true;
            TalentDependency = typeof(FencingFocus);
            DisplayName = "Spear specialist";
            Description = "Increases damage and hit chance of spear weapons.";
            ImageID = 197;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            // 2 damage per point because 2H
            target.Damage(Utility.RandomMinMax(1, Level) * 2, attacker);
        }
    }
}

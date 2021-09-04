using Server.Items;
using System;

namespace Server.Talent
{
    public class SpearSpecialist : BaseTalent, ITalent
    {
        public SpearSpecialist() : base()
        {
            RequiredWeaponSkill = SkillName.Fencing;
            RequiredWeapon = new Type[] { typeof(BaseSpear) };
            IncreaseHitChance = true;
            TalentDependency = typeof(FencingFocus);
            DisplayName = "Spear specialist";
            Description = "Increases damage and hit chance of spear weapons.";
            ImageID = 30230;
        }
        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            // 2 damage per point because 2H
            target.Damage(Level * 2, attacker);
        }
    }
}

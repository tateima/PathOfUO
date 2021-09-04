using Server.Items;
using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class PolearmSpecialist : BaseTalent, ITalent
    {
        public PolearmSpecialist() : base()
        {
            TalentDependency = typeof(SwordsmanshipFocus);
            RequiredWeapon = new Type[] { typeof(BasePoleArm) };
            RequiredWeaponSkill = SkillName.Swords;
            IncreaseHitChance = true;
            DisplayName = "Polearm specialist";
            Description = "Increases damage and hit chance of polearm weapons.";
            ImageID = 30038;
        }
        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            target.Damage(Level * 2, attacker);
        }
    }
}

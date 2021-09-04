using System;
using Server.Items;

namespace Server.Talent
{
    public class CrossbowSpecialist : BaseTalent, ITalent
    {
        public CrossbowSpecialist() : base()
        {
            TalentDependency = typeof(ArcherFocus);
            RequiredWeapon = new Type[] { typeof(Crossbow), typeof(HeavyCrossbow), typeof(RepeatingCrossbow) };
            RequiredWeaponSkill = SkillName.Archery;
            IncreaseHitChance = true;
            DisplayName = "Crossbow specialist";
            Description = "Increases damage and hit chance of crossbow weapons.";
            ImageID = 39899;
        }
        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            target.Damage(Level, attacker);
        }
    }
}

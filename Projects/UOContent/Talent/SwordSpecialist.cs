using Server.Mobiles;
using Server.Items;
using System;

namespace Server.Talent
{
    public class SwordSpecialist : BaseTalent, ITalent
    {
        public SwordSpecialist() : base()
        {
            RequiredWeaponSkill = SkillName.Swords;
            RequiredWeapon = new Type[] { typeof(BaseSword) };
            TalentDependency = typeof(SwordsmanshipFocus);
            DisplayName = "Sword specialist";
            IncreaseHitChance = true;
            Description = "Increases damage and hit chance of sword weapons.";
            ImageID = 49;
            GumpHeight = 85;
            AddEndY = 80;
        }
        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            target.Damage(Level, attacker);
        }
    }
}

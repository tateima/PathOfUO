using System;
using Server.Items;

namespace Server.Talent
{
    public class AxeSpecialist : BaseTalent, ITalent
    {
        public AxeSpecialist() : base()
        {
            TalentDependency = typeof(SwordsmanshipFocus);
            RequiredWeaponSkill = SkillName.Swords;
            RequiredWeapon = new Type[] { typeof(BaseAxe) };
            IncreaseHitChance = true;
            DisplayName = "Axe specialist";
            Description = "Increases damage and hit chance of axe weapons.";
            ImageID = 122;
            GumpHeight = 85;
            AddEndY = 80;
        }
        
        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            target.Damage(Level, attacker);
        }

    }
}

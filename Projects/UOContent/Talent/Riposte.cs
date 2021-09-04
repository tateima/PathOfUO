using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class Riposte : BaseTalent, ITalent
    {
        public Riposte() : base()
        {
            RequiredWeaponSkill = SkillName.Fencing;
            TalentDependency = typeof(FencingFocus);
            DisplayName = "Riposte";
            Description = "Chance to deal damage to them if enemy misses.";
            ImageID = 30221;
        }
        public virtual void CheckDefenderMissEffect(Mobile attacker, Mobile defender)
        {
            // 5% chance
            if (Utility.Random(100) < Level)
            {
                // max 10 damage (2 per level)
                attacker.Damage(Level * 2, defender);
            }
        }
    }
}

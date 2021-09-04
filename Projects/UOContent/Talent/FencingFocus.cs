using Server.Mobiles;
using Server.Items;
using System;

namespace Server.Talent
{
    public class FencingFocus : BaseTalent, ITalent
    {
        public FencingFocus() : base()
        {
            BlockedBy = new Type[] { typeof(ArcherFocus) };
            RequiredWeaponSkill = SkillName.Fencing;
            DisplayName = "Fencing focus";
            Description = "Chance of getting a critical strike with fencing weapons.";
            ImageID = 30199;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (Utility.Random(100) < Level)
            {
                CriticalStrike(attacker, target, damage);
            }
        }

    }
}

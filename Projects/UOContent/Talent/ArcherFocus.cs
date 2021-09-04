using System;
using Server.Items; 

namespace Server.Talent
{
    public class ArcherFocus : BaseTalent, ITalent
    {
        public ArcherFocus() : base()
        {
            BlockedBy = new Type[] { typeof(MacefightingFocus), typeof(SwordsmanshipFocus), typeof(FencingFocus) };
            RequiredWeaponSkill = SkillName.Archery;
            RequiredWeapon = new Type[] { typeof(BaseRanged) };
            DisplayName = "Archer focus";
            Description = "Chance of getting a critical strike with ranged weapons.";
            ImageID = 39880;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (Utility.Random(100) < Level)
            {
                // double damage crit so damage them with the same damage again
                CriticalStrike(attacker, target, damage);
            }
        }

    }
}

using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class MacefightingFocus : BaseTalent, ITalent
    {
        public MacefightingFocus() : base()
        {
            BlockedBy = new Type[] { typeof(ArcherFocus) };
            RequiredWeaponSkill = SkillName.Macing;
            DisplayName = "Macefighting focus";
            Description = "Chance of getting a critical strike with macing weapons.";
            ImageID = 172;
            GumpHeight = 85;
            AddEndY = 80;
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

using Server.Items;
using System;

namespace Server.Talent
{
    public class SwordsmanshipFocus : BaseTalent, ITalent
    {
        public SwordsmanshipFocus() : base()
        {
            RequiredWeaponSkill = SkillName.Swords;
            RequiredWeapon = new Type[] { typeof(BaseSword) };
            BlockedBy = new Type[] { typeof(ArcherFocus) };
            DisplayName = "Swordsmanship focus";
            Description = "Chance of getting a critical strike with sword weapons.";
            ImageID = 39862;
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

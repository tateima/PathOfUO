using Server.Items;

namespace Server.Talent
{
    public class ArcherFocus : BaseTalent
    {
        public ArcherFocus()
        {
            BlockedBy = new[] { typeof(MacefightingFocus), typeof(SwordsmanshipFocus), typeof(FencingFocus) };
            RequiredWeaponSkill = SkillName.Archery;
            RequiredWeapon = new[] { typeof(BaseRanged) };
            DisplayName = "Archer focus";
            Description = "Chance of getting a critical strike with ranged weapons.";
            AdditionalDetail = $"{CriticalDamageDetail} The chance increases 1% per level and applies to any weapon that requires archery.";
            ImageID = 114;
            GumpHeight = 75;
            AddEndY = 100;
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

using Server.Items;

namespace Server.Talent
{
    public class ArcherFocus : BaseTalent
    {
        public ArcherFocus()
        {
            RequiredWeaponSkill = SkillName.Archery;
            RequiredWeapon = new[] { typeof(BaseRanged) };
            DisplayName = "Archer focus";
            Description = "Unlocks weapon proficiencies with bow and crossbows.";
            AdditionalDetail = $"Can now use bows and crossbows. Chance of getting a critical strike with ranged weapons. {CriticalDamageDetail} The chance increases 1% per level and applies to any weapon that requires archery.";
            ImageID = 114;
            GumpHeight = 75;
            AddEndY = 95;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
            if (Utility.Random(100) < Level)
            {
               CriticalStrike(target, attacker, ref damage);
            }
        }
    }
}

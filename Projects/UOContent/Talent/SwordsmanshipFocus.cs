using Server.Items;

namespace Server.Talent
{
    public class SwordsmanshipFocus : BaseTalent
    {
        public SwordsmanshipFocus()
        {
            RequiredWeaponSkill = SkillName.Swords;
            DisplayName = "Swordsman focus";
            Description = "Unlocks weapon proficiencies with sword weapons.";
            AdditionalDetail =
                $"Can now use sword weapons. Chance of getting a critical strike with sword weapons. {CriticalDamageDetail} The chance increases 1% per level and applies to any weapon that requires swordsmanship.";
            AddEndAdditionalDetailsY = 70;
            ImageID = 133;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
            if (Utility.Random(100) < Level)
            {
                // critical damage them with the same damage again
                CriticalStrike(target, attacker, ref damage);
            }
        }
    }
}

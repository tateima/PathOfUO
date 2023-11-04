namespace Server.Talent
{
    public class MacefightingFocus : BaseTalent
    {
        public MacefightingFocus()
        {
            RequiredWeaponSkill = SkillName.Macing;
            DisplayName = "Macefighting focus";
            Description = "Unlocks weapon proficiencies with mace fighting weapons.";
            AdditionalDetail = $"Can now use mace weapons. Chance of getting a critical strike with macing weapons. {CriticalDamageDetail} The chance increases 1% per level and applies to any weapon that requires mace fighting.";
            AddEndAdditionalDetailsY = 80;
            ImageID = 172;
            GumpHeight = 80;
            AddEndY = 80;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
            if (Utility.Random(100) < Level)
            {
                CriticalStrike(ref damage);
            }
        }
    }
}

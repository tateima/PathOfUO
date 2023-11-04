namespace Server.Talent
{
    public class FencingFocus : BaseTalent
    {
        public FencingFocus()
        {
            RequiredWeaponSkill = SkillName.Fencing;
            DisplayName = "Fencing focus";
            Description = "Unlocks weapon proficiencies with fencing weapons.";
            AdditionalDetail = $"Can now use fencing weapons. Chance of getting a critical strike with fencing weapons. {CriticalDamageDetail} The chance increases 1% per level and applies to any weapon that requires fencing.";
            ImageID = 345;
            GumpHeight = 85;
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

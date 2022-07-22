namespace Server.Talent
{
    public class FencingFocus : BaseTalent
    {
        public FencingFocus()
        {
            BlockedBy = new[] { typeof(ArcherFocus) };
            RequiredWeaponSkill = SkillName.Fencing;
            DisplayName = "Fencing focus";
            Description = "Chance of getting a critical strike with fencing weapons.";
            AdditionalDetail = $"{CriticalDamageDetail} The chance increases 1% per level and applies to any weapon that requires fencing.";
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

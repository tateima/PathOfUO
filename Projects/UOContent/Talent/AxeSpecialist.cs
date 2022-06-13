using Server.Items;

namespace Server.Talent
{
    public class AxeSpecialist : BaseTalent
    {
        public AxeSpecialist()
        {
            TalentDependency = typeof(SwordsmanshipFocus);
            RequiredWeaponSkill = SkillName.Swords;
            RequiredWeapon = new[] { typeof(BaseAxe) };
            IncreaseHitChance = true;
            DisplayName = "Axe specialist";
            Description = "Increases damage and hit chance of axe weapons.";
            ImageID = 122;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            target.Damage(Utility.RandomMinMax(1, Level), attacker);
        }
    }
}

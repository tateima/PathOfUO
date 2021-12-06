using Server.Items;

namespace Server.Talent
{
    public class SwordSpecialist : BaseTalent
    {
        public SwordSpecialist()
        {
            RequiredWeaponSkill = SkillName.Swords;
            RequiredWeapon = new[] { typeof(BaseSword) };
            TalentDependency = typeof(SwordsmanshipFocus);
            DisplayName = "Sword specialist";
            IncreaseHitChance = true;
            Description = "Increases damage and hit chance of sword weapons.";
            ImageID = 49;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            target.Damage(Level, attacker);
        }
    }
}

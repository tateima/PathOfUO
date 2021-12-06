namespace Server.Talent
{
    public class FencingSpecialist : BaseTalent
    {
        public FencingSpecialist()
        {
            TalentDependency = typeof(FencingFocus);
            RequiredWeaponSkill = SkillName.Fencing;
            IncreaseHitChance = true;
            DisplayName = "Fencing specialist";
            Description = "Increases damage and hit chance of one handed fencing weapons.";
            ImageID = 346;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            target.Damage(Level, attacker);
        }
    }
}

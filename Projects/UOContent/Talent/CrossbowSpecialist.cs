using Server.Items;

namespace Server.Talent
{
    public class CrossbowSpecialist : BaseTalent
    {
        public CrossbowSpecialist()
        {
            TalentDependency = typeof(ArcherFocus);
            RequiredWeapon = new[] { typeof(Crossbow), typeof(HeavyCrossbow), typeof(RepeatingCrossbow) };
            RequiredWeaponSkill = SkillName.Archery;
            IncreaseHitChance = true;
            DisplayName = "Crossbow specialist";
            Description = "Increases damage and hit chance of crossbow weapons.";
            AdditionalDetail = $"{PassiveDetail} The chance to hit increases 1% per level. This talent causes 1-X damage where X is the talent level.";
            ImageID = 152;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            target.Damage(Utility.RandomMinMax(1, Level), attacker);
        }
    }
}

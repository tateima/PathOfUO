using Server.Items;

namespace Server.Talent
{
    public class CrossbowSpecialist : BaseTalent
    {
        public CrossbowSpecialist()
        {
            TalentDependencies = new[] { typeof(ArcherFocus) };
            RequiredWeapon = new[] { typeof(Crossbow), typeof(HeavyCrossbow), typeof(RepeatingCrossbow) };
            RequiredWeaponSkill = SkillName.Archery;
            IncreaseHitChance = true;
            MaxLevel = 5;
            DisplayName = "Crossbow specialist";
            Description = "Increases damage and hit chance of crossbow weapons.";
            AdditionalDetail = $"{PassiveDetail} The chance to hit and damage increases 5% per level for crossbow weapons.";
            ImageID = 152;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
            damage += AOS.Scale(damage, Level * 5);
            damage += AOS.Scale(damage, WeaponMasterModifier(attacker));
        }
    }
}

using Server.Items;

namespace Server.Talent
{
    public class FencingSpecialist : BaseTalent
    {
        public FencingSpecialist()
        {
            TalentDependencies = new[] { typeof(FencingFocus) };
            RequiredWeaponSkill = SkillName.Fencing;
            RequiredWeapon = new[]
            {
                typeof(Kryss), typeof(Dagger), typeof(AssassinSpike), typeof(Leafblade), typeof(WarCleaver), typeof(BloodBlade)
            };
            IncreaseHitChance = true;
            MaxLevel = 5;
            DisplayName = "Fencing specialist";
            Description = "Increases damage and hit chance of one handed fencing weapons.";
            AdditionalDetail = $"{PassiveDetail} This talent increases damage of one handed fencing weapons by 5% per level.";
            ImageID = 346;
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

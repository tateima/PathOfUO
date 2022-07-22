using Server.Items;

namespace Server.Talent
{
    public class FencingSpecialist : BaseTalent
    {
        public FencingSpecialist()
        {
            TalentDependency = typeof(FencingFocus);
            RequiredWeaponSkill = SkillName.Fencing;
            RequiredWeapon = new[]
            {
                typeof(Kryss), typeof(Dagger), typeof(AssassinSpike), typeof(Leafblade), typeof(WarCleaver), typeof(BloodBlade)
            };
            IncreaseHitChance = true;
            DisplayName = "Fencing specialist";
            Description = "Increases damage and hit chance of one handed fencing weapons.";
            AdditionalDetail = $"{PassiveDetail} This talent causes (1-X) damage where X is the talent level.";
            ImageID = 346;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
            damage += Utility.RandomMinMax(1, Level);
        }
    }
}

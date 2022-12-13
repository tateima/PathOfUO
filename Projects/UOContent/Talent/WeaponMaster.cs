using Server.Items;

namespace Server.Talent
{
    public class WeaponMaster : BaseTalent
    {
        public WeaponMaster()
        {
            TalentDependencies = new[]
            {
                typeof(MaceSpecialist),
                typeof(SwordSpecialist),
                typeof(AxeSpecialist),
                typeof(SpearSpecialist),
                typeof(TwoHandedMaceSpecialist),
                typeof(FencingSpecialist),
                typeof(CrossbowSpecialist),
                typeof(BowSpecialist),
                typeof(PolearmSpecialist)
            };
            TalentDependencyMinimum = 3;
            RequiredWeapon = new[] { typeof(BaseWeapon) };
            DisplayName = "Weapon Master";
            Description = "Increases damage of all specialist weapon types.";
            AdditionalDetail = $"{PassiveDetail} This talent increases damage of any weapons you are specialising in by 10% per level.";
            ImageID = 428;
            MaxLevel = 2;
            GumpHeight = 75;
            AddEndY = 80;
            IncreaseHitChance = true;
        }
    }
}

using Server.Items;

namespace Server.Talent
{
    public class TwoHandedMaceSpecialist : BaseTalent
    {
        public TwoHandedMaceSpecialist()
        {
            TalentDependency = typeof(MacefightingFocus);
            RequiredWeapon = new[] { typeof(WarHammer), typeof(BaseStaff) };
            DisplayName = "Warmonger";
            Description = "Increases damage to two handed mace fighting weapons.";
            AdditionalDetail = $"{PassiveDetail} This talent causes (1-X) * 2 damage where X is the talent level.";
            ImageID = 196;
            GumpHeight = 75;
            AddEndY = 85;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            target.Damage(Utility.RandomMinMax(1, Level) * 2, attacker);
        }
    }
}

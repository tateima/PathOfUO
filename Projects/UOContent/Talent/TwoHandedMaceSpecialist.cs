using Server.Items;

namespace Server.Talent
{
    public class TwoHandedMaceSpecialist : BaseTalent
    {
        public TwoHandedMaceSpecialist()
        {
            TalentDependency = typeof(MacefightingFocus);
            RequiredWeapon = new[] { typeof(WarHammer) };
            DisplayName = "Warmonger";
            Description = "Increases damage to two handed mace fighting weapons.";
            ImageID = 196;
            GumpHeight = 75;
            AddEndY = 85;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            target.Damage(Level * 2, attacker);
        }
    }
}

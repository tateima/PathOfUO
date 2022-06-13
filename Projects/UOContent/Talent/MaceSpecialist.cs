using Server.Items;

namespace Server.Talent
{
    public class MaceSpecialist : BaseTalent
    {
        public MaceSpecialist()
        {
            BlockedBy = new[] { typeof(TwoHandedMaceSpecialist) };
            TalentDependency = typeof(MacefightingFocus);
            RequiredWeapon = new[]
            {
                typeof(Mace), typeof(Maul), typeof(Club), typeof(DiamondMace), typeof(MagicWand), typeof(HammerPick),
                typeof(Scepter), typeof(WarMace)
            };
            RequiredWeaponSkill = SkillName.Macing;
            DisplayName = "Mace specialist";
            Description = "Increases damage to one handed macefighting weapons.";
            ImageID = 181;
            AddEndY = 90;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            target.Damage(Utility.RandomMinMax(1, Level), attacker);
        }
    }
}

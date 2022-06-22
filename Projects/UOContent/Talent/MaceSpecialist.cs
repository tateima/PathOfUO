using Server.Items;

namespace Server.Talent
{
    public class MaceSpecialist : BaseTalent
    {
        public MaceSpecialist()
        {
            TalentDependency = typeof(MacefightingFocus);
            RequiredWeapon = new[]
            {
                typeof(Mace), typeof(Maul), typeof(Club), typeof(DiamondMace), typeof(MagicWand), typeof(HammerPick),
                typeof(Scepter), typeof(WarMace)
            };
            RequiredWeaponSkill = SkillName.Macing;
            DisplayName = "Mace specialist";
            IncreaseHitChance = true;
            Description = "Increases damage and hit chance to one handed mace fighting weapons.";
            AdditionalDetail = $"{PassiveDetail} The chance to hit increases 1% per level. This talent causes 1-X damage where X is the talent level.";
            ImageID = 181;
            AddEndY = 90;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            target.Damage(Utility.RandomMinMax(1, Level), attacker);
        }
    }
}

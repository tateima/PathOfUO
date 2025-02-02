using Server.Items;

namespace Server.Talent
{
    public class MaceSpecialist : BaseTalent
    {
        public MaceSpecialist()
        {
            TalentDependencies = new[] { typeof(MacefightingFocus) };
            RequiredWeapon = new[]
            {
                typeof(Mace), typeof(Maul), typeof(Club), typeof(DiamondMace), typeof(MagicWand), typeof(HammerPick),
                typeof(Scepter), typeof(WarMace)
            };
            RequiredWeaponSkill = SkillName.Macing;
            DisplayName = "Mace specialist";
            IncreaseHitChance = true;
            MaxLevel = 5;
            Description = "Increases damage and hit to one handed mace fighting weapons.";
            AdditionalDetail = $"{PassiveDetail} The chance to hit and damage increases 5% per level for one handed macing weapons.";
            ImageID = 181;
            AddEndY = 90;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
            damage += AOS.Scale(damage, Level * 5);
            damage += AOS.Scale(damage, WeaponMasterModifier(attacker));
        }
    }
}

using Server.Items;

namespace Server.Talent
{
    public class BowSpecialist : BaseTalent
    {
        public BowSpecialist()
        {
            TalentDependency = typeof(ArcherFocus);
            RequiredWeapon = new[]
            {
                typeof(Bow), typeof(CompositeBow), typeof(LongbowOfMight), typeof(JukaBow), typeof(SlayerLongbow),
                typeof(RangersShortbow), typeof(LightweightShortbow), typeof(FrozenLongbow), typeof(BarbedLongbow),
                typeof(AssassinsShortbow)
            };
            RequiredWeaponSkill = SkillName.Archery;
            IncreaseHitChance = true;
            DisplayName = "Bow specialist";
            Description = "Increases damage and hit chance of bow weapons.";
            AdditionalDetail = $"{PassiveDetail} The chance to hit increases 1% per level. This talent causes 1-X damage where X is the talent level.";
            ImageID = 131;
            GumpHeight = 85;
            AddEndY = 75;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
            damage += Utility.RandomMinMax(1, Level);
        }
    }
}

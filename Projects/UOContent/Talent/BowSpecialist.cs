using System;
using Server.Items;

namespace Server.Talent
{
    public class BowSpecialist : BaseTalent
    {
        public BowSpecialist()
        {
            TalentDependencies = new[] { typeof(ArcherFocus) };
            RequiredWeapon = new[]
            {
                typeof(Bow), typeof(CompositeBow), typeof(LongbowOfMight), typeof(JukaBow), typeof(SlayerLongbow),
                typeof(RangersShortbow), typeof(LightweightShortbow), typeof(FrozenLongbow), typeof(BarbedLongbow),
                typeof(AssassinsShortbow)
            };
            RequiredWeaponSkill = SkillName.Archery;
            IncreaseHitChance = true;
            MaxLevel = 5;
            DisplayName = "Bow specialist";
            Description = "Increases damage and hit chance of bow weapons.";
            AdditionalDetail = $"{PassiveDetail} The chance to hit and damage increases 5% per level for bow weapons.";
            ImageID = 131;
            GumpHeight = 85;
            AddEndY = 75;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
            damage += AOS.Scale(damage, Level * 5);
            damage += AOS.Scale(damage, WeaponMasterModifier(attacker));
        }
    }
}

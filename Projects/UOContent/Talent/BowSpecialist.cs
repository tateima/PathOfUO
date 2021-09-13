using System;
using Server.Items;

namespace Server.Talent
{
    public class BowSpecialist : BaseTalent, ITalent
    {
        public BowSpecialist() : base()
        {
            TalentDependency = typeof(ArcherFocus);
            RequiredWeapon = new Type[] { typeof(Bow), typeof(CompositeBow), typeof(LongbowOfMight), typeof(JukaBow), typeof(SlayerLongbow), typeof(RangersShortbow), typeof(LightweightShortbow), typeof(FrozenLongbow), typeof(BarbedLongbow), typeof(AssassinsShortbow) };
            RequiredWeaponSkill = SkillName.Archery;
            IncreaseHitChance = true;
            DisplayName = "Bow specialist";
            Description = "Increases damage and hit chance of bow weapons.";
            ImageID = 131;
            GumpHeight = 85;
            AddEndY = 80;
        }
        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            target.Damage(Level, attacker);
        }
    }
}

using Server.Items;
using Server.Mobiles;

namespace Server.Talent
{
    public class Riposte : BaseTalent
    {
        public Riposte()
        {
            RequiredWeaponSkill = SkillName.Fencing;
            TalentDependencies = new[] { typeof(FencingFocus) };
            DisplayName = "Riposte";
            StamRequired = 3;
            Description = "Chance to deal damage to them if enemy misses.";
            AdditionalDetail = $"This chance increases by 5% per level and will damage the enemy for level * 2 damage. {PassiveDetail}";
            ImageID = 339;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override void CheckDefenderMissEffect(Mobile attacker, Mobile target)
        {
            int modifier = Level * 2;
            if (target.Weapon is BaseMeleeWeapon targetWeapon && target.Stam >= StamRequired + 1 && Utility.Random(100) < modifier)
            {
                ApplyStaminaCost(target);
                if (targetWeapon.CheckHit(target, attacker))
                {
                    attacker.PlaySound(0x235);
                    AlterDamage(attacker, (PlayerMobile)target, ref modifier);
                    attacker.Damage(modifier, target);
                }
            }

        }
    }
}

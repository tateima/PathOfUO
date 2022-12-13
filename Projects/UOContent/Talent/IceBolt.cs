using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Talent
{
    public class IceBolt : BaseTalent
    {

        public IceBolt()
        {
            TalentDependencies = new[] { typeof(CrossbowSpecialist) };
            RequiredWeapon = new[] { typeof(Crossbow), typeof(HeavyCrossbow), typeof(RepeatingCrossbow) };
            RequiredWeaponSkill = SkillName.Archery;
            CanBeUsed = true;
            ManaRequired = 15;
            CooldownSeconds = 120;
            DisplayName = "Ice bolt";
            Description = "Fire bolt of ice from crossbow that slows target and does minor cold damage.";
            AdditionalDetail = "The slow lasts for 5 seconds per level and will apply an ice damage modifier to the target.";
            ImageID = 378;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
            if (Activated && attacker.Mana >= ManaRequired)
            {
                if (target is BaseCreature creature)
                {
                    SlowCreature(creature, Level * 5, true);
                    IceDamage(attacker, target, damage, 1);
                }

                if (target is PlayerMobile player)
                {
                    IceDamage(attacker, target, damage, 2);
                    player.Slow(Level);
                }

                if (target is PlayerMobile or BaseCreature)
                {
                    target.PlaySound(0x5C7);
                    Activated = false;
                    OnCooldown = true;
                    ApplyManaCost(attacker);
                    Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
                }
            }
        }

        public void IceDamage(Mobile attacker, Mobile target, int damage, int modifier)
        {
            AlterDamage(target, (PlayerMobile)attacker, ref damage);
            if (Core.AOS)
            {
                AOS.Damage(target, AOS.Scale(damage, Level * modifier), 0, 0, 100, 0, 0);
            }
            else
            {
                target.Damage(AOS.Scale(damage, Level * modifier), attacker);
            }
        }
    }
}

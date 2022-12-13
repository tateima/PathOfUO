using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;

namespace Server.Talent
{
    public class GroundSlam : BaseTalent
    {
        public GroundSlam()
        {
            TalentDependencies = new[] { typeof(TwoHandedMaceSpecialist) };
            RequiredWeapon = new[] { typeof(WarHammer) , typeof(BaseStaff) };
            DisplayName = "Ground slam";
            CanBeUsed = true;
            Description = "Push back surrounding mobiles by 1-5 yards and slows them down.";
            AdditionalDetail = "The slow effect lasts 10 seconds.";
            ImageID = 369;
            CooldownSeconds = 30;
            GumpHeight = 75;
            AddEndY = 80;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
            if (Activated)
            {
                Activated = false;
                OnCooldown = true;
                var mobiles = attacker.GetMobilesInRange(3);
                foreach (var mobile in mobiles)
                {
                    if (mobile == attacker || !mobile.CanBeHarmful(attacker, false) ||
                        Core.AOS && !mobile.InLOS(attacker))
                    {
                        continue;
                    }

                    var mobileLocation = mobile.Location;
                    var newMobileLocation = CalculatePushbackFromAnchor(mobileLocation, Level, mobile);
                    var attempts = 10;
                    while (!mobile.InLOS(newMobileLocation))
                    {
                        if (attempts > 10)
                        {
                            break;
                        }
                        newMobileLocation = CalculatePushbackFromAnchor(mobileLocation, 1, mobile);
                        attempts++;
                    }

                    mobile.MoveToWorld(newMobileLocation, mobile.Map);
                    int slamDamage = (mobile == target) ? damage + Level : Level;
                    AlterDamage(mobile, (PlayerMobile)attacker, ref slamDamage);
                    mobile.Damage(damage, attacker);

                    if (mobile is BaseCreature creature)
                    {
                        SlowCreature(creature, 0, false);
                    }
                    else if (mobile is PlayerMobile player)
                    {
                        player.Slow(Utility.Random(10));
                    }
                }
                attacker.PlaySound(0x11E);
                Timer.StartTimer(TimeSpan.FromSeconds(10), ExpireSlowEffect, out _);
                Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
            }
        }

        public override void OnUse(Mobile from)
        {
            var weapon = from.Weapon as BaseWeapon;
            if (weapon?.Skill == RequiredWeaponSkill && weapon is WarHammer)
            {
                base.OnUse(from);
            }
            else
            {
                from.SendMessage("You do not have a war hammer equipped.");
            }
        }
    }
}

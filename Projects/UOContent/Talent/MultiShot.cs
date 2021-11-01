using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Talent
{
    public class MultiShot : BaseTalent, ITalent
    {
        public MultiShot() : base()
        {
            TalentDependency = typeof(BowSpecialist);
            RequiredWeapon = new Type[] { typeof(Bow), typeof(CompositeBow), typeof(LongbowOfMight), typeof(JukaBow), typeof(SlayerLongbow), typeof(RangersShortbow), typeof(LightweightShortbow), typeof(FrozenLongbow), typeof(BarbedLongbow), typeof(AssassinsShortbow) };
            RequiredWeaponSkill = SkillName.Archery;
            CanBeUsed = true;
            DisplayName = "Multi shot";
            Description = "Shoot between 1-5 arrows to nearby enemies. 2 minute cooldown.";
            ImageID = 377;
            GumpHeight = 85;
            AddEndY = 85;
        }
        public void DoShot(Mobile attacker, Mobile target)
        {

            int numberOfShots = 0;
            if (attacker.Weapon is BaseRanged bow && CanApplyHitEffect((Item)attacker.Weapon)) {
                foreach (Mobile mobile in target.GetMobilesInRange(8))
                {
                    if (mobile == attacker || (mobile is PlayerMobile && mobile.Karma > 0) || !mobile.CanBeHarmful(attacker, false) ||
                               Core.AOS && !mobile.InLOS(attacker))
                    {
                        continue;
                    }
                    bool running = attacker.Direction == Direction.Running;
                    if (attacker.GetDirectionTo(mobile.X, mobile.Y, running) == attacker.Direction)
                    {
                        if (bow.OnFired(attacker, mobile))
                        {
                            if (bow.CheckHit(attacker, mobile))
                            {
                                bow.OnHit(attacker, mobile);
                            }
                            else
                            {
                                bow.OnMiss(attacker, mobile);
                            }
                        }
                        numberOfShots++;
                        if (numberOfShots == Level)
                        {
                            OnCooldown = true;
                            Activated = false;
                            Timer.StartTimer(TimeSpan.FromSeconds(120), ExpireTalentCooldown, out _talentTimerToken);
                            break;
                        }
                    }
                }
            }
            
        }
    }
}

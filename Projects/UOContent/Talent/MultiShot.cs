using System;
using System.Linq;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Talent
{
    public class MultiShot : BaseTalent
    {
        public MultiShot()
        {
            TalentDependency = typeof(BowSpecialist);
            RequiredWeapon = new[]
            {
                typeof(Bow), typeof(CompositeBow), typeof(LongbowOfMight), typeof(JukaBow), typeof(SlayerLongbow),
                typeof(RangersShortbow), typeof(LightweightShortbow), typeof(FrozenLongbow), typeof(BarbedLongbow),
                typeof(AssassinsShortbow)
            };
            RequiredWeaponSkill = SkillName.Archery;
            CanBeUsed = true;
            ManaRequired = 30;
            CooldownSeconds = 120;
            DisplayName = "Multi shot";
            Description = "Shoot between 1-6 arrows to nearby enemies. 2 minute cooldown.";
            AdditionalDetail = "The arrow number increases by 1 per level.";
            ImageID = 377;
            GumpHeight = 85;
            AddEndY = 85;
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown && HasSkillRequirement(from))
            {
                if (from.Mana < ManaRequired)
                {
                    from.SendMessage("Multi shot requires 30 mana to use.");
                }
                else
                {
                    from.Target = new InternalTarget(this, from);
                }
            }
            else
            {
                from.SendMessage(FailedRequirements);
            }
        }

        public void DoShot(Mobile attacker, Mobile target)
        {
            var numberOfShots = 0;
            var maxShots = Level + 1;
            if (attacker.Weapon is BaseRanged bow && CanApplyHitEffect(bow))
            {
                var ammoItems = attacker.Backpack?.FindItemsByType(bow.AmmoType);
                var ammoCount = 0;
                if (ammoItems != null)
                {
                    ammoCount += ammoItems.Sum(item => item.Amount);
                }

                if (ammoCount < maxShots)
                {
                    maxShots = ammoCount;
                }
                foreach (var mobile in target.GetMobilesInRange(8))
                {
                    if (mobile == attacker || mobile is PlayerMobile && mobile.Karma > 0 ||
                        !mobile.CanBeHarmful(attacker, false) ||
                        Core.AOS && !mobile.InLOS(attacker))
                    {
                        continue;
                    }
                    if (attacker.InLOS(mobile))
                    {
                        if (bow.OnFired(attacker, mobile))
                        {
                            if (bow.CheckHit(attacker, mobile))
                            {
                                bow.OnHit(attacker, mobile, 0.5); // 50% damage otherwise its too OP
                            }
                            else
                            {
                                bow.OnMiss(attacker, mobile);
                            }
                        }

                        numberOfShots++;
                        if (numberOfShots > maxShots)
                        {
                            ApplyManaCost(attacker);
                            OnCooldown = true;
                            Activated = false;
                            Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
                            break;
                        }
                    }
                }
            }
        }

        private class InternalTarget : Target
        {
            private readonly MultiShot _multiShot;
            private readonly Mobile _attacker;

            public InternalTarget(MultiShot multiShot, Mobile attacker) : base(
                8,
                false,
                TargetFlags.None
            )
            {
                _multiShot = multiShot;
                _attacker = attacker;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                from.RevealingAction();

                if (targeted is Mobile target && from.CanBeHarmful(target, true))
                {
                    _multiShot.DoShot(_attacker, target);
                }
                else
                {
                    from.SendMessage("You can't use multi shot on that!");
                }
            }
        }
    }
}

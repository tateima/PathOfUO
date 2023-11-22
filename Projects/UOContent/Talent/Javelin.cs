using System;
using System.Linq;
using Server.Collections;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Talent
{
    public class Javelin : BaseTalent
    {
        public Javelin()
        {
            RequiredWeaponSkill = SkillName.Fencing;
            RequiredWeapon = new[] { typeof(BaseSpear) };
            TalentDependencies = new[] { typeof(SpearSpecialist) };
            DisplayName = "Javelin";
            CanBeUsed = true;
            Description = "Throw copy of equipped spear at target.";
            AdditionalDetail = "Each level increases the distance you can throw by 3 yards.";
            CooldownSeconds = 15;
            StamRequired = 10;
            ImageID = 370;
            GumpHeight = 75;
            AddEndY = 65;
            MaxLevel = 10;
        }

        public override void OnUse(Mobile from)
        {
            if (from.Stam >= StamRequired + 1 && HasSkillRequirement(from))
            {
                var weapon = from.Weapon as BaseWeapon;
                if (!OnCooldown && weapon?.Skill == RequiredWeaponSkill && weapon is BaseSpear)
                {
                    from.SendMessage("Whom do you wish to throw a spear at?");
                    from.Target = new InternalTarget(from, this);
                }
                else
                {
                    from.SendMessage("You do not have a spear equipped.");
                }
            }
            else
            {
                from.SendMessage(FailedRequirements);
            }

        }

        private class InternalTarget : Target
        {
            private readonly Javelin _javelin;
            private readonly Mobile _mobile;
            private Mobile _target;

            public InternalTarget(Mobile mobile, Javelin javelin) : base(
                8,
                false,
                TargetFlags.None
            )
            {
                _mobile = mobile;
                _javelin = javelin;
            }

            public void CheckHit()
            {
                if (_mobile.Weapon is BaseSpear spear)
                {
                    _javelin.ApplyStaminaCost(_mobile);
                    if (spear.CheckHit(_mobile, _target))
                    {
                        spear.OnHit(_mobile, _target);
                        _target.PlaySound(0x239);
                    }
                    spear.MoveToWorld(_target.Location, _mobile.Map);
                }
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                {
                    if (target == _mobile || !target.CanBeHarmful(from, false) ||
                        Core.AOS && !target.InLOS(from))
                    {
                        from.SendMessage("Thou cannot throw a spear at this target");
                        return;
                    }
                    using var list = PooledRefList<Mobile>.Create();
                    foreach (var mobile in from.GetMobilesInRange(_javelin.Level * 3))
                    {
                        if (mobile == target)
                        {
                            list.Add(mobile);
                        }
                    }
                    if (list.Count > 0)
                    {
                        _target = target;
                        from.RevealingAction();
                        _javelin.OnCooldown = true;
                        Timer.StartTimer(
                            TimeSpan.FromSeconds(_javelin.CooldownSeconds),
                            _javelin.ExpireTalentCooldown,
                            out _javelin._talentTimerToken
                        );
                        Effects.SendMovingEffect(from, target, 0x1BFE, 18, 2, false, false, 0x3B5);
                        Timer.StartTimer(TimeSpan.FromSeconds(1), CheckHit, out _);
                    }
                    else
                    {
                        from.SendMessage("Your target is too far away.");
                    }
                }
            }
        }
    }
}

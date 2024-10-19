using System;
using Server.Items;
using Server.Mobiles;
using Server.Pantheon;
using Server.Spells;
using Server.Targeting;

namespace Server.Talent
{
    public class WellOfDeath : BaseTalent
    {
        public WellOfDeath()
        {
            DisplayName = "Well of death";
            DeityAlignment = Deity.Alignment.Darkness;
            RequiresDeityFavor = true;
            MobilePercentagePerPoint = 10;
            CanBeUsed = true;
            Description =
                "Reanimates the remaining life force of a corpse into negative damage.";
            AdditionalDetail = "This damage is an area of effect and its distance increases by 2 yards per level. Each level also decreases cooldown by 10 seconds. The amount of damage scales with corpse type.";
            ImageID = 422;
            ManaRequired = 25;
            CooldownSeconds = 160;
            GumpHeight = 230;
            AddEndY = 70;
            AddEndAdditionalDetailsY = 100;
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown && HasSkillRequirement(from))
            {
                if (from.Mana > ManaRequired)
                {
                    from.Target = new InternalTarget(from, this);
                }
                else
                {
                    from.SendMessage($"You need {ManaRequired.ToString()} mana to turn a corpse into a well of negative energy.");
                }
            }
            else
            {
                from.SendMessage(FailedRequirements);
            }
        }

        private class InternalTarget : Target
        {
            private readonly WellOfDeath _wellOfDeath;
            private readonly Mobile _mobile;
            private Corpse _corpse;
            private bool _eastToWest;
            private TimerExecutionToken _token;
            private int _damage;
            private int _distance;

            public InternalTarget(Mobile from, WellOfDeath wellOfDeath) : base(
                10,
                false,
                TargetFlags.None
            )
            {
                _wellOfDeath = wellOfDeath;
                _mobile = from;
            }

            private void AreaEffect()
            {
                if (_wellOfDeath.Activated)
                {

                    foreach (var mobile in _corpse.GetMobilesInRange(_distance))
                    {
                        if (mobile == _mobile || !mobile.CanBeHarmful(_mobile, false) ||
                            Core.AOS && !mobile.InLOS(_corpse))
                        {
                            continue;
                        }

                        int damage = _damage;
                        _wellOfDeath.AlterDamage(mobile, (PlayerMobile)_mobile, ref damage);
                        if (Core.AOS)
                        {
                            AOS.Damage(mobile, _mobile, _damage, 0, 0, 50, 0, 50);
                            mobile.FixedParticles(0x374A, 10, 30, 5013, 0, 2, EffectLayer.Waist);
                            mobile.PlaySound(0x0FC);
                        }
                        else
                        {
                            mobile.FixedParticles(0x374A, 10, 15, 5013, EffectLayer.Waist);
                            mobile.PlaySound(0x1F1);
                            mobile.Damage(_damage, _mobile);
                        }
                    }
                    Timer.StartTimer(TimeSpan.FromSeconds(7), AreaEffect, out _token);
                }
            }

            private void ExpireAreaEffect()
            {
                _wellOfDeath.Activated = false;
                _token.Cancel();
                EmptyCorpse(_corpse);
                _corpse.Delete();
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                from.RevealingAction();
                if (targeted is Corpse corpse)
                {
                    Mobile previousLife = null;
                    try
                    {
                        previousLife = corpse.PreviousLifeType.CreateInstance<Mobile>();
                    }
                    catch
                    {
                        // ignored
                    }

                    if (previousLife != null)
                    {
                        _wellOfDeath.Activated = true;
                        _wellOfDeath.OnCooldown = true;
                        int damage = AOS.Scale(previousLife.HitsMax, _wellOfDeath.MobilePercentagePerPoint);
                        if (previousLife is PlayerMobile)
                        {
                            damage = Utility.RandomMinMax(25, 40);
                        }
                        if (damage > 40)
                        {
                            damage = 40;
                        }

                        previousLife = null;

                        _damage = damage;
                        var sound = Core.AOS ? 0x0FC : 0x1F1;
                        _mobile.PlaySound(sound);
                        _corpse = corpse;
                        _eastToWest = SpellHelper.GetEastToWest(from.Location, corpse.Location);
                        _distance = 2 + _wellOfDeath.Level * 2;
                        for (var i = -_distance; i <= _distance; i++)
                        {
                            for (var j = -_distance; j <= _distance; j++)
                            {
                                var targetLoc = new Point3D(_corpse.Location.X + i, _corpse.Location.Y + j, _corpse.Location.Z);
                                Effects.SendLocationParticles(
                                    EffectItem.Create(targetLoc, _mobile.Map, EffectItem.DefaultDuration),
                                    0x374A,
                                    1,
                                    40,
                                    0,
                                    3,
                                    9917,
                                    0
                                );
                            }
                        }
                        Timer.StartTimer(TimeSpan.FromSeconds(7), AreaEffect, out _token);
                        Timer.StartTimer(TimeSpan.FromSeconds(70), ExpireAreaEffect);
                        Timer.StartTimer(TimeSpan.FromSeconds(_wellOfDeath.CooldownSeconds - _wellOfDeath.Level * 10), _wellOfDeath.ExpireTalentCooldown, out _wellOfDeath._talentTimerToken);
                    }
                    else
                    {
                        from.SendMessage("This corpse has no life force left.");
                    }
                }
                else
                {
                    from.SendMessage("This target is not dead.");
                }
            }
        }
    }
}

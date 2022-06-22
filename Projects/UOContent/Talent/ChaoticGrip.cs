using System;
using Server.Items;
using Server.Spells;
using Server.Network;
using Server.Pantheon;
using Server.Spells.Fourth;
using Server.Targeting;

namespace Server.Talent
{
    public class ChaoticGrip : BaseTalent
    {
        public ChaoticGrip()
        {
            DisplayName = "Chaotic grip";
            DeityAlignment = Deity.Alignment.Chaos;
            RequiresDeityFavor = true;
            CanBeUsed = true;
            Description =
                "Pulls a target toward you.";
            AdditionalDetail = "The distance of the grip increases with each level by 3 points. Only players with chaotic alignment can use this.";
            ImageID = 412;
            CooldownSeconds = 120;
            ManaRequired = 35;
            GumpHeight = 230;
            AddEndY = 70;
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown)
            {
                if (from.Mana > ManaRequired)
                {
                    from.Target = new InternalTarget(this);
                }
                else
                {
                    from.SendMessage($"You need {ManaRequired.ToString()} mana to summon this wave of flame.");
                }
            }
        }
        private class InternalTarget : Target
        {
            private ChaoticGrip _chaoticGrip;

            public InternalTarget(ChaoticGrip chaoticGrip) : base(
                10,
                false,
                TargetFlags.None
            ) =>
                _chaoticGrip = chaoticGrip;


            protected override void OnTarget(Mobile from, object targeted)
            {
                from.RevealingAction();
                if (targeted is Mobile target)
                {
                    var distanceTo = (int)from.GetDistanceToSqrt(target.Location);
                    if (4 + _chaoticGrip.Level >= distanceTo)
                    {
                        if (target == from || !target.CanBeHarmful(from, false) ||
                            Core.AOS && !target.InLOS(from))
                        {
                            from.SendMessage("Thou cannot pull this target towards you.");
                        }
                        else
                        {
                            var location = _chaoticGrip.CalculatePushbackFromAnchor(target.Location, (int)from.GetDistanceToSqrt(target.Location)-1, from);
                            _chaoticGrip.ApplyManaCost(from);
                            if (Core.AOS)
                            {
                                target.FixedParticles(0x374A, 10, 30, 5013, 1153, 2, EffectLayer.Waist);
                                target.PlaySound(0x0FC);
                            }
                            else
                            {
                                target.FixedParticles(0x374A, 10, 15, 5013, EffectLayer.Waist);
                                target.PlaySound(0x1F1);
                            }
                            target.MoveToWorld(location, from.Map);

                            _chaoticGrip.OnCooldown = true;
                            Timer.StartTimer(TimeSpan.FromSeconds(_chaoticGrip.CooldownSeconds), _chaoticGrip.ExpireTalentCooldown, out _chaoticGrip._talentTimerToken);
                        }
                    }
                    else
                    {
                        from.SendMessage("You are too far away.");
                    }
                }
            }
        }
    }
}

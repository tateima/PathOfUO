using System;
using Server.Mobiles;
using Server.Network;
using Server.Pantheon;
using Server.Targeting;

namespace Server.Talent
{
    public class LayOnHands : BaseTalent
    {
        public LayOnHands()
        {
            DisplayName = "Lay on hands";
            DeityAlignment = Deity.Alignment.Order;
            RequiresDeityFavor = true;
            CanBeUsed = true;
            Description =
                "Heal a target ally for full health.";
            AdditionalDetail = "The cooldown decreases by 15 seconds per level.";
            ImageID = 417;
            ManaRequired = 35;
            CooldownSeconds = 300;
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
                    from.SendMessage($"You need {ManaRequired.ToString()} mana to use this major healing power.");
                }
            }
        }

        private class InternalTarget : Target
        {
            private readonly LayOnHands _layOnHands;
            public InternalTarget(LayOnHands layOnHands) : base(
                10,
                false,
                TargetFlags.None
            ) =>
                _layOnHands = layOnHands;


            protected override void OnTarget(Mobile from, object targeted)
            {
                from.RevealingAction();
                if (targeted is Mobile target)
                {
                    if (Core.AOS && !target.InLOS(from))
                    {
                        from.SendMessage("Thou cannot give this target a protective barrier.");
                    }
                    else
                    {
                        var validTarget = Deity.CanReceiveAlignment(target, Deity.Alignment.Order);

                        if (validTarget)
                        {
                            _layOnHands.OnCooldown = true;
                            target.Heal(target.HitsMax, from);
                            target.PlaySound(0x202);
                            target.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                            Timer.StartTimer(TimeSpan.FromSeconds(_layOnHands.CooldownSeconds - _layOnHands.Level * 15), _layOnHands.ExpireTalentCooldown, out _layOnHands._talentTimerToken);
                        }
                        else
                        {
                            from.SendMessage("This target is of the wrong deity alignment.");
                        }
                    }
                }
            }
        }
    }
}

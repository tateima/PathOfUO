using System;
using Server.Mobiles;
using Server.Network;
using Server.Pantheon;
using Server.Targeting;

namespace Server.Talent
{
    public class DivineProtection : BaseTalent
    {
        private TimerExecutionToken _protectionTimerToken;
        private Mobile _ally;
        public DivineProtection()
        {
            DisplayName = "Divine Protection";
            DeityAlignment = Deity.Alignment.Light;
            RequiresDeityFavor = true;
            CanAbsorbSpells = true;
            HasDamageAbsorptionEffect = true;
            CanBeUsed = true;
            Description =
                "Prevent all damage to a target ally for 60 seconds.";
            AdditionalDetail = "The duration of this effect increases by 5 seconds per level. This ability will drain all your available mana while active.";
            ImageID = 416;
            CooldownSeconds = 600;
            GumpHeight = 230;
            AddEndY = 70;
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown && HasSkillRequirement(from))
            {
                if (from.Mana > ManaRequired)
                {
                    from.Target = new InternalTarget(this);
                }
                else
                {
                    from.SendMessage($"You need {ManaRequired.ToString()} mana to summon a protective barrier.");
                }
            }
            else
            {
                from.SendMessage(FailedRequirements);
            }
        }

        public override int CheckDamageAbsorptionEffect(Mobile defender, Mobile attacker, int damage)
        {
            if (Activated)
            {
                return 0;
            }
            return damage;
        }

        public void PlayEffect(Mobile from)
        {
            from.PlaySound(0x1E9);
            from.FixedParticles(0x375A, 9, 20, 5016, EffectLayer.Waist);
        }

        private void AddBuff()
        {
            CheckSetTalentEffect(_ally, this);
            Activated = true;
        }

        private void RemoveBuff()
        {
            _protectionTimerToken.Cancel();
            CheckSetTalentEffect(_ally, null);
            Activated = false;
        }

        private void Tick()
        {
            if (Activated)
            {
                _ally.Mana = 0;
                Timer.StartTimer(TimeSpan.FromSeconds(1), Tick);
            }
        }

        private class InternalTarget : Target
        {
            private readonly DivineProtection _divineProtection;
            public InternalTarget(DivineProtection divineProtection) : base(
                10,
                false,
                TargetFlags.None
            ) =>
                _divineProtection = divineProtection;


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
                        var validTarget = Deity.CanReceiveAlignment(target, Deity.Alignment.Light);

                        if (validTarget)
                        {
                            _divineProtection.OnCooldown = true;
                            _divineProtection._ally = target;
                            _divineProtection.AddBuff();
                            _divineProtection.PlayEffect(target);
                            Timer.StartTimer(TimeSpan.FromSeconds(1), _divineProtection.Tick, out _divineProtection._protectionTimerToken);
                            Timer.StartTimer(TimeSpan.FromSeconds(60 + _divineProtection.Level * 5), _divineProtection.RemoveBuff);
                            Timer.StartTimer(TimeSpan.FromSeconds(_divineProtection.CooldownSeconds), _divineProtection.ExpireTalentCooldown, out _divineProtection._talentTimerToken);
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

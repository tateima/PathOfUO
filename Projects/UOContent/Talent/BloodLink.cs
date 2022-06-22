using System;
using Org.BouncyCastle.Bcpg.Sig;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Network;
using Server.Pantheon;
using Server.Spells.Fourth;
using Server.Targeting;

namespace Server.Talent
{
    public class BloodLink : BaseTalent
    {
        private Mobile _caster;
        private Mobile _ally;
        public BloodLink()
        {
            DisplayName = "Blood link";
            DeityAlignment = Deity.Alignment.Darkness;
            RequiresDeityFavor = true;
            CanAbsorbSpells = true;
            HasDamageAbsorptionEffect = true;
            CanBeUsed = true;
            Description =
                "Creates an unnatural link of life force between caster and target.";
            AdditionalDetail = "All damage is shared between both linked parties for 80 seconds. Each level reduces mana requirement by 20.";
            ImageID = 421;
            CooldownSeconds = 200;
            ManaRequired = 60;
            GumpHeight = 230;
            AddEndY = 70;
        }
        public override int CheckDamageAbsorptionEffect(Mobile defender, Mobile attacker, int damage)
        {
            if (Activated)
            {
                int shared = damage / 2;
                if (_ally == defender)
                {
                    _caster.Hits -= shared;
                } else if (_caster == defender)
                {
                    _ally.Hits -= shared;
                }
                return shared;
            }
            return damage;
        }


        public override void OnUse(Mobile from)
        {
            if (!OnCooldown)
            {
                if (from.Mana > ManaRequired - Level * 20)
                {
                    from.Target = new InternalTarget(this);
                }
                else
                {
                    from.SendMessage($"You need {(ManaRequired - Level * 20).ToString()} mana to use {DisplayName}.");
                }
            }
        }
        private void CheckSetBuff(BloodLink? bloodLink)
        {
            if (_ally is PlayerMobile playerMobile)
            {
                playerMobile.TalentEffect = bloodLink;
            } else if (_ally is BaseCreature baseCreature)
            {
                baseCreature.TalentEffect = bloodLink;
            }
        }

        private void AddBuff()
        {
            CheckSetBuff(this);
            Activated = true;
        }

        private void RemoveBuff()
        {
            CheckSetBuff(null);
            Activated = false;
        }

        public void PlayEffect(Mobile from)
        {
            from.FixedParticles(0x3728, 1, 13, 9912, 1150, 7, EffectLayer.Head);
            from.FixedParticles(0x3779, 1, 15, 9502, 67, 7, EffectLayer.Head);
        }

        private class InternalTarget : Target
        {
            private readonly BloodLink _bloodLink;

            public InternalTarget(BloodLink bloodLink) : base(
                10,
                false,
                TargetFlags.None
            ) =>
                _bloodLink = bloodLink;

            protected override void OnTarget(Mobile from, object targeted)
            {
                from.RevealingAction();
                if (targeted is Mobile target)
                {
                    if (Core.AOS && !target.InLOS(from))
                    {
                        from.SendMessage("Thou cannot link your blood with this target.");
                    }
                    else
                    {
                        var validTarget = Deity.CanReceiveAlignment(target, Deity.Alignment.Darkness);
                        if (validTarget)
                        {
                            from.PlaySound(0xFC);
                            _bloodLink.Activated = true;
                            _bloodLink._ally = target;
                            _bloodLink._caster = from;
                            _bloodLink.AddBuff();
                            _bloodLink.PlayEffect(target);
                            _bloodLink.PlayEffect(from);
                            _bloodLink.OnCooldown = true;
                            Timer.StartTimer(TimeSpan.FromSeconds(80), _bloodLink.RemoveBuff);
                            Timer.StartTimer(TimeSpan.FromSeconds(_bloodLink.CooldownSeconds), _bloodLink.ExpireTalentCooldown, out _bloodLink._talentTimerToken);
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

using System;
using System.Collections.Generic;
using Server.Engines.BuffIcons;
using Server.Mobiles;
using Server.Talent;
using Server.Targeting;

namespace Server.Spells.Spellweaving
{
    public class GiftOfRenewalSpell : ArcanistSpell, ITargetingSpell<Mobile>
    {
        private static readonly SpellInfo _info = new(
            "Gift of Renewal",
            "Olorisstra",
            -1
        );

        private static readonly Dictionary<Mobile, GiftOfRenewalTimer> _table = new();

        public GiftOfRenewalSpell(Mobile caster, Item scroll = null) : base(caster, scroll, _info)
        {
        }

        public override TimeSpan CastDelayBase => TimeSpan.FromSeconds(3.0);

        public override double RequiredSkill => 0.0;
        public override int RequiredMana => 24;

        public void Target(Mobile m)
        {
            if (m == null)
            {
                return;
            }

            if (!Caster.CanSee(m))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (_table.ContainsKey(m))
            {
                Caster.SendLocalizedMessage(501775); // This spell is already in effect.
            }
            else if (!Caster.CanBeginAction<GiftOfRenewalSpell>())
            {
                Caster.SendLocalizedMessage(501789); // You must wait before trying again.
            }
            else if (CheckBSequence(m))
            {
                SpellHelper.Turn(Caster, m);

                Caster.FixedEffect(0x374A, 10, 20);
                Caster.PlaySound(0x5C9);

                BaseTalent venomBlood = null;
                if (m is PlayerMobile playerTarget)
                {
                    venomBlood = playerTarget.GetTalent(typeof(VenomBlood));
                }

                if (m.Poisoned && !(venomBlood != null))
                {
                    m.CurePoison(m);
                }
                else
                {
                    var skill = Caster.Skills.Spellweaving.Value;

                    var hitsPerRound = 5 + (int)(skill / 24) + FocusLevel;

                    var duration = 30 + FocusLevel * 10;

                    if (Caster is PlayerMobile player)
                    {
                        BaseTalent lightAffinity = player.GetTalent(typeof(LightAffinity));
                        if (lightAffinity != null)
                        {
                            hitsPerRound += lightAffinity.Level;
                            duration += lightAffinity.Level * 2;
                        }
                    }

                    var t = new GiftOfRenewalTimer(Caster, m, hitsPerRound, duration);

                    _table[m] = t;

                    t.Start();

                    Caster.BeginAction<GiftOfRenewalSpell>();

                    (m as PlayerMobile)?.AddBuff(
                        new BuffInfo(
                            BuffIcon.GiftOfRenewal,
                            1031602,
                            1075797,
                            TimeSpan.FromSeconds(duration),
                            hitsPerRound.ToString()
                        )
                    );
                }
            }
        }

        public override void OnCast()
        {
            Caster.Target = new SpellTarget<Mobile>(this, TargetFlags.Beneficial);
        }

        public static bool StopEffect(Mobile m)
        {
            (m as PlayerMobile)?.RemoveBuff(BuffIcon.GiftOfRenewal);

            if (_table.Remove(m, out var timer))
            {
                timer.Stop();
                Timer.StartTimer(TimeSpan.FromSeconds(60), timer._caster.EndAction<GiftOfRenewalSpell>);
                return true;
            }

            return false;
        }

        private class GiftOfRenewalTimer : Timer
        {
            public Mobile _caster;
            public int _hitsPerRound;
            public Mobile _mobile;

            internal GiftOfRenewalTimer(Mobile caster, Mobile mobile, int hitsPerRound, int duration)
                : base(TimeSpan.FromSeconds(2.0), TimeSpan.FromSeconds(2.0), duration / 2)
            {
                _caster = caster;
                _mobile = mobile;
                _hitsPerRound = hitsPerRound;
            }

            protected override void OnTick()
            {
                // Last tick will change running to false
                if (!Running)
                {
                    StopEffect(_mobile);
                    _mobile.PlaySound(0x455);
                    _mobile.SendLocalizedMessage(1075071); // The Gift of Renewal has faded.
                    return;
                }

                if (!_table.ContainsKey(_mobile))
                {
                    Stop();
                    return;
                }

                if (!_mobile.Alive)
                {
                    Stop();
                    StopEffect(_mobile);
                    return;
                }

                if (_mobile.Hits >= _mobile.HitsMax)
                {
                    return;
                }

                var toHeal = _hitsPerRound;

                SpellHelper.Heal(toHeal, _mobile, _caster);
                _mobile.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist);
            }
        }
    }
}

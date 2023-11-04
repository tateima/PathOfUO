using System;
using System.Collections.Generic;
using System.Linq;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Talent
{
    public class EternalChord : BaseTalent
    {
        public EternalChord()
        {
            TalentDependencies = new[] { typeof(Resonance) };
            CanBeUsed = true;
            StatModNames = new[] { "EternalChord" };
            DisplayName = "Eternal chord";
            CooldownSeconds = 150;
            ManaRequired = 35;
            MaxLevel = 2;
            Description =
                "Unleash a note of pure energy at a target. Requires 80 music.";
            AdditionalDetail = "Each level decreases the cooldown by 15 seconds and increases the potency of the chord.";
            ImageID = 432;
            AddEndY = 110;
        }

        public override bool HasSkillRequirement(Mobile mobile) => mobile.Skills.Musicianship.Base >= 80;

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown && from.Mana > ManaRequired && HasSkillRequirement(from))
            {
                BaseInstrument instrument = null;
                List<Item> instruments = from.Backpack?.FindItemsByType(typeof(BaseInstrument));
                instruments?.ForEach(
                    packInstrument =>
                    {
                        if (((BaseInstrument)packInstrument).UsesRemaining > 0)
                        {
                            instrument = (BaseInstrument)packInstrument;
                        }
                    }
                );

                if (instrument != null)
                {
                    from.SendMessage("Whom do you wish to target");
                    from.Target = new InternalTarget(from, instrument, this);
                }
                else
                {
                    from.SendMessage("You require an instrument to use this talent");
                }
            }
            else
            {
                from.SendMessage(FailedRequirements);
            }
        }

        private class InternalTarget : Target
        {
            private readonly BaseInstrument _instrument;
            private readonly EternalChord _eternalChord;

            public InternalTarget(Mobile from, BaseInstrument instrument, EternalChord eternalChord) : base(
                BaseInstrument.GetBardRange(from, SkillName.Provocation),
                false,
                TargetFlags.None
            )
            {
                _instrument = instrument;
                _eternalChord = eternalChord;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                from.RevealingAction();
                var sonicAffinity = ((PlayerMobile)from).GetTalent(typeof(SonicAffinity));
                if (targeted is Mobile mobile) {
                    if (!BaseInstrument.CheckMusicianship(from))
                    {
                        _instrument.PlayInstrumentBadly(from);
                        from.SendLocalizedMessage(500612); // You play poorly, and there is no effect.
                        return;
                    }
                    int amount = Utility.RandomMinMax(_eternalChord.Level * 2, _eternalChord.Level * 3) + sonicAffinity.ModifySpellMultiplier();
                    if (mobile == from || !from.CanBeHarmful(mobile, false))
                    {
                        // positive event
                        if (mobile.Hits < mobile.HitsMax)
                        {
                            mobile.Heal(amount);
                            mobile.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                            mobile.PlaySound(0x202);
                        }
                        else
                        {
                            StatMod statMod = Utility.RandomMinMax(1, 3) switch
                            {
                                1 => new StatMod(
                                    StatType.Str,
                                    _eternalChord.StatModNames[0],
                                    amount,
                                    TimeSpan.FromSeconds(5 * _eternalChord.Level)
                                ),
                                2 => new StatMod(
                                    StatType.Dex,
                                    _eternalChord.StatModNames[0],
                                    amount,
                                    TimeSpan.FromSeconds(5 * _eternalChord.Level)
                                ),
                                3 => new StatMod(
                                    StatType.Int,
                                    _eternalChord.StatModNames[0],
                                    amount,
                                    TimeSpan.FromSeconds(5 * _eternalChord.Level)
                                ),
                                _ => null
                            };
                            if (statMod is not null)
                            {
                                mobile.AddStatMod(statMod);
                                mobile.FixedParticles(0x375A, 9, 20, 5016, EffectLayer.Waist);
                            }
                        }
                    }
                    else if (!Equals(mobile,from) && from.CanBeHarmful(mobile, true))
                    {
                        amount = Utility.RandomMinMax(_eternalChord.Level * 3, _eternalChord.Level * 5) + sonicAffinity.ModifySpellMultiplier();
                        from.MovingParticles(mobile, 0x379F, 7, 0, false, true, 3043, 4043, 0x211);
                        from.PlaySound(0x20A);
                        mobile.Damage(amount, from);
                        mobile.DoHarmful(from);
                    }
                    _instrument.PlayInstrumentWell(from);
                    _eternalChord.OnCooldown = true;
                    Timer.StartTimer(
                        TimeSpan.FromSeconds(_eternalChord.CooldownSeconds - _eternalChord.Level * 15),
                        _eternalChord.ExpireTalentCooldown,
                        out _eternalChord._talentTimerToken
                    );
                }
                else
                {
                    from.SendMessage("You cannot use an eternal chord on that!");
                }
            }
        }
    }
}

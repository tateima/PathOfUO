using System;
using System.Collections.Generic;
using System.Linq;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Talent
{
    public class DominateCreature : BaseTalent
    {
        public DominateCreature()
        {
            TalentDependencies = new[] { typeof(Resonance) };
            CanBeUsed = true;
            DisplayName = "Dominate creature";
            CooldownSeconds = 300;
            ManaRequired = 40;
            Description =
                "Chance to control target for 10 seconds per level. Requires 90 music, all bardic skills 70+.";
            AdditionalDetail = "Each level decreases the cooldown by 30 seconds.";
            ImageID = 191;
            AddEndY = 110;
        }

        public override bool HasSkillRequirement(Mobile mobile)
        {
            var group = SkillsGumpGroup.Groups.FirstOrDefault(group => group.Name == "Bardic");
            var bardicValid = false;
            var musicValid = false;
            if (group != null)
            {
                foreach (var skill in group.Skills)
                {
                    if (skill == SkillName.Musicianship && mobile.Skills[skill].Base >= 90)
                    {
                        musicValid = true;
                    }
                    // this needs all bardic skills to be at least 70
                    else
                    {
                        bardicValid = mobile.Skills[skill].Base >= 70;
                        if (!bardicValid)
                        {
                            break;
                        }
                    }
                }
            }

            return bardicValid && musicValid;
        }

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
                    from.SendMessage("Whom do you wish to control");
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
            private readonly DominateCreature _dominateCreature;
            private BaseCreature _creature;

            public InternalTarget(Mobile from, BaseInstrument instrument, DominateCreature dominateCreature) : base(
                BaseInstrument.GetBardRange(from, SkillName.Provocation),
                false,
                TargetFlags.None
            )
            {
                _instrument = instrument;
                _dominateCreature = dominateCreature;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                from.RevealingAction();

                if (targeted is BaseCreature creature && from.CanBeHarmful(creature, true))
                {
                    _creature = creature;
                    if (!_instrument.IsChildOf(from.Backpack))
                    {
                        from.SendLocalizedMessage(
                            1062488
                        ); // The instrument you are trying to play is no longer in your backpack!
                    }
                    else if (creature.BardEndTime > Core.Now)
                    {
                        from.SendMessage("You may not dominate this target yet.");
                    }
                    else if (creature.Controlled)
                    {
                        from.SendMessage("They are too loyal to their master to be dominated.");
                    }
                    else if (creature.IsParagon || creature.IsHeroic ||
                             BaseInstrument.GetBaseDifficulty(creature, true) >=
                             117) // dragons are 117 and cannot be tamed so dont allow bards to either
                    {
                        from.SendMessage(" You have no chance of dominating those creatures.");
                    }
                    else
                    {
                        from.RevealingAction();
                        _dominateCreature.OnCooldown = true;
                        Timer.StartTimer(
                            TimeSpan.FromSeconds(_dominateCreature.CooldownSeconds - _dominateCreature.Level * 30),
                            _dominateCreature.ExpireTalentCooldown,
                            out _dominateCreature._talentTimerToken
                        );
                        var diff = _instrument.GetDifficultyFor(creature) - 10.0;
                        // discordance is the core engine mechanic
                        var music = from.Skills.Discordance.Value;

                        if (music > 100.0)
                        {
                            diff -= (music - 100.0) * 0.5;
                        }

                        if (!BaseInstrument.CheckMusicianship(from))
                        {
                            from.NextSkillTime = Core.TickCount + 5000;
                            from.SendLocalizedMessage(500612); // You play poorly, and there is no effect.
                            _instrument.PlayInstrumentBadly(from);
                            _instrument.ConsumeUse(from);
                        }
                        else
                        {
                            if (!from.CheckTargetSkill(SkillName.Discordance, creature, diff - 25.0, diff + 25.0))
                            {
                                from.NextSkillTime = Core.TickCount + 5000;
                                from.SendMessage("Your music fails to dominate the creatures mind.");
                                _instrument.PlayInstrumentBadly(from);
                                _instrument.ConsumeUse(from);
                            }
                            else
                            {
                                from.PrivateOverheadMessage(
                                    MessageType.Regular,
                                    0x3B2,
                                    502799,
                                    from.NetState
                                ); // It seems to accept you as master.
                                from.SendMessage(
                                    "You play your music and your target submits to your will for a brief time"
                                );
                                _instrument.PlayInstrumentWell(from);
                                _instrument.ConsumeUse(from);
                                _creature.Owners.Add(from);
                                _creature.SetControlMaster(from);
                                _creature.Summoned = true;
                                _creature.BardEndTime = Core.Now + TimeSpan.FromSeconds(_dominateCreature.Level * 10);
                                Timer.StartTimer(
                                    TimeSpan.FromSeconds(_dominateCreature.Level * 10),
                                    ExpireDomination,
                                    out _
                                );
                            }
                        }
                    }
                }
                else
                {
                    from.SendLocalizedMessage(501589); // You can't incite that!
                }
            }

            private void ExpireDomination()
            {
                _creature.Owners.Clear();
                _creature.SetControlMaster(null);
                _creature.Summoned = false;
            }
        }
    }
}

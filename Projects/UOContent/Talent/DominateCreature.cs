using System;
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
            TalentDependency = typeof(Resonance);
            CanBeUsed = true;
            DisplayName = "Dominate creature";
            Description =
                "Chance on to control target for 1 minute per level. 5 minute cooldown. Requires 90 music, all bardic skills 70+.";
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
                    }
                }
            }

            return bardicValid && musicValid;
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown)
            {
                BaseInstrument instrument = null;
                from.Backpack?.FindItemsByType<BaseInstrument>()
                    .ForEach(
                        packInstrument =>
                        {
                            if (packInstrument.UsesRemaining > 0)
                            {
                                instrument = packInstrument;
                            }
                        }
                    );

                if (instrument != null)
                {
                    from.SendMessage("Whom do you wish to control");
                    from.Target = new InternalFirstTarget(from, instrument, Level);
                    OnCooldown = true;
                    Timer.StartTimer(TimeSpan.FromSeconds(300 - Level * 30), ExpireTalentCooldown, out _talentTimerToken);
                }
                else
                {
                    from.SendMessage("You require an instrument to use this talent");
                }
            }
        }

        private class InternalFirstTarget : Target
        {
            private readonly BaseInstrument m_Instrument;
            private readonly int m_level;
            private BaseCreature m_Creature;

            public InternalFirstTarget(Mobile from, BaseInstrument instrument, int level) : base(
                BaseInstrument.GetBardRange(from, SkillName.Provocation),
                false,
                TargetFlags.None
            )
            {
                m_Instrument = instrument;
                m_level = level;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                from.RevealingAction();

                if (targeted is BaseCreature creature && from.CanBeHarmful(creature, true))
                {
                    m_Creature = creature;
                    if (!m_Instrument.IsChildOf(from.Backpack))
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
                        var diff = m_Instrument.GetDifficultyFor(creature) - 10.0;
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
                            m_Instrument.PlayInstrumentBadly(from);
                            m_Instrument.ConsumeUse(from);
                        }
                        else
                        {
                            if (!from.CheckTargetSkill(SkillName.Discordance, creature, diff - 25.0, diff + 25.0))
                            {
                                from.NextSkillTime = Core.TickCount + 5000;
                                from.SendMessage("Your music fails to dominate the creatures mind.");
                                m_Instrument.PlayInstrumentBadly(from);
                                m_Instrument.ConsumeUse(from);
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
                                m_Instrument.PlayInstrumentWell(from);
                                m_Instrument.ConsumeUse(from);
                                m_Creature.Owners.Add(from);
                                m_Creature.SetControlMaster(from);
                                m_Creature.Summoned = true;
                                m_Creature.BardEndTime = Core.Now + TimeSpan.FromSeconds(m_level * 10);
                                Timer.StartTimer(
                                    TimeSpan.FromSeconds(m_level * 10),
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
                m_Creature.Owners.Clear();
                m_Creature.SetControlMaster(null);
                m_Creature.Summoned = false;
            }
        }
    }
}

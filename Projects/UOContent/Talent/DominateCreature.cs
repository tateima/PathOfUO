using System;
using System.Collections.Generic;
using System.Linq;
using Server.Mobiles;
using Server.Items;
using Server.Targeting;
using Server.Gumps;


namespace Server.Talent
{
    public class DominateCreature : BaseTalent, ITalent
    {
        public DominateCreature() : base()
        {
            TalentDependency = typeof(Resonance);
            CanBeUsed = true;
            DisplayName = "Dominate creature";
            Description = "Chance on to control target for 1 minute per level. 5 minute cooldown.";
            ImageID = 30049;
        }
        public override bool HasSkillRequirement(Mobile mobile)
        {
            SkillsGumpGroup group = SkillsGumpGroup.Groups.Where(group => group.Name == "Bardic").First();
            bool bardicValid = false;
            bool musicValid = false;
            foreach (SkillName skill in group.Skills)
            {
                if (skill == SkillName.Musicianship && mobile.Skills[skill].Base >= 90)
                {
                    musicValid = true;
                }
                // this needs all bardic skills to be atleast 70
                else
                {
                    bardicValid = (mobile.Skills[skill].Base >= 70);
                }
            }
            return (bardicValid && musicValid);
        }
        public override void OnUse(Mobile from)
        {
            if (!OnCooldown)
            {
                BaseInstrument instrument = null;
                if (from.Backpack != null)
                {
                    from.Backpack.FindItemsByType<BaseInstrument>(true).ForEach(
                    packInstrument =>
                    {
                        if (packInstrument.UsesRemaining > 0)
                        {
                            instrument = packInstrument;
                            return;
                        }
                    }
                    );
                }
                if (instrument != null)
                {
                    from.SendMessage("Whom do you wish to control");
                    from.Target = new InternalFirstTarget(from, instrument, Level);
                    OnCooldown = true;
                    Timer.StartTimer(TimeSpan.FromSeconds(300 - (Level * 60)), ExpireTalentCooldown, out _talentTimerToken);
                }
                else
                {
                    from.SendMessage("You require an instrument to use this talent");
                }
            }
        }
        private class InternalFirstTarget : Target
        {
            public TimerExecutionToken _dominateTimerToken;
            private readonly BaseInstrument m_Instrument;
            private BaseCreature m_Creature;
            private readonly int m_level;
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
                    else if (creature.Controlled)
                    {
                        from.SendMessage("They are too loyal to their master to be dominated.");
                    }
                    else if (creature.IsParagon && BaseInstrument.GetBaseDifficulty(creature, true) >= 160.0)
                    {
                        from.SendMessage(" You have no chance of dominating those creatures.");
                    }
                    else
                    {
                        from.RevealingAction();
                        var diff = m_Instrument.GetDifficultyFor((Mobile)targeted) - 10.0;
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
                                from.SendMessage("You play your music and your target submits to your will for a brief time");
                                m_Instrument.PlayInstrumentWell(from);
                                m_Instrument.ConsumeUse(from);
                                m_Creature.Owners.Add(from);
                                Timer.StartTimer(TimeSpan.FromSeconds(m_level * 60), ExpireDomination, out _dominateTimerToken);
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
            }
        }
    }
}

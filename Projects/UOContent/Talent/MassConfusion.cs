using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Talent
{
    public class MassConfusion : BaseTalent
    {
        public MassConfusion()
        {
            TalentDependency = typeof(Resonance);
            DisplayName = "Mass Confusion";
            Description =
                "Your music confuses nearby monsters. Each level increases AOE and decreases cooldown by 10s. 3 min cooldown. Requires 60 peace, provocation and 70 music.";
            ImageID = 384;
            CanBeUsed = true;
            GumpHeight = 230;
            AddEndY = 125;
            MaxLevel = 5;
        }

        public override bool HasSkillRequirement(Mobile mobile)
        {
            var bardicValid = mobile.Skills[SkillName.Peacemaking].Value >= 60 &&
                              mobile.Skills[SkillName.Provocation].Value >= 60;
            var musicValid = mobile.Skills[SkillName.Musicianship].Value >= 70;
            return bardicValid && musicValid;
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown)
            {
                if (from.Mana < 30)
                {
                    from.SendMessage("You require at least 30 mana to use this talent");
                }
                else
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
                        from.Mana -= 30;
                        from.RevealingAction();
                        var success = false;
                        var sonicAffinity = ((PlayerMobile)from).GetTalent(typeof(SonicAffinity));
                        var resonance = ((PlayerMobile)from).GetTalent(typeof(Resonance)) as Resonance;
                        const int seconds = 10;
                        foreach (var other in from.GetMobilesInRange(Level + 3))
                        {
                            if (other == from || !other.CanBeHarmful(from, false) ||
                                Core.AOS && !other.InLOS(from) || other is not BaseCreature creature)
                            {
                                continue;
                            }

                            var diff = instrument.GetDifficultyFor(creature) - 10.0;
                            if (sonicAffinity != null)
                            {
                                diff -= sonicAffinity.ModifySpellScalar();
                            }

                            if (from.Skills.Musicianship.Value > 100.0)
                            {
                                diff -= (from.Skills.Musicianship.Value - 100.0) * 0.5;
                            }

                            if (!BaseInstrument.CheckMusicianship(from)
                                || !from.CheckTargetSkill(SkillName.Peacemaking, other, diff - 25.0, diff + 25.0)
                                || !from.CheckTargetSkill(SkillName.Provocation, other, diff - 25.0, diff + 25.0))
                            {
                                from.SendLocalizedMessage(500612); // You play poorly, and there is no effect.
                            }
                            else
                            {
                                var attempts = 10;
                                resonance?.CheckHitEffect(from, creature, 0);
                                while (!success)
                                {
                                    attempts++;
                                    var nearby = Blindness.RandomNearbyMobile(creature, Level + 3);
                                    if (nearby != null && creature.CanBeHarmful(nearby) && creature.InLOS(nearby))
                                    {
                                        creature.Provoke(from, nearby, true);
                                        success = true;
                                    }

                                    if (attempts > 10)
                                    {
                                        creature.Pacify(from, Core.Now + TimeSpan.FromSeconds(seconds));
                                        success = true;
                                    }
                                }
                            }
                        }

                        if (success)
                        {
                            instrument.PlayInstrumentWell(from);
                        }
                        else
                        {
                            instrument.PlayInstrumentBadly(from);
                        }

                        instrument.ConsumeUse(from);
                        OnCooldown = true;
                        Timer.StartTimer(
                            TimeSpan.FromSeconds(180 - Level * 10),
                            ExpireTalentCooldown,
                            out _talentTimerToken
                        );
                    }
                    else
                    {
                        from.SendMessage("You require an instrument to use this talent");
                    }
                }
            }
        }
    }
}

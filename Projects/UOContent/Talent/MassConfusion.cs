using Server.Mobiles;
using Server.Items;
using System;

namespace Server.Talent
{
    public class MassConfusion : BaseTalent, ITalent
    {
        public MassConfusion() : base()
        {
            TalentDependency = typeof(Resonance);
            DisplayName = "Mass Confusion";
            Description = "Your music confuses nearby monsters. Each level increases AOE and decreases cooldown by 10s. 3 min cooldown. Requires 60 peace, provocation and 70 music.";
            ImageID = 384;
            CanBeUsed = true;
            GumpHeight = 230;
            AddEndY = 125;
            MaxLevel = 5;
        }
         public override bool HasSkillRequirement(Mobile mobile)
        {
            bool bardicValid = mobile.Skills[SkillName.Peacemaking].Value >= 60 && mobile.Skills[SkillName.Provocation].Value >= 60;
            bool musicValid = mobile.Skills[SkillName.Musicianship].Value >= 70;
            return (bardicValid && musicValid);
        }
        public override void OnUse(Mobile from)
        {
            if (!OnCooldown)
            {
                if (from.Mana < 30) {
                    from.SendMessage("You require atleast 30 mana to use this talent");
                } else {
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
                        from.Mana -= 30;
                        from.RevealingAction();
                        bool success = false;
                        BaseTalent sonicAffinity = ((PlayerMobile)from).GetTalent(typeof(SonicAffinity));
                        BaseTalent resonance = ((PlayerMobile)from).GetTalent(typeof(Resonance));
                        int seconds = 10;
                        foreach (var other in from.GetMobilesInRange(Level + 3))
                        {
                            if (other == from || !other.CanBeHarmful(from, false) ||
                                Core.AOS && !other.InLOS(from) || !(other is BaseCreature) )
                                {
                                    continue;
                                }
                            var diff = instrument.GetDifficultyFor(other) - 10.0;
                            if (sonicAffinity != null)
                            {
                                diff -= sonicAffinity.ModifySpellScalar();
                            }
                            if (from.Skills.Musicianship.Value > 100.0)
                            {
                                diff -= (from.Skills.Musicianship.Value - 100.0) * 0.5;
                            }
                            if (!BaseInstrument.CheckMusicianship(from))
                            {
                                from.SendLocalizedMessage(500612); // You play poorly, and there is no effect.
                            }
                            else
                            {
                                int attempts = 10;
                                if (resonance != null) {
                                    resonance.CheckHitEffect(from, other, 0);
                                }
                                while (!success) {
                                    attempts++;
                                    Mobile nearby = Blindness.RandomNearbyMobile(other, Level + 3);
                                    if (nearby != null && other.CanBeHarmful(nearby) && other.InLOS(nearby)) {
                                        ((BaseCreature)other).Provoke(from, nearby, true);
                                        success = true;
                                    }
                                    if (attempts > 10) {
                                        ((BaseCreature)other).Pacify(from, Core.Now + TimeSpan.FromSeconds(seconds));
                                        success = true;
                                    }
                                }
                            }
                        }
                        if (success)
                        {
                            instrument.PlayInstrumentWell(from);
                        } else
                        {
                            instrument.PlayInstrumentBadly(from);
                        }
                        instrument.ConsumeUse(from);      
                        OnCooldown = true;
                        Timer.StartTimer(TimeSpan.FromSeconds(180 - (Level * 10)), ExpireTalentCooldown, out _talentTimerToken);
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

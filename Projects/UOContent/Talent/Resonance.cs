using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using System;

namespace Server.Talent
{
    public class Resonance : BaseTalent, ITalent
    {
        public Resonance() : base()
        {
            TalentDependency = typeof(SonicAffinity);
            DisplayName = "Resonance";
            CanBeUsed = true; 
            Description = "Chance on barding success that target is damaged by sonic energy. AOE damage effect on use - 1m cooldown.";
            ImageID = 180;
        }
        public override bool HasSkillRequirement(Mobile mobile)
        {
            return (mobile.Skills.Musicianship.Value >= 60.0);
        }
        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            target.Damage(Level * 2, attacker);
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown)
            {
                BaseTalent sonicAffinity = ((PlayerMobile)from).GetTalent(typeof(SonicAffinity));
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
                if (from.Mana < 20) {
                    from.SendMessage("You require atleast 20 mana to use this talent");
                }
                else if (instrument != null)
                {
                    from.Mana -= 20;
                    OnCooldown = true;
                    foreach (var other in from.GetMobilesInRange(Level + 3))
                    {
                        if (other == from || (other is PlayerMobile && other.Karma > 0) || !other.CanBeHarmful(from, false) ||
                            Core.AOS && !other.InLOS(from))
                        {
                            continue;
                        }
                        from.RevealingAction();
                        var diff = instrument.GetDifficultyFor(other) - 10.0;
                        if (from.Skills.Musicianship.Value > 100.0)
                        {
                            diff -= (from.Skills.Musicianship.Value - 100.0) * 0.5;
                        }
                        if (!BaseInstrument.CheckMusicianship(from))
                        {
                            from.NextSkillTime = Core.TickCount + 5000;
                            from.SendLocalizedMessage(500612); // You play poorly, and there is no effect.
                            instrument.PlayInstrumentBadly(from);
                            instrument.ConsumeUse(from);
                        }
                        else
                        {
                            int baseDamage = Level;
                            if (sonicAffinity != null)
                            {
                                // increase damage further by 3 points per level of sonic affinity
                                baseDamage += sonicAffinity.Level * 3;
                            }
                            double scalar = (from.Skills.Musicianship.Value / 200);
                            int amount = AOS.Scale(baseDamage, 100 + (int)(scalar * 10));
                            if (Core.AOS)
                            {
                                AOS.Damage(other, baseDamage, 0, 0, 0, 0, 100);
                            } else
                            {
                                other.Damage(baseDamage, from);
                            }
                        }

                    }
                    Timer.StartTimer(TimeSpan.FromSeconds(60), ExpireTalentCooldown, out _talentTimerToken);
                }
                else
                {
                    from.SendMessage("You require an instrument to use this talent");
                }
            }
        }
    }
}

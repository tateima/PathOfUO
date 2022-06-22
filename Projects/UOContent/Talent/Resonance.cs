using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Talent
{
    public class Resonance : BaseTalent
    {
        public Resonance()
        {
            TalentDependency = typeof(SonicAffinity);
            DisplayName = "Resonance";
            CanBeUsed = true;
            CooldownSeconds = 30;
            ManaRequired = 25;
            Description =
                "Chance on barding success that target is damaged by sonic energy. AOE damage effect on use. Requires 60+ music.";
            AdditionalDetail = "Each level increases area of effect and damage by 1";
            ImageID = 180;
            AddEndY = 125;
        }

        public override bool HasSkillRequirement(Mobile mobile) => mobile.Skills.Musicianship.Value >= 60.0;

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            target.Damage(Utility.RandomMinMax(1, Level) * 2, attacker);
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown)
            {
                var sonicAffinity = ((PlayerMobile)from).GetTalent(typeof(SonicAffinity));
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

                if (from.Mana < ManaRequired)
                {
                    from.SendMessage($"You require at least {ManaRequired.ToString()} mana to use this talent");
                }
                else if (instrument != null)
                {
                    ApplyManaCost(from);
                    OnCooldown = true;
                    var success = false;
                    foreach (var other in from.GetMobilesInRange(Level + 3))
                    {
                        if (other == from || !other.CanBeHarmful(from, false) ||
                            Core.AOS && !other.InLOS(from))
                        {
                            continue;
                        }

                        if (!BaseInstrument.CheckMusicianship(from))
                        {
                            from.SendLocalizedMessage(500612); // You play poorly, and there is no effect.
                        }
                        else
                        {
                            success = true;
                            var baseDamage = Level;
                            if (sonicAffinity != null)
                            {
                                // increase damage further by 3 points per level of sonic affinity
                                baseDamage += sonicAffinity.Level * 3;
                            }

                            var scalar = from.Skills.Musicianship.Value / 200;
                            AOS.Scale(baseDamage, 100 + (int)(scalar * 10));
                            if (Core.AOS)
                            {
                                AOS.Damage(other, baseDamage, 0, 0, 0, 0, 100);
                            }
                            else
                            {
                                other.Damage(baseDamage, from);
                            }

                            other.FixedParticles(0x376A, 9, 32, 5007, EffectLayer.Waist);
                        }
                    }

                    from.RevealingAction();
                    if (success)
                    {
                        instrument.PlayInstrumentWell(from);
                    }
                    else
                    {
                        instrument.PlayInstrumentBadly(from);
                    }

                    instrument.ConsumeUse(from);
                    Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
                }
                else
                {
                    from.SendMessage("You require an instrument to use this talent");
                }
            }
        }
    }
}

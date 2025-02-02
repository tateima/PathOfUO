using System;
using Server.Collections;
using Server.Items;
using Server.Mobiles;

namespace Server.Talent
{
    public class Resonance : BaseTalent
    {
        public Resonance()
        {
            TalentDependencies = new[] { typeof(SonicAffinity) };
            DisplayName = "Resonance";
            CanBeUsed = true;
            CooldownSeconds = 30;
            ManaRequired = 25;
            Description =
                "Chance on barding success that target is damaged by sonic energy. AOE damage effect on use. Requires 60+ music.";
            AdditionalDetail = "Each level increases area of effect and damage by 1";
            MaxLevel = 5;
            ImageID = 180;
            AddEndY = 125;
        }

        public override bool HasSkillRequirement(Mobile mobile) => mobile.Skills.Musicianship.Value >= 60.0;

        public override void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
            if (HasSkillRequirement(attacker))
            {
                damage += Utility.RandomMinMax(1, Level) * 2;
                AlterDamage(target, (PlayerMobile)attacker, ref damage);
                if (Core.AOS)
                {
                    AOS.Damage(
                        target,
                        attacker,
                        damage,
                        true,
                        0,
                        0,
                        0,
                        0,
                        100
                    );
                }
                else
                {
                    target.Damage(damage, attacker);
                }
            }
        }

        public int SlayerDamage(int baseDamage, SlayerName slayer, Mobile other)
        {
            var entry = SlayerGroup.GetEntryByName(slayer);

            if (entry != null)
            {
                if (entry.Slays(other))
                {
                    baseDamage += Utility.RandomMinMax(1, 10);
                }
                else if (entry.Group.OppositionSuperSlays(other))
                {
                    baseDamage += Utility.RandomMinMax(10, 20);
                }
            }

            return baseDamage;
        }
        public override void OnUse(Mobile from)
        {
            if (!OnCooldown && HasSkillRequirement(from))
            {
                var sonicAffinity = ((PlayerMobile)from).GetTalent(typeof(SonicAffinity));
                BaseInstrument instrument = null;
                Container.FindItemsByTypeEnumerator<Item> instruments = from.Backpack.FindItemsByType(typeof(BaseInstrument));
                foreach (var packInstrument in instruments)
                {
                    if (((BaseInstrument)packInstrument).UsesRemaining > 0)
                    {
                        instrument = (BaseInstrument)packInstrument;
                        break;
                    }
                }
                if (from.Mana < ManaRequired)
                {
                    from.SendMessage($"You require at least {ManaRequired.ToString()} mana to use this talent");
                }
                else if (instrument != null)
                {
                    ApplyManaCost(from);
                    OnCooldown = true;
                    var success = false;
                    using var queue = PooledRefQueue<Mobile>.Create();
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
                            queue.Enqueue(other);
                        }
                    }

                    while (queue.Count > 0)
                    {
                        var other = queue.Dequeue();
                        success = true;
                        var baseDamage = Level;
                        if (sonicAffinity != null)
                        {
                            // increase damage further by 3 points per level of sonic affinity
                            baseDamage += sonicAffinity.Level * 3;
                        }

                        if (instrument.Slayer != SlayerName.None)
                        {
                            baseDamage = SlayerDamage(baseDamage, instrument.Slayer, other);
                        }

                        if (instrument.Slayer2 != SlayerName.None)
                        {
                            baseDamage = SlayerDamage(baseDamage, instrument.Slayer2, other);
                        }

                        var scalar = from.Skills.Musicianship.Value / 200;
                        AOS.Scale(baseDamage, 100 + (int)(scalar * 10));
                        AlterDamage(other, (PlayerMobile)from, ref baseDamage);
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
            else
            {
                from.SendMessage(FailedRequirements);
            }
        }
    }
}

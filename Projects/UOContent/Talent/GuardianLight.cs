using Server.Mobiles;
using Server.Spells;
using Server.Items;
using System;

namespace Server.Talent
{
    public class GuardianLight : BaseTalent, ITalent
    {
        public GuardianLight() : base()
        {
            TalentDependency = typeof(LightAffinity);
            HasDefenseEffect = true;
            DisplayName = "Guardian of light";
            Description = "Chance to be healed or cured when damaged, increases holy avenger AOE.";
            ImageID = 30032;
        }
        public override void CheckDefenseEffect(Mobile defender, Mobile attacker, int damage)
        {
            if (defender is PlayerMobile player)
            {
                BaseTalent lightAffinity = player.GetTalent(typeof(LightAffinity));
                if (Utility.Random(100) < Level)
                {
                    if (defender.Poisoned)
                    {
                        defender.CurePoison(defender);
                        defender.SendLocalizedMessage(1010059); // You have been cured of all poisons.
                        defender.FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Waist);
                        defender.PlaySound(0x1E0);
                    }
                    else
                    {
                        int lightLevel = (lightAffinity != null) ? lightAffinity.Level : 1;
                        int healAmount = AOS.Scale(damage, lightLevel);
                        defender.Heal(healAmount);
                        defender.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                        defender.PlaySound(0x202);
                        if (!OnCooldown)
                        {
                            // if defender has holy avenger, reflect heal as damage back to area
                            BaseTalent holyAvenger = player.GetTalent(typeof(HolyAvenger));
                            if (holyAvenger != null)
                            {
                                OnCooldown = true;
                                foreach (var mobile in defender.GetMobilesInRange(Level))
                                {
                                    if (defender == mobile || (mobile is PlayerMobile && mobile.Karma > 0) || !defender.CanBeHarmful(mobile, false) ||
                                        Core.AOS && !defender.InLOS(mobile))
                                    {
                                        continue;
                                    }
                                    defender.DoHarmful(mobile);
                                    mobile.Damage(healAmount, defender);
                                    Effects.SendLocationParticles(
                                        EffectItem.Create(new Point3D(mobile.X, mobile.Y, mobile.Z), mobile.Map, EffectItem.DefaultDuration),
                                        0x37C4,
                                        1,
                                        29,
                                        0x47D,
                                        2,
                                        9502,
                                        0
                                    );
                                }
                                Timer.StartTimer(TimeSpan.FromSeconds((double)(15-(holyAvenger.Level))), ExpireTalentCooldown, out _talentTimerToken);
                            }
                        }
                    }
                }
            }
        }

    }
}

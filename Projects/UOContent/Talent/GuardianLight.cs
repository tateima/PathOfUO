using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Talent
{
    public class GuardianLight : BaseTalent
    {
        public GuardianLight()
        {
            TalentDependency = typeof(LightAffinity);
            HasDefenseEffect = true;
            DisplayName = "Guardian of light";
            Description = "Chance to be healed or cured when damaged, increases holy avenger AOE.";
            AdditionalDetail = "Each level from light affinity will increase the heal amount by 5%";
            ImageID = 34;
            CooldownSeconds = 25;
            GumpHeight = 75;
            AddEndY = 105;
        }

        public override void CheckDefenseEffect(Mobile defender, Mobile target, int damage)
        {
            if (defender is PlayerMobile player && !OnCooldown)
            {
                OnCooldown = true;
                var lightAffinity = player.GetTalent(typeof(LightAffinity));
                if (defender.Poisoned)
                {
                    defender.CurePoison(defender);
                    defender.SendLocalizedMessage(1010059); // You have been cured of all poisons.
                    defender.FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Waist);
                    defender.PlaySound(0x1E0);
                }
                else
                {
                    var lightLevel = lightAffinity?.Level ?? 1;
                    var healAmount = AOS.Scale(damage, lightLevel * 5);
                    if (healAmount < 1)
                    {
                        healAmount = 1;
                    }
                    defender.Heal(healAmount);
                    defender.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                    defender.PlaySound(0x202);
                    // if defender has holy avenger, reflect heal as damage back to area
                    var holyAvenger = player.GetTalent(typeof(HolyAvenger));
                    if (holyAvenger != null)
                    {
                        foreach (var mobile in defender.GetMobilesInRange(Level))
                        {
                            if (defender == mobile || mobile is PlayerMobile && mobile.Karma > 0 ||
                                !defender.CanBeHarmful(mobile, false) ||
                                Core.AOS && !defender.InLOS(mobile))
                            {
                                continue;
                            }
                            mobile.Damage(healAmount, defender);
                            Effects.SendLocationParticles(
                                EffectItem.Create(
                                    new Point3D(mobile.X, mobile.Y, mobile.Z),
                                    mobile.Map,
                                    EffectItem.DefaultDuration
                                ),
                                0x37C4,
                                1,
                                29,
                                0x47D,
                                2,
                                9502,
                                0
                            );
                        }
                    }
                }
                Timer.StartTimer(
                    TimeSpan.FromSeconds(CooldownSeconds),
                    ExpireTalentCooldown,
                    out _talentTimerToken
                );
            }
        }
    }
}

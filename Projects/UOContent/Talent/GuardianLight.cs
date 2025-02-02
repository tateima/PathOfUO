using System;
using Server.Collections;
using Server.Items;
using Server.Mobiles;
using Server.Pantheon;

namespace Server.Talent
{
    public class GuardianLight : BaseTalent
    {
        public GuardianLight()
        {
            TalentDependencies = new[] { typeof(LightAffinity) };
            HasDefenseEffect = true;
            DisplayName = "Guardian of light";
            Description = "Chance to be healed or cured when damaged, increases holy avenger AOE.";
            MaxLevel = 5;
            AdditionalDetail = "Each level from light affinity will increase the heal amount by 2-5 points. Each level in Guardian of light will decrease the cooldown by 10 seconds.";
            ImageID = 34;
            CooldownSeconds = 50;
            CanBeUsed = true;
            GumpHeight = 75;
            AddEndY = 105;
            MaxLevel = 3;
            AddEndAdditionalDetailsY = 100;
        }

        public void StartTimer()
        {
            OnCooldown = true;
            Timer.StartTimer(
                TimeSpan.FromSeconds(CooldownSeconds - Level * 10),
                ExpireTalentCooldown,
                out _talentTimerToken
            );
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown && HasSkillRequirement(from))
            {
                CheckHeal(from as PlayerMobile);
            }
            else
            {
                from.SendMessage(FailedRequirements);
            }
        }

        public bool CheckPoison(Mobile mobile)
        {
            if (mobile.Poisoned)
            {
                mobile.CurePoison(mobile);
                mobile.SendLocalizedMessage(1010059); // You have been cured of all poisons.
                mobile.FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Waist);
                mobile.PlaySound(0x1E0);
                StartTimer();
                return true;
            }
            return false;
        }

        public void CheckHeal(PlayerMobile player)
        {
            var lightAffinity = player.GetTalent(typeof(LightAffinity));
            if (!CheckPoison(player) && player.Hits < player.HitsMax)
            {
                var lightLevel = lightAffinity?.Level ?? 1;
                var healAmount = Utility.RandomMinMax(1 + (lightLevel * 2), lightLevel * 5);
                if (healAmount < 1)
                {
                    healAmount = 1;
                }

                if (healAmount + player.Hits > player.HitsMax)
                {
                    healAmount = player.HitsMax - player.Hits;
                }

                player.Heal(healAmount);
                player.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                player.PlaySound(0x202);
                using var queue = PooledRefQueue<Mobile>.Create();
                // if defender has holy avenger, reflect heal as damage back to area
                var holyAvenger = player.GetTalent(typeof(HolyAvenger));
                if (holyAvenger != null)
                {
                    foreach (var mobile in player.GetMobilesInRange(Level*2))
                    {
                        if (player == mobile || mobile is PlayerMobile && mobile.Karma > 0 ||
                            !player.CanBeHarmful(mobile, false) ||
                            Deity.AlignmentCheck(mobile, Deity.Alignment.Light, false) ||
                            Core.AOS && !player.InLOS(mobile))
                        {
                            continue;
                        }
                        queue.Enqueue(mobile);
                    }
                }

                while (queue.Count > 0)
                {
                    var mobile = queue.Dequeue();
                    AlterDamage(mobile, player, ref healAmount);
                        mobile.Damage(healAmount, player);
                        mobile.DoHarmful(player);
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
                StartTimer();
            }
        }

        public override void CheckDefenseEffect(Mobile defender, Mobile target, int damage)
        {
            if (defender is PlayerMobile player && !OnCooldown)
            {
                CheckHeal(player);
            }
        }
    }
}

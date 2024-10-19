using System;
using Server.Collections;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Fourth;

namespace Server.Talent
{
    public class DryThunderstorm : BaseTalent
    {
        private Mobile _mobile;

        public DryThunderstorm()
        {
            TalentDependencies = new[] { typeof(SpellMind) };
            DisplayName = "Thunderstorm";
            CanBeUsed = true;
            CooldownSeconds = 120;
            ManaRequired = 60;
            Description =
                "Conjures up a storm that will strike lightning bolts to any nearby enemies.";
            AdditionalDetail = "Each level in this talent will increase the number of bolts by 3 + (1-X) where X is level.";
            AddEndAdditionalDetailsY = 70;
            ImageID = 371;
        }

        public int RemainingBolts { get; set; }

        public override void OnUse(Mobile from)
        {
            if (Activated)
            {
                from.SendMessage($"{DisplayName} is already in use");
            }
            else
            {
                if (from.Mana < ManaRequired)
                {
                    from.SendMessage($"You require {ManaRequired.ToString()} mana to conjure a storm.");
                }
                else if (!Activated && !OnCooldown && HasSkillRequirement(from))
                {
                    _mobile = from;
                    Activated = true;
                    ApplyManaCost(from);
                    RemainingBolts = Level * 3 + Utility.Random(Level);
                    Timer.StartTimer(TimeSpan.FromSeconds(Utility.Random(7, 10)), CheckStorm, out _talentTimerToken);
                    from.PlaySound(0x5CE);
                }
                else
                {
                    from.SendMessage("You do not have the required skill to use this talent.");
                }
            }
        }

        public void CheckStorm()
        {
            if (RemainingBolts > 0)
            {
                using var queue = PooledRefQueue<Mobile>.Create();
                foreach (var mobile in _mobile.GetMobilesInRange(8))
                {
                    if (mobile == _mobile || mobile is PlayerMobile && mobile.Karma > 0 ||
                        !mobile.CanBeHarmful(_mobile, false) ||
                        Core.AOS && !mobile.InLOS(_mobile))
                    {
                        continue;
                    }
                    queue.Enqueue(mobile);
                    break;
                }

                while (queue.Count > 0)
                {
                    var mobile = queue.Dequeue();
                    double damage;
                    var lightning = new LightningSpell(_mobile);
                    if (Core.AOS)
                    {
                        damage = lightning.GetNewAosDamage(23, 1, 4, mobile);
                    }
                    else
                    {
                        damage = Utility.Random(12, 9);

                        if (lightning.CheckResisted(mobile))
                        {
                            damage *= 0.75;
                            mobile.SendLocalizedMessage(501783); // You feel yourself resisting magical energy.
                        }

                        damage *= lightning.GetDamageScalar(mobile);
                    }

                    mobile.BoltEffect(0);
                    SpellHelper.Damage(lightning, mobile, damage, 0, 0, 0, 0, 100);
                    _mobile.DoHarmful(mobile);
                    RemainingBolts--;
                }
                Timer.StartTimer(TimeSpan.FromSeconds(Utility.Random(7, 10)), CheckStorm, out _talentTimerToken);
            }
            else
            {
                Activated = false;
                OnCooldown = true;
                Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
            }
        }
    }
}

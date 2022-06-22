using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Pantheon;
using Server.Targeting;
using Server.Utilities;

namespace Server.Talent
{
    public class SiphonLife : BaseTalent
    {
        public SiphonLife()
        {
            DisplayName = "Siphon life";
            DeityAlignment = Deity.Alignment.Darkness;
            RequiresDeityFavor = true;
            MobilePercentagePerPoint = 10;
            CanBeUsed = true;
            Description =
                "Steals life and stamina for caster from a nearby corpse's life force.";
            AdditionalDetail = "The amount stolen increases by 10% per level. Each level also decreases cooldown by 10 seconds. The amount healed scales with corpse type.";
            ImageID = 420;
            ManaRequired = 25;
            CooldownSeconds = 150;
            GumpHeight = 230;
            AddEndY = 70;
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown)
            {
                if (from.Mana > ManaRequired)
                {
                    from.Target = new InternalTarget(this);
                }
                else
                {
                    from.SendMessage($"You need {ManaRequired.ToString()} mana to siphon life from a corpse.");
                }
            }
        }

        private class InternalTarget : Target
        {
            private readonly SiphonLife _siphonLife;
            public InternalTarget(SiphonLife siphonLife) : base(
                10,
                false,
                TargetFlags.None
            ) =>
                _siphonLife = siphonLife;


            protected override void OnTarget(Mobile from, object targeted)
            {
                from.RevealingAction();
                if (targeted is Corpse corpse)
                {
                    Mobile previousLife = null;
                    try
                    {
                        previousLife = corpse.PreviousLife.CreateInstance<Mobile>();
                    }
                    catch
                    {
                        // ignored
                    }

                    if (previousLife != null)
                    {
                        _siphonLife.OnCooldown = true;
                        int hitAmount = AOS.Scale(previousLife.HitsMax, _siphonLife.MobilePercentagePerPoint * _siphonLife.Level);
                        int stamAmount = AOS.Scale(previousLife.StamMax, _siphonLife.MobilePercentagePerPoint * _siphonLife.Level);
                        if (previousLife is PlayerMobile)
                        {
                            hitAmount = Utility.RandomMinMax(1, 75);
                            stamAmount = Utility.RandomMinMax(1, 75);
                        }
                        if (hitAmount > 75)
                        {
                            hitAmount = 75;
                        }
                        if (stamAmount > 75)
                        {
                            stamAmount = 75;
                        }
                        // 0x37CC
                        previousLife = null;
                        Effects.SendMovingParticles(
                            new Entity(Serial.Zero, new Point3D(corpse.X, corpse.Y, corpse.Z + 10), corpse.Map),
                            new Entity(Serial.Zero, new Point3D(from.X, from.Y, from.Z + 10), from.Map),
                            0x37CC,
                            1,
                            0,
                            false,
                            false,
                            0,
                            3,
                            9501,
                            1,
                            0,
                            EffectLayer.Waist,
                            0x100
                        );
                        from.Heal(hitAmount);
                        from.Stam += stamAmount;
                        from.PlaySound(0x258);
                        from.FixedParticles(0x373A, 1, 17, 9903, 15, 4, EffectLayer.Head);
                        EmptyCorpse(corpse);
                        corpse.Delete();
                        Timer.StartTimer(TimeSpan.FromSeconds(_siphonLife.CooldownSeconds - _siphonLife.Level * 10), _siphonLife.ExpireTalentCooldown, out _siphonLife._talentTimerToken);
                    }
                    else
                    {
                        from.SendMessage("This corpse has no life force left.");
                    }
                }
                else
                {
                    from.SendMessage("This target is not a corpse.");
                }
            }
        }
    }
}

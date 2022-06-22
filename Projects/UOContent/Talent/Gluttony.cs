using System;
using Server.Items;
using Server.Mobiles;
using Server.Pantheon;

namespace Server.Talent
{
    public class Gluttony : BaseTalent
    {
        public Timer _internalTimer;
        public Gluttony()
        {
            DeityAlignment = Deity.Alignment.Greed;
            RequiresDeityFavor = true;
            DisplayName = "Gluttony";
            Description = "While full you fart every 3 minutes, slowing down surrounding enemies.";
            AdditionalDetail = $"Any enemies nearby have a chance of being poisoned. Each level increases the potency of the poison by 1 {PassiveDetail}";
            ImageID = 423;
            GumpHeight = 75;
            AddEndY = 100;
        }
        public override void UpdateMobile(Mobile mobile)
        {
            if (_internalTimer is null)
            {
                _internalTimer = new InternalTimer(mobile, this);
                _internalTimer.Start();
            }
        }

        private class InternalTimer : Timer
        {
            private readonly Mobile _mobile;
            private readonly Gluttony _gluttony;

            public InternalTimer(Mobile mobile, Gluttony gluttony)
                : base(TimeSpan.FromSeconds(180.0), TimeSpan.FromSeconds(180.0))
            {
                _mobile = mobile;
                _gluttony = gluttony;
            }

            protected override void OnTick()
            {
                if (_mobile.Deleted || _mobile.Map == Map.Internal)
                {
                    Stop();
                }
                else if (_mobile.Hunger >= 20)
                {
                    _mobile.SendSound(0x318);
                    foreach (var mobile in _mobile.GetMobilesInRange(3))
                    {
                        if (mobile == _mobile || !mobile.CanBeHarmful(_mobile, false) ||
                            Core.AOS && !mobile.InLOS(_mobile))
                        {
                            continue;
                        }
                        Effects.SendLocationParticles(
                            EffectItem.Create(mobile.Location, _mobile.Map, EffectItem.DefaultDuration),
                            0x3728,
                            10,
                            10,
                            2023
                        );
                        if (mobile is BaseCreature creature)
                        {
                            _gluttony.SlowCreature(creature, Utility.RandomMinMax(1,10), true);
                        } else if (mobile is PlayerMobile player)
                        {
                            player.Slow( Utility.RandomMinMax(1,10));
                        }

                        if (Utility.Random(100) < 66)
                        {
                            mobile.ApplyPoison(_mobile, Poison.GetPoison(_gluttony.Level));
                        }
                    }
                }
            }
        }
    }
}

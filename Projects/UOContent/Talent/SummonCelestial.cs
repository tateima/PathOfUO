using System;
using Server.Mobiles;
using Server.Spells;
using Server.Pantheon;

namespace Server.Talent
{
    public class SummonCelestial : BaseTalent
    {
        private BaseCreature _summoned;
        private PlayerMobile _summoner;
        private int _remainingSeconds;
        private DateTime _startSummonDate;
        private TimerExecutionToken _healTimerToken;
        public SummonCelestial()
        {
            DisplayName = "Celestial";
            DeityAlignment = Deity.Alignment.Light;
            RequiresDeityFavor = true;
            MobilePercentagePerPoint = 5;
            CanBeUsed = true;
            Description =
                "Summon a celestial being to assist you for 5 minutes.";
            AdditionalDetail = "The power of the celestial increases by 5% per level. Only players with light alignment can use this.";
            ImageID = 414;
            HasGroupKillEffect = true;
            CooldownSeconds = 600;
            _remainingSeconds = 600;
            ManaRequired = 60;
            GumpHeight = 230;
            AddEndY = 70;
        }

        public void HealTick()
        {
            if (_summoned.Alive && !_summoned.Deleted && _summoned.CanSee(_summoner) && _summoned.Mana > 20 && _summoner.Alive && _summoner.Hits < _summoner.HitsMax/2)
            {
                _summoned.Mana -= 20;
                _summoner.Heal(Utility.RandomMinMax(10, 25));
                _summoner.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                _summoner.PlaySound(0x202);
            }
            Timer.StartTimer(TimeSpan.FromSeconds(10), HealTick, out _healTimerToken);
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown && HasSkillRequirement(from))
            {
                if (from.Mana > ManaRequired)
                {
                    ApplyManaCost(from);
                    from.RevealingAction();
                    from.PublicOverheadMessage(
                        MessageType.Spell,
                        from.SpeechHue,
                        true,
                        "Mar Doth All Palo Demi Gras",
                        false
                    );
                    // its a talent, no need for animation timer, just a single animation is fine
                    from.Animate(269, 7, 1, true, false, 0);

                    var creature = (BaseCreature)ScaleMobile(new Celestial());
                    creature.SetLevel();
                    SpellHelper.Summon(creature, from, 0x217, TimeSpan.FromMinutes(6), false, false);
                    _startSummonDate = DateTime.Now;
                    EmptyCreatureBackpack(creature);
                    _summoned = creature;
                    _summoner = (PlayerMobile)from;
                    Timer.StartTimer(TimeSpan.FromSeconds(10), HealTick, out _healTimerToken);
                    Timer.StartTimer(TimeSpan.FromMinutes(5), ExpireTimer);
                    Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
                    OnCooldown = true;
                }
                else
                {
                    from.SendMessage($"You need {ManaRequired.ToString()} mana to summon a celestial.");
                }
            }
            else
            {
                from.SendMessage(FailedRequirements);
            }
        }

        private void ExpireTimer()
        {
            _healTimerToken.Cancel();
        }

        public override void ResetDeityPower()
        {
            if (_summoned?.Alive == true)
            {
                _summoned.Kill();
                ExpireTimer();
            }
        }
        public override void CheckGroupKillEffect(Mobile victim, Mobile killer)
        {
            if (OnCooldown)
            {
                _remainingSeconds = CooldownSeconds - (int)(_talentTimerToken.Next - _startSummonDate).TotalSeconds;
                _remainingSeconds -= Level + SummonerCommandLevel(killer);
                if (_remainingSeconds <= 0)
                {
                    ExpireTalentCooldown();
                    if (_talentTimerToken.Running)
                    {
                        _talentTimerToken.Cancel();
                    }
                    _remainingSeconds = CooldownSeconds;
                }
            }
        }
    }
}

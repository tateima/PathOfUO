using System;
using Server.Mobiles;
using Server.Spells;
using Server.Pantheon;

namespace Server.Talent
{
    public class SummonChaosElemental : BaseTalent
    {
        private BaseCreature _summoned;
        private int _remainingSeconds;
        private DateTime _startSummonDate;
        public SummonChaosElemental()
        {
            DisplayName = "Chaos elemental";
            DeityAlignment = Deity.Alignment.Chaos;
            RequiresDeityFavor = true;
            MobilePercentagePerPoint = 5;
            CanBeUsed = true;
            Description =
                "Summon a chaos elemental to assist you for 5 minutes.";
            AdditionalDetail = "The power of the chaotic elemental increases by 5% per level. Only players with chaotic alignment can use this.";
            ImageID = 347;
            HasGroupKillEffect = true;
            CooldownSeconds = 600;
            _remainingSeconds = 600;
            ManaRequired = 75;
            GumpHeight = 230;
            AddEndY = 70;
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
                        "Kal Vas Xen Anox Demi Gras",
                        false
                    );
                    // its a talent, no need for animation timer, just a single animation is fine
                    from.Animate(269, 7, 1, true, false, 0);

                    var creature = (BaseCreature)ScaleMobile(new ChaosElemental());
                    creature.SetLevel();
                    SpellHelper.Summon(creature, from, 0x217, TimeSpan.FromMinutes(6), false, false);
                    EmptyCreatureBackpack(creature);
                    _summoned = creature;
                    Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
                    _startSummonDate = DateTime.Now;
                    OnCooldown = true;
                }
                else
                {
                    from.SendMessage($"You need {ManaRequired.ToString()} mana to summon a chaos elemental.");
                }
            }
            else
            {
                from.SendMessage(FailedRequirements);
            }
        }

        public override void ResetDeityPower()
        {
            if (_summoned?.Alive == true)
            {
                _summoned.Kill();
            }
        }
        public override void CheckGroupKillEffect(Mobile victim, Mobile killer)
        {
            if (OnCooldown)
            {
                _remainingSeconds = CooldownSeconds - (int)(_talentTimerToken.Next - _startSummonDate).TotalSeconds;
                _remainingSeconds -= Level + + SummonerCommandLevel(killer);
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

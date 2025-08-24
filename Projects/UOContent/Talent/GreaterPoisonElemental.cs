using System;
using Server.Mobiles;
using Server.Spells;

namespace Server.Talent
{
    public class GreaterPoisonElemental : BaseTalent
    {
        private int _remainingSeconds;
        private DateTime _startSummonDate;
        public GreaterPoisonElemental()
        {
            TalentDependencies = new[] { typeof(WyvernAspect) };
            DisplayName = "Poison Elemental";
            MobilePercentagePerPoint = 15;
            CanBeUsed = true;
            ManaRequired = 65;
            CooldownSeconds = 600;
            _remainingSeconds = 600;
            HasGroupKillEffect = true;
            Description = "Summon a poison elemental to assist you for 2 minutes.";
            ImageID = 390;
            MaxLevel = 1;
            GumpHeight = 230;
            AddEndY = 125;
        }

        public override bool HasSkillRequirement(Mobile mobile) => mobile.Skills.Poisoning.Base >= 85;

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
                        "Nox Vas Xen Apoch Gras",
                        false
                    );
                    // its a talent, no need for animation timer, just a single animation is fine
                    from.Animate(269, 7, 1, true, false, 0);
                    var creature = new PoisonElemental
                    {
                        OverrideDispellable = true
                    };
                    creature.SetLevel();
                    SpellHelper.Summon(
                        creature,
                        from,
                        0x217,
                        TimeSpan.FromMinutes(4),
                        false,
                        false
                    ); // dont scale because they're already quite powerful
                    EmptyCreatureBackpack(creature);
                    Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
                    _startSummonDate = DateTime.Now;
                    OnCooldown = true;
                }
                else
                {
                    from.SendMessage($"You need {ManaRequired.ToString()} mana to summon this poison lord.");
                }
            }
            else
            {
                from.SendMessage(FailedRequirements);
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

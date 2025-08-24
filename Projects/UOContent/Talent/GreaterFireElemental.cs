using System;
using Server.Mobiles;
using Server.Spells;

namespace Server.Talent
{
    public class GreaterFireElemental : BaseTalent
    {
        private int _remainingSeconds;
        private DateTime _startSummonDate;
        public GreaterFireElemental()
        {
            BlockedBy = new[] { typeof(MasterOfDeath), typeof(HolyAvenger) };
            TalentDependencies = new[] { typeof(DragonAspect) };
            DisplayName = "Greater fire lord";
            MobilePercentagePerPoint = 15;
            CanBeUsed = true;
            Description =
                "Summon a fire lord to assist you for 2 minutes. Hits will be used if no mana is available.";
            AdditionalDetail = "The power of the fire lord increases by 15% per level.";
            HasGroupKillEffect = true;
            ImageID = 347;
            CooldownSeconds = 600;
            _remainingSeconds = 600;
            ManaRequired = 50;
            GumpHeight = 230;
            AddEndY = 145;
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown && HasSkillRequirement(from))
            {
                var canCast = true;
                if (from.Mana < ManaRequired && from.Hits >= 26)
                {
                    from.Hits -= 25;
                }
                else if (from.Mana > ManaRequired)
                {
                    ApplyManaCost(from);
                }
                else
                {
                    canCast = false;
                    from.SendMessage($"You need either {ManaRequired.ToString()} mana or 26 hit points to summon this fire lord.");
                }

                if (canCast)
                {
                    from.RevealingAction();
                    from.PublicOverheadMessage(
                        MessageType.Spell,
                        from.SpeechHue,
                        true,
                        "Kal Vas Xen Flam Apoch Gras",
                        false
                    );
                    // its a talent, no need for animation timer, just a single animation is fine
                    from.Animate(269, 7, 1, true, false, 0);

                    if (Core.AOS)
                    {
                        var creature = (BaseCreature)ScaleMobile(new SummonedFireElemental());
                        creature.Name = "a greater fire lord";
                        creature.OverrideDispellable = true;
                        creature.SetLevel();
                        SpellHelper.Summon(creature, from, 0x217, TimeSpan.FromMinutes(4), false, false);
                    }
                    else
                    {
                        var creature = (BaseCreature)ScaleMobile(new FireElemental());
                        creature.Name = "a greater fire lord";
                        creature.OverrideDispellable = true;
                        creature.SetLevel();
                        SpellHelper.Summon(creature, from, 0x217, TimeSpan.FromMinutes(4), false, false);
                    }
                    _startSummonDate = DateTime.Now;
                    Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
                    OnCooldown = true;
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

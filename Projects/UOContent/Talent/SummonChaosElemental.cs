using System;
using Server.Mobiles;
using Server.Spells;
using Server.Network;
using Server.Pantheon;

namespace Server.Talent
{
    public class SummonChaosElemental : BaseTalent
    {
        private BaseCreature _summoned;
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
            CooldownSeconds = 300;
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
                    SpellHelper.Summon(creature, from, 0x217, TimeSpan.FromMinutes(5), false, false);
                    EmptyCreatureBackpack(creature);
                    _summoned = creature;
                    Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
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
    }
}

using System;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Talent
{
    public class HolyBolt : BaseTalent
    {
        public HolyBolt()
        {
            TalentDependency = typeof(HolyAvenger);
            CanBeUsed = true;
            DisplayName = "Holy bolt";
            Description = "Fires a bolt of pure holy damage towards a target. Requires 85+ chivalry.";
            AdditionalDetail = "Each level increases damage or healing done by 3 points.";
            CooldownSeconds = 30;
            ManaRequired = 30;
            ImageID = 389;
            AddEndY = 115;
        }

        public override bool HasSkillRequirement(Mobile mobile) =>
            mobile.Skills[SkillName.Chivalry].Base >= 85; //&& mobile.Karma > 15000;

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown)
            {
                if (from.Mana < ManaRequired)
                {
                    from.SendMessage($"Holy bolt requires {ManaRequired.ToString()} mana to use.");
                }
                else
                {
                    from.PublicOverheadMessage(MessageType.Spell, from.SpeechHue, true, "In Hoth Hol", false);
                    from.Animate(269, 7, 1, true, false, 0);
                    from.Target = new InternalTarget(this, Level);
                }
            }
        }

        private class InternalTarget : Target
        {
            private readonly int _level;
            private readonly BaseTalent _talent;

            public InternalTarget(BaseTalent talent, int level) : base(
                8,
                false,
                TargetFlags.None
            )
            {
                _talent = talent;
                _level = level;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                from.RevealingAction();
                if (targeted is Mobile mobile && from.CanBeHarmful(mobile, true))
                {
                    _talent.ApplyManaCost(from);
                    Effects.SendMovingParticles(
                        from,
                        mobile,
                        0x36D4,
                        7,
                        0,
                        false,
                        true,
                        0x6AE,
                        0,
                        9502,
                        4019,
                        0x160,
                        0
                    );
                    from.SendSound(0x1E0);
                    if (from == mobile)
                    {
                        SpellHelper.Heal(_level * 3, from, from, false);
                    }
                    else
                    {
                        if (Core.AOS)
                        {
                            AOS.Damage(mobile, _level * 3, true, 0, 100, 0, 0, 0);
                        }
                        else
                        {
                            mobile.Damage(_level * 3, from);
                        }
                    }

                    _talent.OnCooldown = true;
                    Timer.StartTimer(TimeSpan.FromSeconds(_talent.CooldownSeconds), _talent.ExpireTalentCooldown, out _talent._talentTimerToken);
                }
                else
                {
                    from.SendMessage("You can't use this holy bolt on that!");
                }
            }
        }
    }
}

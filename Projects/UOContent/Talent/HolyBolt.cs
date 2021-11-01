using System;
using Server.Targeting;
using Server.Spells;
using Server.Network;


namespace Server.Talent
{
    public class HolyBolt : BaseTalent, ITalent
    {
        public HolyBolt() : base()
        {
            TalentDependency = typeof(HolyAvenger);
            CanBeUsed = true;
            DisplayName = "Holy bolt";
            Description = "Fires a bolt of pure holy damage towards a target. 1 minute cooldown.";
            ImageID = 389;
            AddEndY = 110;
        }
        public override bool HasSkillRequirement(Mobile mobile)
        {
            return mobile.Skills[SkillName.Chivalry].Base >= 85; //&& mobile.Karma > 15000;
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown)
            {
                if (from.Mana < 30) {
                    from.SendMessage("Holy bolt requires 30 mana to use.");
                } else {
                    from.PublicOverheadMessage(MessageType.Spell, from.SpeechHue, true, "Inluv subO", false);
                    from.Animate(203, 7, 2, true, true, 0);
                    from.Target = new InternalTarget(from, this, Level);
                }
            }
        }
        private class InternalTarget : Target
        {
            private BaseTalent m_Talent;
            private readonly int m_Level;
            public InternalTarget(Mobile from, BaseTalent talent, int level) : base(
                8,
                false,
                TargetFlags.None
            )
            {
                m_Talent = talent;
                m_Level = level;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                from.RevealingAction();

                if (targeted is Mobile mobile && from.CanBeHarmful(mobile, true))
                {
                    from.Mana -= 30;
                    Effects.SendMovingParticles(
                        from,
                        mobile,
                        0x36D4,
                        7,
                        0,
                        false,
                        true,
                        0x100,
                        0,
                        9502,
                        4019,
                        0x160,
                        0
                    );
                    from.SendSound(0x1E0);
                    if (from == mobile) {
                        SpellHelper.Heal(m_Level * 3, from, from, false);
                    } else {
                        if (Core.AOS) {
                            AOS.Damage(mobile, m_Level * 3, true, 0, 100, 0, 0, 0);
                        } else {
                            mobile.Damage(m_Level * 3, from);
                        }   
                    }
                    m_Talent.OnCooldown = true;
                    Timer.StartTimer(TimeSpan.FromSeconds(60), m_Talent.ExpireTalentCooldown);
                }
                else
                {
                    from.SendMessage("You can't use this holy bolt on that!");
                }
            }
        }
    }
}

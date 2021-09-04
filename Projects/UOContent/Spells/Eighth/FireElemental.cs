using System;
using Server.Mobiles;
using Server.Talent;

namespace Server.Spells.Eighth
{
    public class FireElementalSpell : MagerySpell
    {
        private static readonly SpellInfo m_Info = new(
            "Fire Elemental",
            "Kal Vas Xen Flam",
            269,
            9050,
            false,
            Reagent.Bloodmoss,
            Reagent.MandrakeRoot,
            Reagent.SpidersSilk,
            Reagent.SulfurousAsh
        );

        public FireElementalSpell(Mobile caster, Item scroll = null) : base(caster, scroll, m_Info)
        {
        }
        public override bool RequiresReagents => true;
        public override SpellCircle Circle => SpellCircle.Eighth;

        public override bool CheckCast()
        {
            if (!base.CheckCast())
            {
                return false;
            }

            if (Caster.Followers + 4 > Caster.FollowersMax)
            {
                Caster.SendLocalizedMessage(1049645); // You have too many followers to summon that creature.
                return false;
            }

            return true;
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                var duration = TimeSpan.FromSeconds(2 * Caster.Skills.Magery.Fixed / 5);              

                if (Core.AOS)
                {
                    SpellHelper.Summon((BaseCreature)CheckFireAffinity(new SummonedFireElemental()), Caster, 0x217, duration, false, false);
                }
                else
                {
                    SpellHelper.Summon((BaseCreature)CheckFireAffinity(new FireElemental()), Caster, 0x217, duration, false, false);
                }
            }

            FinishSequence();
        }

        public Mobile CheckFireAffinity(Mobile summon)
        {
            if (Caster is PlayerMobile player)
            {
                BaseTalent fireAffinity = player.GetTalent(typeof(FireAffinity));
                if (fireAffinity != null)
                {
                    return fireAffinity.ScaleMobileStats(summon);
                }
            }
            return summon;
        }
    }
}

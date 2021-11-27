using System;
using Server.Mobiles;

namespace Server.Spells.Eighth
{
    public class EarthElementalSpell : MagerySpell
    {
        private static readonly SpellInfo _info = new(
            "Earth Elemental",
            "Kal Vas Xen Ylem",
            269,
            9020,
            false,
            Reagent.Bloodmoss,
            Reagent.MandrakeRoot,
            Reagent.SpidersSilk
        );

        public EarthElementalSpell(Mobile caster, Item scroll = null) : base(caster, scroll, _info)
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

            if (Caster.Followers + 2 > Caster.FollowersMax)
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
                var duration = TimeSpan.FromSeconds(2 * Caster.Skills.Magery.Fixed / 5.0);

                if (Core.AOS)
                {
                    SpellHelper.Summon(new SummonedEarthElemental(), Caster, 0x217, duration, false, false);
                }
                else
                {
                    SpellHelper.Summon(new EarthElemental(), Caster, 0x217, duration, false, false);
                }
            }

            FinishSequence();
        }
    }
}

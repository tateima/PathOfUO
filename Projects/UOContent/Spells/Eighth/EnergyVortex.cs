using System;
using Server.Mobiles;

namespace Server.Spells.Eighth
{
    public class EnergyVortexSpell : MagerySpell, ISpellTargetingPoint3D
    {
        private static readonly SpellInfo m_Info = new(
            "Energy Vortex",
            "Vas Corp Por",
            260,
            9032,
            false,
            Reagent.Bloodmoss,
            Reagent.BlackPearl,
            Reagent.MandrakeRoot,
            Reagent.Nightshade
        );

        public EnergyVortexSpell(Mobile caster, Item scroll = null) : base(caster, scroll, m_Info)
        {
        }
        public override bool RequiresReagents => true;
        public override SpellCircle Circle => SpellCircle.Eighth;

        public void Target(IPoint3D p)
        {
            var map = Caster.Map;

            SpellHelper.GetSurfaceTop(ref p);

            if (map?.CanSpawnMobile(p.X, p.Y, p.Z) != true)
            {
                Caster.SendLocalizedMessage(501942); // That location is blocked.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                TimeSpan duration;

                if (Core.AOS)
                {
                    duration = TimeSpan.FromSeconds(90.0);
                }
                else
                {
                    duration = TimeSpan.FromSeconds(Utility.Random(80, 40));
                }

                BaseCreature.Summon(new EnergyVortex(), false, Caster, new Point3D(p), 0x212, duration);
            }

            FinishSequence();
        }

        public override bool CheckCast()
        {
            if (!base.CheckCast())
            {
                return false;
            }

            if (Caster.Followers + (Core.SE ? 2 : 1) > Caster.FollowersMax)
            {
                Caster.SendLocalizedMessage(1049645); // You have too many followers to summon that creature.
                return false;
            }

            return true;
        }

        public override void OnCast()
        {
            Caster.Target = new SpellTargetPoint3D(this);
        }
    }
}

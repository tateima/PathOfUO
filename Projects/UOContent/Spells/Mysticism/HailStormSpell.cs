using System;
using System.Collections.Generic;

namespace Server.Spells.Mysticism
{
    public class HailStormSpell : MysticSpell, ISpellTargetingPoint3D
    {
        private static readonly SpellInfo m_Info = new(
            "Hail Storm",
            "Kal Des Ylem",
            -1,
            9002,
            Reagent.DragonsBlood,
            Reagent.Bloodmoss,
            Reagent.BlackPearl,
            Reagent.MandrakeRoot
        );

        public HailStormSpell(Mobile caster, Item scroll = null)
            : base(caster, scroll, m_Info)
        {
        }

        public override TimeSpan CastDelayBase => TimeSpan.FromSeconds(2.25);

        public override double RequiredSkill => 70.0;
        public override int RequiredMana => 40;

        public void Target(IPoint3D p)
        {
            if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                /* Summons a storm of hailstones that strikes all Targets
                 * within a radius around the Target's Location, dealing
                 * cold damage.
                 */

                SpellHelper.Turn(Caster, p);

                if (p is Item item)
                {
                    p = item.GetWorldLocation();
                }

                var targets = new List<Mobile>();

                var map = Caster.Map;

                var pvp = false;

                if (map != null)
                {
                    var loc = new Point3D(p);

                    PlayEffect(loc, Caster.Map);

                    foreach (var m in map.GetMobilesInRange(loc, 2))
                    {
                        if (m == Caster)
                        {
                            continue;
                        }

                        if (SpellHelper.ValidIndirectTarget(Caster, m) && Caster.CanBeHarmful(m, false) && Caster.CanSee(m))
                        {
                            if (!Caster.InLOS(m))
                            {
                                continue;
                            }

                            targets.Add(m);

                            if (m.Player)
                            {
                                pvp = true;
                            }
                        }
                    }
                }

                double damage = GetNewAosDamage(51, 1, 5, pvp);
                int naturePower = 0;
                NatureAffinityPower(ref naturePower);
                damage += (double)naturePower;

                foreach (var m in targets)
                {
                    Caster.DoHarmful(m);
                    SpellHelper.Damage(this, m, damage, 0, 0, 100, 0, 0);
                }
            }

            FinishSequence();
        }

        public override void OnCast()
        {
            Caster.Target = new SpellTargetPoint3D(this);
        }

        private static void PlayEffect(Point3D p, Map map)
        {
            Effects.PlaySound(p, map, 0x64F);

            PlaySingleEffect(p, map, -1, 1, -1, 1);
            PlaySingleEffect(p, map, -2, 0, -3, -1);
            PlaySingleEffect(p, map, -3, -1, -1, 1);
            PlaySingleEffect(p, map, 1, 3, -1, 1);
            PlaySingleEffect(p, map, -1, 1, 1, 3);
        }

        private static void PlaySingleEffect(Point3D p, Map map, int a, int b, int c, int d)
        {
            int x = p.X, y = p.Y, z = p.Z + 18;

            SendEffectPacket(p, map, new Point3D(x + a, y + c, z), new Point3D(x + a, y + c, z));
            SendEffectPacket(p, map, new Point3D(x + b, y + c, z), new Point3D(x + b, y + c, z));
            SendEffectPacket(p, map, new Point3D(x + b, y + d, z), new Point3D(x + b, y + d, z));
            SendEffectPacket(p, map, new Point3D(x + a, y + d, z), new Point3D(x + a, y + d, z));

            SendEffectPacket(p, map, new Point3D(x + b, y + c, z), new Point3D(x + a, y + c, z));
            SendEffectPacket(p, map, new Point3D(x + b, y + d, z), new Point3D(x + b, y + c, z));
            SendEffectPacket(p, map, new Point3D(x + a, y + d, z), new Point3D(x + b, y + d, z));
            SendEffectPacket(p, map, new Point3D(x + a, y + c, z), new Point3D(x + a, y + d, z));
        }

        private static void SendEffectPacket(Point3D p, Map map, Point3D orig, Point3D dest)
        {
            Effects.SendMovingEffect(
                p,
                map,
                0x36D4,
                orig,
                dest,
                0,
                0,
                false,
                false,
                0x63,
                0x4
            );
        }
    }
}

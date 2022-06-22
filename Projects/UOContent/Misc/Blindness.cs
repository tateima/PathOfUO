using System;
using System.Collections.Generic;
using System.Linq;
using Server.Mobiles;

namespace Server
{
    public static class Blindness
    {

        public static void BlindTarget(Mobile from, int duration, string blindMessage = "* Blinded *") {
            if (from is BaseCreature creature) {
                creature.Blind(duration, blindMessage);
            } else if (from is PlayerMobile player) {
                player.Blind(duration, blindMessage);
            }
        }
        public static bool BlindMobile(Mobile from) {
            bool blinded = false;
            if (from is BaseCreature creature) {
                blinded = creature.Blinded;
            } else if (from is PlayerMobile player) {
                blinded = player.Blinded;
            }
            return blinded;
        }
        public static Mobile GetBlindTarget(Mobile mobile, int distance) {
            Mobile nearby = RandomNearbyMobile(mobile, distance);
            if (nearby != null) {
                return nearby;
            } else if (Utility.Random(100) < 10) {
                return mobile;
            }
            return null;
        }
        public static Mobile RandomNearbyMobile(Mobile from, int distance) {
            Mobile nearby = null;
            List<Mobile> viables = new List<Mobile>();
            foreach(Mobile mobile in from.GetMobilesInRange(distance)) {
                if (mobile == from || !mobile.CanBeHarmful(from, false) ||
                    Core.AOS && !mobile.InLOS(from))
                {
                    continue;
                }
                viables.Add(mobile);
            }
            if (viables.Count > 0)
            {
                nearby = viables[Utility.Random(viables.Count)];
            }

            return nearby;
        }
    }
}

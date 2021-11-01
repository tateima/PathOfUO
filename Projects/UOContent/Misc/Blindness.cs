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
            List<Mobile> mobiles = from.GetMobilesInRange(distance).ToList();
            foreach(Mobile mobile in mobiles) {
                if (mobile == from || !mobile.CanBeHarmful(from, false) ||
                    Core.AOS && !mobile.InLOS(from))
                {
                    continue;
                }
                if (Utility.Random(100) < 25) {
                    nearby = mobile;
                    break;
                } else if (mobile == mobiles.Last()) {
                    nearby = mobile;
                }
            }
            return nearby;
        }
    }
}
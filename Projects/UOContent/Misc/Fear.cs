using System;
using System.Collections.Generic;
using System.Linq;
using Server.Mobiles;

namespace Server
{
    public static class Fear
    {
        public static void FearTarget(Mobile from, int duration, string blindMessage = "* Blinded *") {
            if (from is BaseCreature creature) {
                creature.Fear(duration, blindMessage);
            } else if (from is PlayerMobile player) {
                player.Fear(duration, blindMessage);
            }
        }
        public static bool FearedMobile(Mobile from) {
            bool feared = false;
            if (from is BaseCreature creature) {
                feared = creature.Feared;
            } else if (from is PlayerMobile player) {
                feared = player.Feared;
            }
            return feared;
        }
    }
}

using System;
using Server.Mobiles;
using Server.Talent;

namespace Server
{
    public static class PlanarTravel
    {
        public const string NO_TRAVEL_MESSAGE = "You are currently suffering from planar exhaustion.";
        public static bool CanPlanarTravel(Mobile mobile) {
            return (mobile as PlayerMobile)?.NextPlanarTravel <= Core.Now;
        }
        public static void NextPlanarTravel(Mobile mobile) {
            if (mobile is PlayerMobile player) {
                AstralBorn astralBorn = player.GetTalent(typeof(AstralBorn)) as AstralBorn;
                if (astralBorn != null) {
                    player.NextPlanarTravel = Core.Now.AddMinutes(30);
                }
                else
                {
                    player.NextPlanarTravel = Core.Now.AddHours(1);
                }
                
            }
        }
    }
}

using Server.Mobiles;
using Server.Items;
using System;

namespace Server.Talent
{
    public class TwoHandedMaceSpecialist : BaseTalent, ITalent
    {
        public TwoHandedMaceSpecialist() : base()
        {
            TalentDependency = typeof(MacefightingFocus);
            RequiredWeapon = new Type[] { typeof(WarHammer) };
            DisplayName = "Warmonger";
            Description = "Increases damage to two handed macefighting weapons.";
            ImageID = 196;
            GumpHeight = 75;
            AddEndY = 85;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            target.Damage(Level * 2, attacker);
        }

    }
}

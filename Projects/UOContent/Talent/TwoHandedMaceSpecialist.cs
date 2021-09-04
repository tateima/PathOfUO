using Server.Mobiles;
using Server.Items;
using System;

namespace Server.Talent
{
    public class TwoHandedMaceSpecialist : BaseTalent, ITalent
    {
        public TwoHandedMaceSpecialist() : base()
        {
            BlockedBy = new Type[] { typeof(MaceSpecialist) };
            TalentDependency = typeof(MacefightingFocus);
            RequiredWeapon = new Type[] { typeof(WarHammer) };
            DisplayName = "Two handed macing specialist";
            Description = "Increases damage to two handed macefighting weapons.";
            ImageID = 39885;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            target.Damage(Level * 2, attacker);
        }

    }
}

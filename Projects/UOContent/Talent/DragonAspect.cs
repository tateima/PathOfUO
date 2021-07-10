using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class DragonAspect : BaseTalent, ITalent
    {
        public void UpdateMobile(Mobile m)
        {
            if (Core.AOS)
            {
                if (ReistanceMod != null)
                {
                    m.RemoveResistanceMod(ReistanceMod);
                }
                ReistanceMod = new ResistanceMod(ResistanceType.Fire, Level * 5);
                m.AddResistanceMod(ReistanceMod);
            }
        }

        public DragonAspect() : base()
        {
            BlockedBy = new Type[] { typeof(ViperAspect) };
            TalentDependency = typeof(FireAffinity);
            DisplayName = "Dragon aspect";
            Description = "Has a chance on spell cast or hit to conjure a fire breath.";
            ImageID = 39900;
        }

    }
}

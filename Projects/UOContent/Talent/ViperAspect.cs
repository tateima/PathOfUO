using Server.Mobiles;
using Server.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class ViperAspect : BaseTalent, ITalent
    {
        public void UpdateMobile(Mobile m)
        {
            if (Core.AOS)
            {
                if (ReistanceMod != null)
                {
                    m.RemoveResistanceMod(ReistanceMod);
                }
                ReistanceMod = new ResistanceMod(ResistanceType.Poison, Level * 5);
                m.AddResistanceMod(ReistanceMod);
            }
        }

        public void CheckHitEffect(Mobile attacker, Mobile target)
        {
            if (Utility.Random(100) < 2)
            {
                target.ApplyPoison(attacker, Poison.GetPoison(Level));
            }
        }

        public ViperAspect() : base()
        {
            BlockedBy = new Type[] { typeof(DragonAspect) };
            DisplayName = "Viper aspect";
            Description = "Increased poison resistance and adds a small chance to poison your target on hit.";
            ImageID = 30149;
        }

    }
}

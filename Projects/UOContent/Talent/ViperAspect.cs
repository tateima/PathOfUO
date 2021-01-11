using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    public class ViperAspect : BaseTalent, ITalent
    {
        [Description("Increases your poison resistance if appropriate and adds a chance to poison your target on hit.")]
        public static string ToString()
        {
            return "Viper aspect";
        }
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

        public void CheckEffect(Mobile attacker, Mobile target)
        {
            if (Utility.Random(100) < 2)
            {
                target.ApplyPoison(attacker, Poison.GetPoison(Level));
            }
        }

        public ViperAspect() : base()
        {
        }

    }
}

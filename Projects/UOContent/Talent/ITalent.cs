using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    interface ITalent
    {
        public void CheckEffect(Mobile m, Mobile t);
        public void UpdateMobile(Mobile m);
        public int MaxLevel { get; set; }
    }
}

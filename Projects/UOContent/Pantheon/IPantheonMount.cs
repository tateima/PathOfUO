using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;
using Server.Talent;
using Server.Utilities;
using Server.Network;

namespace Server.Pantheon
{
    public interface IPantheonMount
    {
        public Deity.Alignment Alignment { get; set; }
    }
}


using System;
using System.Collections.Generic;
using Server.Items;
using Server.Mobiles;
using Server.Talent;
using Server.Utilities;
using Server.Network;

namespace Server.Pantheon
{
    public interface IPantheonItem
    {
        public string AlignmentRaw{ get; set; }
        public int TalentIndex { get; set; }
        public BaseTalent Talent { get; set; }
        public int TalentLevel { get; set; }
    }
}


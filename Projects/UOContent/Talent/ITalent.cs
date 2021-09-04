using Server.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Talent
{
    interface ITalent
    {
        public Type[] BlockedBy { get; set; }
        public Type TalentDependency { get; set; }
        public void CheckHitEffect(Mobile m, Mobile t, int d);
        public int ModifySpellMultiplier();
        public void UpdateMobile(Mobile m);
        public bool HasSkillRequirement(Mobile m);
        public int ImageID { get; set; }
        public int AddEndY { get; set; }
        public int MaxLevel { get; set; }
    }
}

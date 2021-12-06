using System;

namespace Server.Talent
{
    internal interface ITalent
    {
        public Type[] BlockedBy { get; set; }
        public Type TalentDependency { get; set; }
        public int ImageID { get; set; }
        public int AddEndY { get; set; }
        public int MaxLevel { get; set; }
        public void UpdateMobile(Mobile mobile);
    }
}

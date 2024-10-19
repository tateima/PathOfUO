using Server.Talent;

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


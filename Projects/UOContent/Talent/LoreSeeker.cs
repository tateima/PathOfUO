using Server.Gumps;
using Server.Mobiles;
using System;
using System.Linq;
namespace Server.Talent
{
    public class LoreSeeker : BaseTalent, ITalent
    {
        private Mobile m_Mobile;
        public LoreSeeker() : base()
        {
            DisplayName = "Lore giver";
            Description = "Expose weaknesses in enemies on hit. Need 70 or above in two lore skills.";
            ImageID = 127;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override bool HasSkillRequirement(Mobile mobile) {
            SkillsGumpGroup group = SkillsGumpGroup.Groups.Where(group => group.Name == "Lore & Knowledge").FirstOrDefault();
            int validCount = 0;
            if (group != null)
            {
                foreach (SkillName skill in group.Skills)
                {
                    if (mobile.Skills[skill].Base >= 70)
                    {
                        validCount++;
                    }
                }
            }
            return validCount >= 2;
        }
        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (!OnCooldown)
            {
                OnCooldown = true;
                // reduce stats
                target.AddStatMod(new StatMod(StatType.All, "LoreSeekerDebuff", -Level, TimeSpan.FromSeconds(20)));
                // reduce random resistance
                Array values = Enum.GetValues(typeof(ResistanceType));
                ResistanceType randomResistanceType = (ResistanceType)values.GetValue(Utility.Random(values.Length));
                ResMod = new ResistanceMod(randomResistanceType, -(Level * 2));
                target.AddResistanceMod(ResMod);
                m_Mobile = target;
                Timer.StartTimer(TimeSpan.FromSeconds(30), ExpireTalentCooldown, out _talentTimerToken);
            }
        }
        public override void ExpireTalentCooldown()
        {
            base.ExpireTalentCooldown();
            if (m_Mobile != null)
            {
                m_Mobile.RemoveResistanceMod(ResMod);
            }
        }

    }
}

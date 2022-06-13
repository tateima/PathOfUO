using System;
using System.Linq;
using Server.Gumps;

namespace Server.Talent
{
    public class LoreSeeker : BaseTalent
    {
        private Mobile m_Mobile;

        public LoreSeeker()
        {
            DisplayName = "Lore seeker";
            Description = "Expose weaknesses in enemies on hit. Need 70 or above in two lore skills.";
            ImageID = 127;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override bool HasSkillRequirement(Mobile mobile)
        {
            var group = SkillsGumpGroup.Groups.FirstOrDefault(group => @group.Name == "Lore & Knowledge");
            var validCount = 0;
            if (group != null)
            {
                validCount += @group.Skills.Count(skill => mobile.Skills[skill].Base >= 70);
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
                var values = Enum.GetValues(typeof(ResistanceType));
                var randomResistanceType = (ResistanceType)values.GetValue(Utility.Random(values.Length));
                ResMod = new ResistanceMod(randomResistanceType, -(Level * 2));
                target.AddResistanceMod(ResMod);
                m_Mobile = target;
                Timer.StartTimer(TimeSpan.FromSeconds(30), ExpireTalentCooldown, out _talentTimerToken);
            }
        }

        public override void ExpireTalentCooldown()
        {
            base.ExpireTalentCooldown();
            m_Mobile?.RemoveResistanceMod(ResMod);
        }
    }
}

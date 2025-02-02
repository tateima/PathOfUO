using System;
using System.Linq;
using Server.Gumps;

namespace Server.Talent
{
    public class LoreSeeker : BaseTalent
    {
        private Mobile _mobile;

        public LoreSeeker()
        {
            DisplayName = "Lore seeker";
            Description = "Expose weaknesses in enemies on hit. Need 70 or above in one lore skill.";
            AdditionalDetail =
                "Each point in this talent will also increase the skill gain by 1 for each skill point in any lore skill. The more lore skills you have, the greater the effect of the de-buff.";
            ImageID = 127;
            CooldownSeconds = 30;
            GumpHeight = 85;
            AddEndY = 80;
            AddEndAdditionalDetailsY = 110;
        }

        public override bool HasSkillRequirement(Mobile mobile)
        {
            var group = SkillsGumpGroup.Groups.FirstOrDefault(group => group.Name == "Lore & Knowledge");
            var validCount = 0;
            if (group != null)
            {
                validCount += group.Skills.Count(skill => mobile.Skills[skill].Base >= 70);
            }

            return validCount >= 1;
        }

        public static int GetLoreModifier(Mobile attacker, int level)
        {
            var group = SkillsGumpGroup.Groups.FirstOrDefault(group => group.Name == "Lore & Knowledge");
            if (group is not null)
            {
                foreach (var skillName in group.Skills)
                {
                    level += (int)attacker.Skills[skillName].Base / 20;
                }
            }
            return level;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
            if (!OnCooldown && HasSkillRequirement(attacker))
            {
                OnCooldown = true;
                int modifier = GetLoreModifier(attacker, Level);
                // reduce stats
                target.AddStatMod(new StatMod(StatType.All, "LoreSeekerDebuff", -modifier, TimeSpan.FromSeconds(20)));
                // reduce random resistance
                var values = Enum.GetValues(typeof(ResistanceType));
                if (values.Length > 0)
                {
                    var randomResistanceType = (ResistanceType)values.GetValue(Utility.Random(values.Length));
                    ResMod = new ResistanceMod(randomResistanceType, "LoreSeeker", -(modifier * 2));
                    target.AddResistanceMod(ResMod);
                    _mobile = target;
                    Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
                }
            }
        }

        public override void ExpireTalentCooldown()
        {
            base.ExpireTalentCooldown();
            _mobile?.RemoveResistanceMod(ResMod);
        }
    }
}

using System;
using Server.Mobiles;

namespace Server.Talent
{
    public class CharmUndead : BaseTalent
    {
        private BaseCreature _charmed;

        public CharmUndead()
        {
            DisplayName = "Charm Undead";
            CanBeUsed = true;
            CooldownSeconds = 180;
            Description =
                "Allows a chance for spirit speak to charm a nearby undead. Requires 20-100 spiritspeak.";
            ImageID = 406;
            MaxLevel = 5;
            GumpHeight = 95;
            AddEndY = 100;
        }


        public override bool HasSkillRequirement(Mobile mobile)
        {
            return Level switch
            {
                0 => mobile.Skills.SpiritSpeak.Base >= 20.0,
                1 => mobile.Skills.SpiritSpeak.Base >= 40.0,
                2 => mobile.Skills.SpiritSpeak.Base >= 60.0,
                3 => mobile.Skills.SpiritSpeak.Base >= 80.0,
                4 => mobile.Skills.SpiritSpeak.Base >= 100.0,
                5 => true,
                _ => false
            };
        }

        public void ExpireCharm()
        {
            _charmed.Owners.Clear();
            _charmed.SetControlMaster(null);
            _charmed.Summoned = false;
        }

        public bool CheckCharm(Mobile from)
        {
            if (!OnCooldown && Activated)
            {
                foreach (var mobile in from.GetMobilesInRange(5))
                {
                    if (mobile == from || !from.CanBeHarmful(mobile) || mobile is PlayerMobile)
                    {
                        continue;
                    }

                    if (Utility.Random(100) < Level * 2 && mobile is BaseCreature creature &&
                        creature.DynamicExperienceValue() <= 1000 && IsMobileType(
                            OppositionGroup.UndeadGroup,
                            mobile.GetType()
                        ))
                    {
                        creature.Owners.Add(from);
                        creature.SetControlMaster(from);
                        creature.Summoned = true;
                        _charmed = creature;
                        OnCooldown = true;
                        Activated = false;
                        Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireCharm);
                        Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
                        from.SendMessage("You charm a nearby undead creature.");
                        return true;
                    }
                }
            }

            return false;
        }
    }
}

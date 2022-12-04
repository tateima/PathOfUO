using System;
using Server.Mobiles;

namespace Server.Talent
{
    public class SlaveDriver : BaseTalent
    {
        public SlaveDriver()
        {
            TalentDependency = typeof(ResourcefulHarvester);
            DisplayName = "Slave driver";
            Description = "Summon a slave to harvest resources for you.";
            AdditionalDetail =
                "This slave will respond to the commands log, ore, cloth or hide. They will then teleport and start harvesting this resource and return with your goods.  There is a chance that they will fail in this task, this chance is decreased by 1% per level.";
            CooldownSeconds = 600;
            ImageID = 360;
            CanBeUsed = true;
            MaxLevel = 10;
            GumpHeight = 85;
            AddEndY = 80;
        }

        public override void OnUse(Mobile mobile)
        {
            if (!OnCooldown && HasSkillRequirement(mobile))
            {
                var slave = new Slave();
                EmptyCreatureBackpack(slave);

                var location = mobile.Location;
                location.X += 3;
                location.Y += 3;
                slave.MoveToWorld(location, mobile.Map);
                slave.Say("I am here to serve thee!");
                slave.FixedParticles(0x376A, 9, 32, 0x13AF, EffectLayer.Waist);
                slave.PlaySound(0x1FE);
                Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
                OnCooldown = true;
            }
            else
            {
                mobile.SendMessage(FailedRequirements);
            }
        }
    }
}

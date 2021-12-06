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
            Description = "Summon a slave to harvest resources for you, 6m cooldown";
            ImageID = 360;
            MaxLevel = 6;
            GumpHeight = 85;
            AddEndY = 80;
            MaxLevel = 10;
        }

        public virtual void OnUse(Mobile mobile)
        {
            if (!OnCooldown)
            {
                var slave = new Slave();
                if (slave.Backpack != null)
                {
                    for (var x = slave.Backpack.Items.Count - 1; x >= 0; x--)
                    {
                        var item = slave.Backpack.Items[x];
                        item.Delete();
                    }
                }

                var location = mobile.Location;
                location.X += 3;
                location.Y += 3;
                slave.MoveToWorld(location, mobile.Map);
                slave.Say("I am here to serve thee!");
                slave.FixedParticles(0x376A, 9, 32, 0x13AF, EffectLayer.Waist);
                slave.PlaySound(0x1FE);
                Timer.StartTimer(TimeSpan.FromMinutes(6), ExpireTalentCooldown, out _talentTimerToken);
                OnCooldown = true;
            }
        }
    }
}

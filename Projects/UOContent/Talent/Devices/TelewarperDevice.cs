using System;
using Server;
using Server.Spells;
using Server.Mobiles;
using Server.Spells.Fourth;
using Server.Spells.Fifth;
using Server.Spells.Eighth;
using Server.Items;


namespace Server.Talent.Devices
{
    [Serializable(0, false)]

    public partial class TelewarperDevice : BaseDevice
    {
        public override double DefaultWeight
        {
            get { return 0.1; }
        }

        public override int LabelNumber { get { return 1061183; } } // A glitchy device

        [Constructible]
        public TelewarperDevice() : base(WandEffect.Device, 10, 10)
        {
            Stackable = false;
            Light = LightType.Circle150;
            Hue = 1992;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("Teleports target, but may accidentally summon creatures instead.");
        }
        public override void OnWandUse(Mobile from)
        {
            if (Parent is PlayerMobile player)
            {
                BaseTalent talent = player.GetTalent(typeof(Telewarper));
                BaseTalent bugFixer = player.GetTalent(typeof(BugFixer));
                if (talent != null)
                {
                    int modifier = 0;
                    if (bugFixer != null)
                    {
                        modifier = bugFixer.Level;
                    }
                    if (Utility.Random(100) < 6 - modifier)
                    {
                        // glitch
                        if (Utility.Random(100) < 50)
                        {
                            Cast(new SummonCreatureSpell(from, this));
                        } else
                        {
                            switch (Utility.Random(1, 5))
                            {
                                case 1:
                                    Cast(new FireElementalSpell(from, this));
                                    break;
                                case 2:
                                    Cast(new SummonDaemonSpell(from, this));
                                    break;
                                case 3:
                                    Cast(new AirElementalSpell(from, this));
                                    break;
                                case 4:
                                    Cast(new WaterElementalSpell(from, this));
                                    break;
                                case 5:
                                    Cast(new EarthElementalSpell(from, this));
                                    break;
                            }
                        }                       
                    }
                    else
                    {
                        Cast(new GreaterHealSpell(from, this));
                    }
                }
                else
                {
                    from.SendMessage("You cannot use this device.");
                }
            }
        }
    }
}

using ModernUO.Serialization;
using Server.Mobiles;
using Server.Spells.Fifth;
using Server.Spells.Eighth;
using Server.Spells.Third;
using Server.Talent;

namespace Server.Items
{
    [SerializationGenerator(0, false)]

    public partial class TelewarperDevice : BaseDevice
    {
        public override double DefaultWeight => 0.1;

        public override int LabelNumber => 1061183; // A glitchy device

        [Constructible]
        public TelewarperDevice() : base(WandEffect.Device, 10, 10)
        {
            Name = "Telewarper Device";
            Stackable = false;
            Light = LightType.Circle150;
            Hue = 1992;
        }

        public override void GetProperties(IPropertyList list)
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
                    if (Utility.Random(100) <= 6 - modifier)
                    {
                        // glitch
                        if (Utility.Random(100) <= 50)
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
                        Cast(new TeleportSpell(from, this));
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

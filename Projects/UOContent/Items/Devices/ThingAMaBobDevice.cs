using Server.Mobiles;
using Server.Spells.Fourth;
using Server.Spells.Seventh;
using Server.Spells.Sixth;
using Server.Spells.Third;
using Server.Spells.Second;
using Server.Talent;


namespace Server.Items
{
    [Serializable(0, false)]
    public partial class ThingAMaBobDevice : BaseDevice
    {
        public override double DefaultWeight => 0.1;

        public override int LabelNumber => 1061183; // A glitchy device

        [Constructible]
        public ThingAMaBobDevice() : base(WandEffect.Device, 10, 10)
        {
            Name = "Thing-a-ma-bob Device";
            Stackable = false;
            Light = LightType.Circle150;
            Hue = 1992;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("Shoots random spells, but may heal accidentally");
        }
        public override void OnWandUse(Mobile from)
        {
            if (Parent is PlayerMobile player)
            {
                BaseTalent talent = player.GetTalent(typeof(ThingAMaBob));
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
                        Cast(new GreaterHealSpell(from, this));
                    }
                    else
                    {
                        switch (Utility.Random(1,5))
                        {
                            case 1:
                                Cast(new FlameStrikeSpell(from, this));
                                break;
                            case 2:
                                Cast(new EnergyBoltSpell(from, this));
                                break;
                            case 3:
                                Cast(new LightningSpell(from, this));
                                break;
                            case 4:
                                Cast(new PoisonSpell(from, this));
                                break;
                            case 5:
                                Cast(new HarmSpell(from, this));
                                break;
                        }
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

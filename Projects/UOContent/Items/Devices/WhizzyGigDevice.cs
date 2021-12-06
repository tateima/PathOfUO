using Server.Mobiles;
using Server.Spells.Fourth;
using Server.Spells.Sixth;
using Server.Talent;

namespace Server.Items
{
    [Serializable(0, false)]
    public partial class WhizzyGigDevice : BaseDevice
    {
        public override double DefaultWeight => 0.1;

        public override int LabelNumber => 1061183; // A glitchy device

        [Constructible]
        public WhizzyGigDevice() : base(WandEffect.Device, 10, 10)
        {
            Name = "Whizzy-gig Device";
            Stackable = false;
            Light = LightType.Circle150;
            Hue = 1992;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("Heals target, but may accidentally damage them instead.");
        }
        public override void OnWandUse(Mobile from)
        {
            if (Parent is PlayerMobile player)
            {
                BaseTalent talent = player.GetTalent(typeof(WhizzyGig));
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
                        Cast(new ExplosionSpell(from, this));
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

using System;
using Server;
using Server.Mobiles;
using Server.Spells.First;
using Server.Talent;

namespace Server.Items
{
    [Serializable(0, false)]
    public partial class PolymeterDevice : BaseDevice
    {
        public override double DefaultWeight
        {
            get { return 0.1; }
        }

        public override int LabelNumber { get { return 1061183; } } // A glitchy device

        [Constructible]
        public PolymeterDevice() : base(WandEffect.Device, 10, 10)
        {
            Stackable = false;
            Light = LightType.Circle150;
            Hue = 1992;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("Polymorphs creatures, but may make them stronger accidentally.");
        }
        public override void OnWandUse(Mobile from)
        {
            if (Parent is PlayerMobile player)
            {
                BaseTalent talent = player.GetTalent(typeof(Polymeter));
                BaseTalent bugFixer = player.GetTalent(typeof(BugFixer));
                if (talent != null)
                {
                    Cast(new HealSpell(from, this));
                    int modifier = 0;
                    if (bugFixer != null)
                    {
                        modifier = bugFixer.Level;
                    }
                    if (Utility.Random(100) <= 6-modifier)
                    {
                        // glitch
                        if (from is BaseCreature creature)
                        {
                            if (!creature.IsHeroic)
                            {
                                creature.IsHeroic = true;
                            } else if (!creature.IsParagon)
                            {
                                creature.IsParagon = true;
                            }
                        }
                    } else
                    {
                        Point3D location = from.Location;
                        Map map = from.Map;
                        if (from.Backpack != null)
                        {
                            Gold gold = (Gold)from.Backpack.FindItemByType(typeof(Gold), true);
                            if (gold != null)
                            {
                                gold.MoveToWorld(location);
                            }
                        }
                        Rabbit rabbit = new Rabbit();
                        from.Delete();
                        rabbit.MoveToWorld(location, map);
                    }
                } else
                {
                    from.SendMessage("You cannot use this device.");
                }
            } 
        }
    }
}

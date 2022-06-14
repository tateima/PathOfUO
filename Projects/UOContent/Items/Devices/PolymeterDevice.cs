using Server.Mobiles;
using Server.Talent;
using Server.Targeting;
using ModernUO.Serialization;
namespace Server.Items
{
    [SerializationGenerator(0, false)]
    public partial class PolymeterDevice : BaseDevice
    {
        public override double DefaultWeight => 0.1;

        public override int LabelNumber => 1061183; // A glitchy device

        [Constructible]
        public PolymeterDevice() : base(WandEffect.Device, 10, 10)
        {
            Stackable = false;
            Light = LightType.Circle150;
            Name = "Polymeter Device";
            Hue = 1992;
        }

        public override void GetProperties(IPropertyList list)
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
                    from.SendMessage("What creature do you wish to use this device on?");
                    from.Target = new InternalTarget(bugFixer);
                }
                else
                {
                    from.SendMessage("You cannot use this device.");
                }
            }
        }

        private class InternalTarget : Target
        {
            private readonly BaseTalent m_BugFixer;

            public InternalTarget(BaseTalent bugFixer) : base(
                2,
                false,
                TargetFlags.None
            ) =>
                m_BugFixer = bugFixer;

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseCreature creature)
                {
                    int modifier = 0;
                    if (m_BugFixer != null)
                    {
                        modifier = m_BugFixer.Level;
                    }

                    if (Utility.Random(100) <= 6 - modifier)
                    {
                        // glitch
                        if (!creature.IsHeroic)
                        {
                            creature.IsHeroic = true;
                        }
                        else if (!creature.IsParagon)
                        {
                            creature.IsParagon = true;
                        }
                    }
                    else
                    {
                        Point3D location = creature.Location;
                        Map map = creature.Map;
                        Gold gold = (Gold)creature.Backpack?.FindItemByType(typeof(Gold));
                        gold?.MoveToWorld(location);
                        Rabbit rabbit = new Rabbit();
                        Effects.SendLocationParticles(
                            EffectItem.Create(creature.Location, creature.Map, EffectItem.DefaultDuration),
                            0x3728,
                            8,
                            20,
                            5042
                        );
                        Effects.PlaySound(creature, 0x201);
                        creature.Delete();
                        rabbit.MoveToWorld(location, map);
                    }
                }
            }
        }
    }
}

namespace Server.Mobiles
{
    public class ManaDrake : Drake
    {
        [Constructible]
        public ManaDrake()
        {
            AI = AIType.AI_Mage;
            SetInt(200, 300);
            Tamable = true;
            ControlSlots = 3;
            SetSkill(SkillName.Magery, 65.1, 80.0);
            SetSkill(SkillName.EvalInt, 65.1, 80.0);
            Hue = Utility.RandomBlueHue();
        }

        public ManaDrake(Serial serial) : base(serial)
        {
        }
        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder };
        public override string CorpseName => "a mana drake corpse";
        public override string DefaultName => "a mana drake";

        public override void OnGaveMeleeAttack(Mobile defender, int damage)
        {
            if (Controlled && ControlMaster is not null && Alive)
            {
                if (ControlMaster.Mana < ControlMaster.ManaMax)
                {
                    ControlMaster.Mana += AOS.Scale(damage, 2);
                }
            }
            if (Mana < ManaMax)
            {
                Mana += AOS.Scale(damage, 2);
            }
            base.OnGaveMeleeAttack(defender, damage);
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);
            var version = reader.ReadInt();
        }
    }
}

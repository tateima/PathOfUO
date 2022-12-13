using Server.Items;

namespace Server.Mobiles
{
    public class NecroticWyvern : Wyvern
    {
        [Constructible]
        public NecroticWyvern()
        {
            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 84.3;
            Hue = Utility.RandomRedHue();
        }

        public NecroticWyvern(Serial serial) : base(serial)
        {
        }
        public override OppositionGroup[] OppositionGroups => new[] { OppositionGroup.ChaosAndOrder };
        public override string CorpseName => "a necrotic wyvern corpse";
        public override string DefaultName => "a necrotic wyvern";

        public override void OnGaveMeleeAttack(Mobile defender, int damage)
        {
            if (Controlled && ControlMaster is not null && Alive)
            {
                if (ControlMaster.Hits < ControlMaster.HitsMax)
                {
                    ControlMaster.Hits += AOS.Scale(damage, 2);
                }
            }
            if (Hits < HitsMax)
            {
                Hits += AOS.Scale(damage, 2);
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

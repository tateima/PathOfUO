using Server.Mobiles;
using System;
namespace Server.Items
{
    [Flippable(0xc77, 0xc78)]
    public class Carrot : Food
    {
        [Constructible]
        public Carrot(int amount = 1) : base(0xc78, amount)
        {
            Weight = 1.0;
            FillFactor = 1;
        }

        public Carrot(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }
    [Flippable(0xc77, 0xc78)]
    public class Chilli : Food
    {
        private Mobile m_Mobile;
        private ResistanceMod m_Mod;

        [Constructible]
        public Chilli(int amount = 1) : base(0xc78, amount)
        {
            Hue = 0x8A;
            Weight = 1.0;
            FillFactor = 1;
        }

        public Chilli(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
        public override bool Eat(Mobile from) {
            from.SendMessage("You feel a slight increase in fire resistance");
            m_Mobile = from;
            m_Mod = new ResistanceMod(ResistanceType.Fire, +1);
            m_Mobile.AddResistanceMod(m_Mod);
            Timer.StartTimer(TimeSpan.FromMinutes(10), ExpireBuff);
            return base.Eat(from);
        }
        public void ExpireBuff() {
            if (m_Mobile != null && m_Mod != null) {
                m_Mobile.RemoveResistanceMod(m_Mod);
            }
        }
        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(
                1060847,
                "Chilli",
                ""
            );
        }
    }

    [Flippable(0xc7b, 0xc7c)]
    public class Cabbage : Food
    {
        [Constructible]
        public Cabbage(int amount = 1) : base(0xc7b, amount)
        {
            Weight = 1.0;
            FillFactor = 1;
        }

        public Cabbage(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    [Flippable(0xc7b, 0xc7c)]
    public class FrozenCabbage : Food
    {
        private Mobile m_Mobile;
        private ResistanceMod m_Mod;

        [Constructible]
        public FrozenCabbage(int amount = 1) : base(0xc78, amount)
        {
            Hue = 0xBC;
            Weight = 1.0;
            FillFactor = 1;
        }

        public FrozenCabbage(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
        public override bool Eat(Mobile from) {
            from.SendMessage("You feel a slight increase in cold resistance");
            m_Mobile = from;
            m_Mod = new ResistanceMod(ResistanceType.Cold, +5);
            m_Mobile.AddResistanceMod(m_Mod);
            Timer.StartTimer(TimeSpan.FromMinutes(10), ExpireBuff);
            return base.Eat(from);
        }
        public void ExpireBuff() {
            if (m_Mobile != null && m_Mod != null) {
                m_Mobile.RemoveResistanceMod(m_Mod);
            }
        }
        public override void GetProperties(ObjectPropertyList list)
        {
            list.Add(
                1060847,
                "{0} {1}",
                "Frozen",
                "cabbage"
            );
        }
    }

    [Flippable(0xc6d, 0xc6e)]
    public class Onion : Food
    {
        [Constructible]
        public Onion(int amount = 1) : base(0xc6d, amount)
        {
            Weight = 1.0;
            FillFactor = 1;
        }

        public Onion(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    [Flippable(0xc70, 0xc71)]
    public class Lettuce : Food
    {
        [Constructible]
        public Lettuce(int amount = 1) : base(0xc70, amount)
        {
            Weight = 1.0;
            FillFactor = 1;
        }

        public Lettuce(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }

    [Flippable(0xC6A, 0xC6B)]
    public class Pumpkin : Food
    {
        [Constructible]
        public Pumpkin(int amount = 1) : base(0xC6A, amount)
        {
            Weight = 1.0;
            FillFactor = 8;
        }

        public Pumpkin(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(1); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();

            if (version < 1)
            {
                if (FillFactor == 4)
                {
                    FillFactor = 8;
                }

                if (Weight == 5.0)
                {
                    Weight = 1.0;
                }
            }
        }
    }

    public class SmallPumpkin : Food
    {
        [Constructible]
        public SmallPumpkin(int amount = 1) : base(0xC6C, amount)
        {
            Weight = 1.0;
            FillFactor = 8;
        }

        public SmallPumpkin(Serial serial) : base(serial)
        {
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }
}

using System;

namespace Server.Items
{
    public class Blood : Item
    {
        [Constructible]
        public Blood() : this(Utility.RandomList(0x1645, 0x122A, 0x122B, 0x122C, 0x122D, 0x122E, 0x122F))
        {
        }

        [Constructible]
        public Blood(int itemID) : base(itemID)
        {
            Movable = false;

            new InternalTimer(this).Start();
        }

        public Blood(Serial serial) : base(serial)
        {
            new InternalTimer(this).Start();
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

        private class InternalTimer : Timer
        {
            private readonly Item m_Blood;

            public InternalTimer(Item blood) : base(TimeSpan.FromSeconds(5.0))
            {

                m_Blood = blood;
            }

            protected override void OnTick()
            {
                m_Blood.Delete();
            }
        }
    }
}

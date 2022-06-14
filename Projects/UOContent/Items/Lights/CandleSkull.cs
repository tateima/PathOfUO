using System;

namespace Server.Items
{
    public class CandleSkull : BaseLight
    {
        [Constructible]
        public CandleSkull() : base(0x1853)
        {
            if (Burnout)
            {
                Duration = TimeSpan.FromMinutes(25);
            }
            else
            {
                Duration = TimeSpan.Zero;
            }

            Burning = false;
            Light = LightType.Circle150;
            Weight = 5.0;
        }

        public CandleSkull(Serial serial) : base(serial)
        {
        }

        public override int LitItemID
        {
            get
            {
                if (ItemID is 0x1583 or 0x1854)
                {
                    return 0x1854;
                }

                return 0x1858;
            }
        }

        public override int UnlitItemID
        {
            get
            {
                if (ItemID is 0x1853 or 0x1584)
                {
                    return 0x1853;
                }

                return 0x1857;
            }
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

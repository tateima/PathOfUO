﻿namespace Server.Items
{
    [Serializable(0, false)]
    public partial class TwilightLantern : Lantern
    {
        [Constructible]
        public TwilightLantern() => Hue = Utility.RandomBool() ? 244 : 997;

        public override string DefaultName => "Twilight Lantern";

        public override bool AllowEquippedCast(Mobile from) => true;

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add(1060482); // Spell Channeling
        }
    }
}

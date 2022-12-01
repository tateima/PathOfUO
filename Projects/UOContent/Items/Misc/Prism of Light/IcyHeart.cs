using ModernUO.Serialization;
using Server.Mobiles;

namespace Server.Items;

[SerializationGenerator(0, false)]
public partial class IcyHeart : Item
{
    [Constructible]
    public IcyHeart() : base(0x24B) => Hue = MonsterBuff.FrozenHue;

    public override int LabelNumber => 1073162; // Icy Heart
}

using ModernUO.Serialization;
namespace Server.Items
{
    [Flippable(0x13B9, 0x13Ba)]
    [SerializationGenerator(0, false)]
    public partial class Valkyrie : VikingSword
    {
        [Constructible]
        public Valkyrie()
        {
            Hue = 0x58D;
            Frozen = true;
            Slayer = SlayerName.Silver;
            WeaponAttributes.ResistColdBonus = 50;
            WeaponAttributes.HitColdArea = 80;
            Attributes.WeaponSpeed = 50;
            Attributes.BonusStr = 15;
            Attributes.RegenHits = 5;
            Attributes.WeaponDamage = 60;
        }

        public override int LabelNumber => 1061064; // Valkyrie
        public override int ArtifactRarity => 12;

        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;
    }
}

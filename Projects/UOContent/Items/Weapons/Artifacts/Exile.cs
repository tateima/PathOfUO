using ModernUO.Serialization;
namespace Server.Items
{
    [Flippable(0xF5C, 0xF5D)]
    [SerializationGenerator(0, false)]
    public partial class Exile : Mace
    {
        [Constructible]
        public Exile()
        {
            Weight = 14.0;
            Hue = 0x210;
            Slayer = SlayerName.DragonSlaying;
            Burning = true;
            WeaponAttributes.HitFireArea = 80;
            Attributes.AttackChance = 25;
            Attributes.BonusDex = 15;
            Attributes.ReflectPhysical = 25;
            Attributes.WeaponDamage = 60;
            WeaponAttributes.ResistPhysicalBonus = 50;
        }

        public override WeaponAbility PrimaryAbility => WeaponAbility.ConcussionBlow;
        public override WeaponAbility SecondaryAbility => WeaponAbility.Disarm;
        public override int LabelNumber => 1061062; // Exile
        public override int ArtifactRarity => 12;

        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;
    }
}

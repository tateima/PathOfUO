using ModernUO.Serialization;

namespace Server.Items
{
    [SerializationGenerator(0, false)]
    public partial class DeathEater : Halberd
    {
        [Constructible]
        public DeathEater()
        {
            Hue = 0x327;
            Slayer = SlayerName.Repond;
            Haunted = true;
            WeaponAttributes.HitLeechHits = 100;
            WeaponAttributes.HitLeechMana = 100;
            Attributes.AttackChance = 20;
            Attributes.DefendChance = 25;
            Attributes.WeaponDamage = 60;
            WeaponAttributes.ResistEnergyBonus = 50;
        }

        public override int LabelNumber => 1061065; // The Death Eater
        public override int ArtifactRarity => 12;

        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;
    }
}

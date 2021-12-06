namespace Server.Items
{
    [Serializable(0, false)]
    public partial  class TwigOfCreation : BlackStaff
    {
        [Constructible]
        public TwigOfCreation()
        {
            Hue = 0x9B;
            Slayer = SlayerName.BalronDamnation;
            WeaponAttributes.MageWeapon = 30;
            WeaponAttributes.HitLightning = 80;
            WeaponAttributes.ResistFireBonus = 50;
            Attributes.SpellChanneling = 1;
            Attributes.CastSpeed = 2;
            Attributes.SpellDamage = 15;
            Attributes.WeaponDamage = 60;
        }

        public override int LabelNumber => 1061063; // Twig of Creation
        public override int ArtifactRarity => 11;

        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;

    }
}

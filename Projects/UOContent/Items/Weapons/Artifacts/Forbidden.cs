namespace Server.Items
{
    [Serializable(0, false)]
    public partial class Forbidden : Bow
    {
        [Constructible]
        public Forbidden()
        {
            Hue = 0x1FC;
            Haunted = true;
            Slayer = SlayerName.ElementalBan;
            WeaponAttributes.HitHarm = 80;
            WeaponAttributes.HitLeechStam = 100;
            Attributes.RegenStam = 15;
            Attributes.WeaponDamage = 60;
            Attributes.WeaponSpeed = 30;
            WeaponAttributes.ResistPoisonBonus = 50;
        }

        public override int LabelNumber => 1061060; // Forbidden
        public override int ArtifactRarity => 12;

        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;

        public override void GetDamageTypes(
            Mobile wielder, out int phys, out int fire, out int cold, out int pois,
            out int nrgy, out int chaos, out int direct
        )
        {
            phys = fire = pois = nrgy = cold = direct = 0;
            chaos = 100;
        }
    }
}

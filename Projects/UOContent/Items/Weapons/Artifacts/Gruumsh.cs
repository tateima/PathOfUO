namespace Server.Items
{
    [Flippable(0xF62, 0xF63)]
    [Serializable(0, false)]
    public partial class Gruumsh : Spear
    {
        [Constructible]
        public Gruumsh()
        {
            Weight = 7.0;
            Slayer = SlayerName.Repond;
            Toxic = true;
            WeaponAttributes.HitPoisonArea = 80;
            Attributes.AttackChance = 25;
            Attributes.BonusHits = 15;
            WeaponAttributes.HitLowerAttack = 40;
            WeaponAttributes.HitLowerDefend = 50;
            Attributes.WeaponDamage = 60;
        }

        public override WeaponAbility PrimaryAbility => WeaponAbility.ArmorIgnore;
        public override WeaponAbility SecondaryAbility => WeaponAbility.ParalyzingBlow;
        public override int LabelNumber => 1061061; // Gruumsh
        public override int ArtifactRarity => 12;

        public override int InitMinHits => 255;
        public override int InitMaxHits => 255;
    }
}

namespace Server.Talent
{
    public class ElementalHunter : BaseTalent
    {
        public ElementalHunter()
        {
            BlockedBy = new[] { typeof(HumanoidHunter) };
            TalentDependency = typeof(ExperiencedHunter);
            HasDamageAbsorptionEffect = true;
            DisplayName = "Elemental hunter";
            Description = "Increases damage to elemental and heals damage from them.";
            ImageID = 175;
            AddEndY = 85;
        }

        public override int CheckDamageAbsorptionEffect(Mobile defender, Mobile attacker, int damage)
        {
            if (IsMobileType(OppositionGroup.ElementalGroup, attacker.GetType()))
            {
                damage -= AOS.Scale(damage, Level * 5);
            }
            return damage;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (IsMobileType(OppositionGroup.ElementalGroup, target.GetType()))
            {
                target.Damage(Utility.RandomMinMax(1, Level), attacker);
            }
        }
    }
}

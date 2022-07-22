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
            AdditionalDetail = $"The damage caused is 1-X where X is the talent level. The damage absorbed from their attacks is 5% per level. {PassiveDetail}";
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

        public override void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
            if (IsMobileType(OppositionGroup.ElementalGroup, target.GetType()))
            {
                damage += Utility.RandomMinMax(1, Level);
            }
        }
    }
}

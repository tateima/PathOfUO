namespace Server.Talent
{
    public class ElementalHunter : BaseTalent
    {
        public ElementalHunter()
        {
            BlockedBy = new[] { typeof(HumanoidHunter) };
            TalentDependency = typeof(ExperiencedHunter);
            DisplayName = "Elemental hunter";
            Description = "Increases damage to elemental and heals damage from them.";
            ImageID = 175;
            AddEndY = 85;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (IsMobileType(OppositionGroup.ElementalGroup, target.GetType()))
            {
                target.Damage(Level, attacker);
            }
        }

        public override void CheckDefenseEffect(Mobile defender, Mobile target, int damage)
        {
            if (IsMobileType(OppositionGroup.ElementalGroup, target.GetType()))
            {
                defender.Heal(AOS.Scale(damage, Level));
            }
        }
    }
}

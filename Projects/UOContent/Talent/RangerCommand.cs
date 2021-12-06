namespace Server.Talent
{
    public class RangerCommand : BaseTalent
    {
        public RangerCommand()
        {
            TalentDependency = typeof(NatureAffinity);
            DisplayName = "Ranger command";
            Description = "Decreases stat and skill loss of tamed creatures by 1% per level.";
            ImageID = 188;
            AddEndY = 95;
        }

        public override double ModifySpellScalar() => Level / 100.0;
    }
}

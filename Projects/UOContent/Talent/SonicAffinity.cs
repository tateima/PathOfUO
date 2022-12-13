namespace Server.Talent
{
    public class SonicAffinity : BaseTalent
    {
        public SonicAffinity()
        {
            DisplayName = "Sonic affinity";
            Description = "Increases effectiveness of provocation, peacemaking and discordance.";
            ImageID = 163;
            GumpHeight = 85;
            AddEndY = 70;
            MaxLevel = 5;
        }

        public override int ModifySpellMultiplier() => Level * 2; // 2 per point

        public override double ModifySpellScalar() => Level / 100.0; // 1% per point
    }
}

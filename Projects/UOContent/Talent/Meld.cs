namespace Server.Talent
{
    public class Meld : BaseTalent
    {
        public Meld()
        {
            TalentDependencies = new[] { typeof(Enchant) };
            DisplayName = "Meld";
            Description =
                "Can meld elemental shards into items to increase their power.";
            AdditionalDetail = "Requires at least one magic and one crafting skill above 70+";
            ImageID = 398;
            MaxLevel = 1;
            GumpHeight = 85;
            AddEndY = 90;
        }

        public override bool HasSkillRequirement(Mobile mobile) => Disenchant.CanDisenchant(mobile, 90);
    }
}

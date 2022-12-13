namespace Server.Talent
{
    public class ExoticTamer : BaseTalent
    {
        public ExoticTamer()
        {
            TalentDependencies = new[] { typeof(BondingMaster) };
            DisplayName = "Exotic tamer";
            Description = "Allows you to tame exotic and unique creatures.";
            MaxLevel = 1;
            AdditionalDetail = "These creatures include: Mana Drakes, Necrotic Wyverns, Prismatic Drakes, Silver Serpents and Diamond Serpents";
            ImageID = 431;
            AddEndAdditionalDetailsY = 70;
        }
    }
}

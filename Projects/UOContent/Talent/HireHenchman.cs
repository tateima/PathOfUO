using Server.Items;
using Server.Mobiles;

namespace Server.Talent
{
    public class HireHenchman : BaseTalent
    {
        public HireHenchman()
        {
            BlockedBy = new[] { typeof(Merchant) };
            TalentDependencies = new[] { typeof(SmoothTalker) };
            DisplayName = "Henchman";
            MaxLevel = 3;
            CanBeUsed = true;
            Description =
                "Hire a henchman to protect you. At least 5000 gold is required in your bank during hire. Power increases with gold spent.";
            AdditionalDetail = $"A maximum of 100,000 gold is accepted, which would provide a maximum power scale of 20%. The number of henchmen you can have increases by 1 per level. {PassiveDetail}";
            ImageID = 365;
            GumpHeight = 230;
            AddEndY = 115;
        }

        public override void OnUse(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                TavernKeeper keeper = null;
                foreach (var mobile in from.GetMobilesInRange(3))
                {
                    if (mobile.GetType() == typeof(TavernKeeper))
                    {
                        keeper = (TavernKeeper)mobile;
                    }
                }
                if (keeper != null)
                {
                    if (player.Henchmen.Count + player.RestedHenchmen.Count < 2)
                    {
                        Container bank = from.FindBankNoCreate();
                        int totalPlayerGold = Banker.GetBalance(from);
                        if (totalPlayerGold >= 5000)
                        {
                            int goldToUse;
                            if (totalPlayerGold >= 100000)
                            {
                                goldToUse = 100000;
                            }
                            else
                            {
                                goldToUse = totalPlayerGold % 5000 >= 500
                                    ? totalPlayerGold + 5000 - totalPlayerGold % 5000
                                    : totalPlayerGold - totalPlayerGold % 5000;
                            }

                            Banker.Withdraw(from, goldToUse);
                            MobilePercentagePerPoint = goldToUse / 5000; // max of 20% per henchmen
                            var hireling = new Henchman();
                            hireling = (Henchman)ScaleMobileStats(hireling);
                            hireling = (Henchman)ScaleMobileSkills(hireling, "Magical");
                            hireling = (Henchman)ScaleMobileSkills(hireling, "Combat Ratings");
                            hireling = (Henchman)ScaleMobileSkills(hireling, "Lore & Knowledge");
                            hireling.Hits = hireling.HitsMax;
                            from.RevealingAction();
                            hireling.MoveToWorld(from.Location, from.Map);
                            hireling.Owners.Add(from);
                            hireling.SetControlMaster(from);
                            player.Leadership();
                        }
                        else
                        {
                            keeper.Say("Thou cannot afford to hire a henchman. You need at least 6000 gold.");
                        }
                    }
                    else
                    {
                        keeper.Say("Thou already have too many henchmen.");
                    }
                }
                else
                {
                    from.SendMessage("Thou must seek out a tavern keeper first before using this talent.");
                }
            }
        }
    }
}

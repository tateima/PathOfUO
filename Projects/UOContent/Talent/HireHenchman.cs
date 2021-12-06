using System.Linq;
using Server.Items;
using Server.Mobiles;

namespace Server.Talent
{
    public class HireHenchman : BaseTalent
    {
        public HireHenchman()
        {
            BlockedBy = new[] { typeof(Merchant) };
            TalentDependency = typeof(SmoothTalker);
            DisplayName = "Henchman";
            MobilePercentagePerPoint = 3;
            CanBeUsed = true;
            Description =
                "Hire a henchman to protect you. At least 2500 gold is required in your bank during hire. Stats scale 1-250%, skills 1-100%.";
            ImageID = 365;
            GumpHeight = 230;
            AddEndY = 125;
        }

        public override void OnUse(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                TavernKeeper keeper = from.GetMobilesInRange(3).OfType<TavernKeeper>().FirstOrDefault();
                if (keeper != null)
                {
                    if (player.Henchmen.Count + player.RestedHenchmen.Count < 2)
                    {
                        Container bank = from.FindBankNoCreate();
                        if (Banker.GetBalance(from) >= 2500)
                        {
                            int goldToUse;
                            var gold = (Gold)bank.FindItemByType(typeof(Gold));
                            var totalGold = gold.Amount;
                            if (totalGold > 50000)
                            {
                                goldToUse = 50000;
                            }
                            else
                            {
                                goldToUse = totalGold % 1000 >= 500
                                    ? totalGold + 1000 - totalGold % 1000
                                    : totalGold - totalGold % 1000;
                            }

                            Banker.Withdraw(from, goldToUse);
                            MobilePercentagePerPoint = goldToUse / 1000;
                            var hireling = new Henchman();
                            hireling = (Henchman)ScaleMobileStats(hireling);
                            // needs to be 20 max now to make 100% skill bonus
                            MobilePercentagePerPoint = goldToUse / 2500;
                            hireling = (Henchman)ScaleMobileSkills(hireling, "Magical");
                            hireling = (Henchman)ScaleMobileSkills(hireling, "Combat Ratings");
                            hireling = (Henchman)ScaleMobileSkills(hireling, "Lore & Knowledge");
                            from.RevealingAction();
                            hireling.MoveToWorld(from.Location, from.Map);
                            hireling.Owners.Add(from);
                            hireling.ControlMaster = from;
                            hireling.ControlTarget = from;
                        }
                        else
                        {
                            keeper.Say("Thou cannot afford to hire a henchman. You need at least 2500 gold.");
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

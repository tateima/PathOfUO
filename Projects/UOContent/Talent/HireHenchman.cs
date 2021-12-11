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
                "Hire a henchman to protect you. At least 6000 gold is required in your bank during hire. Stats and skills scale 12% per level.";
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
                        int totalPlayerGold = Banker.GetBalance(from);
                        if (totalPlayerGold >= 6000)
                        {
                            int goldToUse;
                            if (totalPlayerGold >= 75000)
                            {
                                goldToUse = 75000;
                            }
                            else
                            {
                                goldToUse = totalPlayerGold % 6000 >= 500
                                    ? totalPlayerGold + 6000 - totalPlayerGold % 6000
                                    : totalPlayerGold - totalPlayerGold % 6000;
                            }

                            Banker.Withdraw(from, goldToUse);
                            MobilePercentagePerPoint = goldToUse / 6000; // max of 12% per henchmen
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

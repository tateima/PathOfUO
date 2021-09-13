using Server.Mobiles;
using Server.Items;
using Server.Network;
using System;

namespace Server.Talent
{
    public class HireHenchman : BaseTalent, ITalent
    {
        public HireHenchman() : base()
        {
            BlockedBy = new Type[] { typeof(Merchant) };
            TalentDependency = typeof(SmoothTalker);
            DisplayName = "Henchman";
            MobilePercentagePerPoint = 3;
            CanBeUsed = true;
            Description = "Hire a henchman to protect you. Atleast 2500 gold is required in your bank during hire. Stats scale 1-250%, skills 1-100%.";
            ImageID = 365;
            GumpHeight = 230;
            AddEndY = 125;
        }
        public override void OnUse(Mobile boss)
        {
            if (boss is PlayerMobile player)
            {
                TavernKeeper keeper = null;
                foreach (Mobile mobile in boss.GetMobilesInRange(3))
                {
                    if (mobile is TavernKeeper)
                    {
                        keeper = (TavernKeeper)mobile;
                        break;
                    }
                }
                if (keeper != null)
                {
                    if ((player.Henchmen.Count + player.RestedHenchmen.Count) < 2)
                    {
                        Container bank = boss.FindBankNoCreate();
                        if (Banker.GetBalance(boss) >= 2500)
                        {
                            int goldToUse = 0;
                            Gold gold = (Gold)bank.FindItemByType(typeof(Gold), true);
                            int totalGold = gold.Amount;
                            if (totalGold > 50000)
                            {
                                goldToUse = 50000;
                            }
                            else
                            {
                                goldToUse = totalGold % 1000 >= 500 ? totalGold + 1000 - totalGold % 1000 : totalGold - totalGold % 1000;
                            }
                            Banker.Withdraw(boss, goldToUse);
                            MobilePercentagePerPoint = (goldToUse / 1000);
                            Henchman hireling = new Henchman();
                            hireling = (Henchman)ScaleMobileStats(hireling);
                            // needs to be 20 max now to make 100% skill bonus
                            MobilePercentagePerPoint = (goldToUse / 2500);
                            hireling = (Henchman)ScaleMobileSkills(hireling, "Magical");
                            hireling = (Henchman)ScaleMobileSkills(hireling, "Combat Ratings");
                            hireling = (Henchman)ScaleMobileSkills(hireling, "Lore & Knowledge");
                            boss.RevealingAction();
                            hireling.MoveToWorld(boss.Location, boss.Map);
                            hireling.Owners.Add(boss);
                            hireling.ControlMaster = boss;
                            hireling.ControlTarget = boss;
                        }
                        else
                        {
                            keeper.Say("Thou cannot afford to hire a henchman. You need at least 2500 gold.");
                        }
                    } else
                    {
                        keeper.Say("Thou already have too many henchmen.");
                    }
                }
                else
                {
                    keeper.Say("Thou must seek out a tavern keeper first before using this talent.");
                }                
            }            
        }
    }
}

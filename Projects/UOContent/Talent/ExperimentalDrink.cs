using System;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Talent
{
    public class ExperimentalDrink : BaseTalent, ITalent
    {
        public ExperimentalDrink() : base()
        {
            DisplayName = "Experimental drink";
            Description = "Unlocks extra drink types for discovery. Experiment using different materials. Requires 80+ cooking.";
            ImageID = 404;
            CanBeUsed = true;
            GumpHeight = 85;
            AddEndY = 105;
        }
        public override bool HasSkillRequirement(Mobile mobile) {
            return mobile.Skills.Cooking.Base >= 80;
        }

        public override void OnUse(Mobile mobile)
        {
            mobile.SendMessage("What beverage do you wish to experiment with?");
            mobile.Target = new InternalTarget(mobile, this);
        }

        private class InternalTarget : Target
        {
            private Mobile m_Cook;
            private ExperimentalDrink m_Talent;
            public InternalTarget(Mobile cook, ExperimentalDrink talent) : base(
                8,
                false,
                TargetFlags.None
            )
            {
                m_Cook = cook;
                m_Talent = talent;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                
                if (targeted is BaseBeverage beverage)
                {
                    m_Cook.Target = new InternalSecondTarget(beverage, m_Cook, m_Talent);
                }
                else
                {
                    m_Cook.SendMessage("You cannot experiment with that target.");
                }
            }
        }

        private class InternalSecondTarget : Target
        {
            private Mobile m_Cook;
            private ExperimentalDrink m_Talent;
            private BaseBeverage m_Beverage;
            public InternalSecondTarget(BaseBeverage beverage, Mobile cook, ExperimentalDrink talent) : base(
                8,
                false,
                TargetFlags.None
            )
            {
                m_Beverage = beverage;
                m_Cook = cook;
                m_Talent = talent;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                
                if (targeted is Item item)
                {
                    bool success = false;
                    bool partialSuccess = false;
                    int itemConsume = 0;
                    int beverageConsume = 0;
                    if (Utility.Random(100) < m_Talent.Level * 10) {
                        if (from.Backpack != null) {
                            itemConsume = 1;
                            beverageConsume = 1;
                            if (item is BlackPearl && m_Beverage is BeverageBottle) {
                                success = true;
                                BlackSambucca drink = new BlackSambucca(BeverageType.Liquor);
                                from.Backpack.AddItem(drink);
                            } else if (item is Bone && m_Beverage.Content == BeverageType.Water) {
                                success = true;
                                BoneBroth drink = new BoneBroth(BeverageType.Water);
                                from.Backpack.AddItem(drink);
                            } else if (item is EnchanterDust && m_Beverage.Content == BeverageType.Milk) {
                                success = true;
                                EnchantedMilk drink = new EnchantedMilk(BeverageType.Milk);
                                from.Backpack.AddItem(drink);
                            } else if (item is SulfurousAsh && m_Beverage.Content == BeverageType.Water ) {
                                success = true;
                                MageWater drink = new MageWater(BeverageType.Water);
                                from.Backpack.AddItem(drink);
                            } else if (item is NoxCrystal && m_Beverage.Content == BeverageType.Ale) {
                                success = true;
                                Guinness drink = new Guinness(BeverageType.Ale);
                                from.Backpack.AddItem(drink);
                            } else if (item is Ruby && m_Beverage is Pitcher) {
                                success = true;
                                FireSpirits drink = new FireSpirits(BeverageType.Liquor);
                                from.Backpack.AddItem(drink);
                            } else if (item is Sapphire && m_Beverage is Jug) {
                                success = true;
                                ChargedSpirits drink = new ChargedSpirits(BeverageType.Liquor);
                                from.Backpack.AddItem(drink);
                            } else if (item is OrcishKinMask && m_Beverage.Content == BeverageType.Water) {
                                success = true;
                                OrcishWater drink = new OrcishWater(BeverageType.Water);
                                from.Backpack.AddItem(drink);                          
                            } else if (item is DaemonBone && m_Beverage.Content == BeverageType.Water) { 
                                success = true;
                                DemonicWater drink = new DemonicWater(BeverageType.Water);
                                from.Backpack.AddItem(drink);
                            } else if (item is BoneHelm && m_Beverage.Content == BeverageType.Water) {
                                success = true;
                                SkeletalWater drink = new SkeletalWater(BeverageType.Water);
                                from.Backpack.AddItem(drink);
                            } else if (item is Garlic && m_Beverage.Content == BeverageType.Water) {
                                success = true;
                                HolyWater drink = new HolyWater(BeverageType.Water);
                                from.Backpack.AddItem(drink);
                            } else if (item is Fish && m_Beverage is BeverageBottle) {
                                success = true;
                                PiratesBrew drink = new PiratesBrew(BeverageType.Ale);
                                from.Backpack.AddItem(drink);
                            } else if (item is OilCloth && m_Beverage is Jug) {
                                success = true;
                                SoldiersBrew drink = new SoldiersBrew(BeverageType.Ale);
                                from.Backpack.AddItem(drink);
                            } else if (item is Shuriken && m_Beverage is Pitcher) {
                                success = true;
                                NinjasBrew drink = new NinjasBrew(BeverageType.Ale);
                                from.Backpack.AddItem(drink);
                            } else if (item is GnarledStaff && m_Beverage is Jug) {
                                success = true;
                                HealersBrew drink = new HealersBrew(BeverageType.Ale);
                                from.Backpack.AddItem(drink);
                            } else if (item is EnchantedSausage && m_Beverage is BeverageBottle) {
                                success = true;
                                AncestralBrew drink = new AncestralBrew(BeverageType.Ale);
                                from.Backpack.AddItem(drink);
                            } 
                        }
                    }
                    if (!success && !partialSuccess) {
                        m_Cook.SendMessage("Your experiment failed");
                    } else if (partialSuccess) {
                        from.SendMessage("Your experiment has potential, but failed");
                        itemConsume = Utility.Random(5);
                        beverageConsume = Utility.Random(5);
                    } else {
                        m_Cook.SendMessage("Your experiment has worked");
                    }
                    if (itemConsume > 0) {
                        item.Consume(itemConsume);
                    } else {
                        item.Delete();
                    }
                    if (beverageConsume > 0) {
                        m_Beverage.Consume(beverageConsume);
                    } else {
                        m_Beverage.Delete();
                    }                    
                }
                else
                {
                    m_Cook.SendMessage("You cannot experiment with that target.");
                }
            }
        }
    }        
}

using Server.Items;
using Server.Targeting;

namespace Server.Talent
{
    public class ExperimentalDrink : BaseTalent
    {
        public ExperimentalDrink()
        {
            DisplayName = "Experimental drink";
            Description =
                "Unlocks extra drink types for discovery. Experiment using different materials. Requires 80+ cooking.";
            ImageID = 404;
            CanBeUsed = true;
            GumpHeight = 85;
            AddEndY = 105;
        }

        public override bool HasSkillRequirement(Mobile mobile) => mobile.Skills.Cooking.Base >= 80;

        public override void OnUse(Mobile from)
        {
            from.SendMessage("What beverage do you wish to experiment with?");
            from.Target = new InternalTarget(from, this);
        }

        private class InternalTarget : Target
        {
            private readonly Mobile _cook;
            private readonly ExperimentalDrink _talent;

            public InternalTarget(Mobile cook, ExperimentalDrink talent) : base(
                8,
                false,
                TargetFlags.None
            )
            {
                _cook = cook;
                _talent = talent;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseBeverage beverage)
                {
                    from.SendMessage("What do you wish to experiment with on this drink?");
                    _cook.Target = new InternalSecondTarget(beverage, _cook, _talent);
                }
                else
                {
                    _cook.SendMessage("You cannot experiment with that target.");
                }
            }
        }

        private class InternalSecondTarget : Target
        {
            private readonly BaseBeverage _beverage;
            private readonly Mobile _cook;
            private readonly ExperimentalDrink _talent;

            public InternalSecondTarget(BaseBeverage beverage, Mobile cook, ExperimentalDrink talent) : base(
                8,
                false,
                TargetFlags.None
            )
            {
                _beverage = beverage;
                _cook = cook;
                _talent = talent;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item item)
                {
                    var success = false;
                    var itemConsume = 0;
                    var beverageConsume = 0;
                    if (Utility.Random(100) < _talent.Level * 7)
                    {
                        if (from.Backpack != null)
                        {
                            itemConsume = 1;
                            beverageConsume = 1;
                            if (item is BlackPearl && _beverage is BeverageBottle)
                            {
                                success = true;
                                var drink = new BlackSambucca(BeverageType.Liquor);
                                from.Backpack.AddItem(drink);
                            }
                            else if (item is Bone && _beverage.Content == BeverageType.Water)
                            {
                                success = true;
                                var drink = new BoneBroth(BeverageType.Water);
                                from.Backpack.AddItem(drink);
                            }
                            else if (item is EnchanterDust && _beverage.Content == BeverageType.Milk)
                            {
                                success = true;
                                var drink = new EnchantedMilk(BeverageType.Milk);
                                from.Backpack.AddItem(drink);
                            }
                            else if (item is SulfurousAsh && _beverage.Content == BeverageType.Water)
                            {
                                success = true;
                                var drink = new MageWater(BeverageType.Water);
                                from.Backpack.AddItem(drink);
                            }
                            else if (item is NoxCrystal && _beverage.Content == BeverageType.Ale)
                            {
                                success = true;
                                var drink = new Guinness(BeverageType.Ale);
                                from.Backpack.AddItem(drink);
                            }
                            else if (item is Ruby && _beverage is Pitcher)
                            {
                                success = true;
                                var drink = new FireSpirits(BeverageType.Liquor);
                                from.Backpack.AddItem(drink);
                            }
                            else if (item is Sapphire && _beverage is Jug)
                            {
                                success = true;
                                var drink = new ChargedSpirits(BeverageType.Liquor);
                                from.Backpack.AddItem(drink);
                            }
                            else if (item is OrcishKinMask && _beverage.Content == BeverageType.Water)
                            {
                                success = true;
                                var drink = new OrcishWater(BeverageType.Water);
                                from.Backpack.AddItem(drink);
                            }
                            else if (item is DaemonBone && _beverage.Content == BeverageType.Water)
                            {
                                success = true;
                                var drink = new DemonicWater(BeverageType.Water);
                                from.Backpack.AddItem(drink);
                            }
                            else if (item is BoneHelm && _beverage.Content == BeverageType.Water)
                            {
                                success = true;
                                var drink = new SkeletalWater(BeverageType.Water);
                                from.Backpack.AddItem(drink);
                            }
                            else if (item is Garlic && _beverage.Content == BeverageType.Water)
                            {
                                success = true;
                                var drink = new HolyWater(BeverageType.Water);
                                from.Backpack.AddItem(drink);
                            }
                            else if (item is Fish && _beverage is BeverageBottle)
                            {
                                success = true;
                                var drink = new PiratesBrew(BeverageType.Ale);
                                from.Backpack.AddItem(drink);
                            }
                            else if (item is OilCloth && _beverage is Jug)
                            {
                                success = true;
                                var drink = new SoldiersBrew(BeverageType.Ale);
                                from.Backpack.AddItem(drink);
                            }
                            else if (item is Shuriken && _beverage is Pitcher)
                            {
                                success = true;
                                var drink = new NinjasBrew(BeverageType.Ale);
                                from.Backpack.AddItem(drink);
                            }
                            else if (item is GnarledStaff && _beverage is Jug)
                            {
                                success = true;
                                var drink = new HealersBrew(BeverageType.Ale);
                                from.Backpack.AddItem(drink);
                            }
                            else if (item is EnchantedSausage && _beverage is BeverageBottle)
                            {
                                success = true;
                                var drink = new AncestralBrew(BeverageType.Ale);
                                from.Backpack.AddItem(drink);
                            }
                        }
                    }

                    _cook.SendMessage(!success ? "Your experiment failed" : "Your experiment has worked");

                    if (itemConsume > 0)
                    {
                        item.Consume(itemConsume);
                    }
                    else
                    {
                        item.Delete();
                    }

                    if (beverageConsume > 0)
                    {
                        _beverage.Consume(beverageConsume);
                    }
                    else
                    {
                        _beverage.Delete();
                    }
                }
                else
                {
                    _cook.SendMessage("You cannot experiment with that target.");
                }
            }
        }
    }
}

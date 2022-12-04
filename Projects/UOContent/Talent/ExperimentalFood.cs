using Server.Items;
using Server.Targeting;

namespace Server.Talent
{
    public class ExperimentalFood : BaseTalent
    {
        public ExperimentalFood()
        {
            DisplayName = "Experimental food";
            Description =
                "Unlocks extra food types for discovery. Experiment using different materials. Requires 80+ cooking.";
            ImageID = 403;
            CanBeUsed = true;
            GumpHeight = 85;
            AddEndY = 115;
        }

        public override bool HasSkillRequirement(Mobile mobile) => mobile.Skills.Cooking.Base >= 80;

        public override void OnUse(Mobile from)
        {
            if (HasSkillRequirement(from))
            {
                from.SendMessage("What food do you wish to experiment with?");
                from.Target = new InternalTarget(from, this);
            }
            else
            {
                from.SendMessage("You don't have the skills to experiment with food.");
            }
        }

        private class InternalTarget : Target
        {
            private readonly Mobile _cook;
            private readonly ExperimentalFood _talent;

            public InternalTarget(Mobile cook, ExperimentalFood talent) : base(
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
                if (targeted is Food food)
                {
                    from.SendMessage("What do you wish to experiment with on this food?");
                    _cook.Target = new InternalSecondTarget(food, _cook, _talent);
                }
                else
                {
                    _cook.SendMessage("You cannot experiment with that target.");
                }
            }
        }

        private class InternalSecondTarget : Target
        {
            private readonly Mobile _cook;
            private readonly Food _food;
            private readonly ExperimentalFood _talent;

            public InternalSecondTarget(Food food, Mobile cook, ExperimentalFood talent) : base(
                8,
                false,
                TargetFlags.None
            )
            {
                _food = food;
                _cook = cook;
                _talent = talent;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item item)
                {
                    var success = false;
                    var partialSuccess = false;
                    var itemConsume = 0;
                    var foodConsume = 0;
                    if (Utility.Random(100) < _talent.Level * 7)
                    {
                        if (from.Backpack != null)
                        {
                            itemConsume = 1;
                            foodConsume = 1;
                            if (item is Garlic && _food is BreadLoaf)
                            {
                                success = true;
                                var bread = new GarlicBread();
                                from.Backpack.AddItem(bread);
                            }
                            else if (item is MandrakeRoot && _food is Cake)
                            {
                                success = true;
                                var cake = new MandrakeCake();
                                from.Backpack.AddItem(cake);
                            }
                            else if (item is EnchanterDust && _food is Sausage)
                            {
                                success = true;
                                var sausage = new EnchantedSausage();
                                from.Backpack.AddItem(sausage);
                            }
                            else if (item is Gold && _food is Ham)
                            {
                                if (item.Amount >= 100)
                                {
                                    itemConsume = 100;
                                    success = true;
                                    var ham = new GoldenHam();
                                    from.Backpack.AddItem(ham);
                                }
                                else
                                {
                                    partialSuccess = true;
                                }
                            }
                            else if (item is FireHorn && _food is Carrot)
                            {
                                success = true;
                                var chilli = new Chilli();
                                from.Backpack.AddItem(chilli);
                            }
                            else if (item is IcyHeart && _food is Cabbage)
                            {
                                success = true;
                                var cabbage = new FrozenCabbage();
                                from.Backpack.AddItem(cabbage);
                            }
                            else if (item is IronIngot && _food is CheeseWheel)
                            {
                                if (item.Amount >= 10)
                                {
                                    itemConsume = 10;
                                    success = true;
                                    var cheese = new IronRichCheese();
                                    from.Backpack.AddItem(cheese);
                                }
                                else
                                {
                                    partialSuccess = true;
                                }
                            }
                            else if (item is CurePotion && _food is Muffins)
                            {
                                if (item.Amount >= 3)
                                {
                                    success = true;
                                    var sourDough = new SourDough();
                                    from.Backpack.AddItem(sourDough);
                                }
                                else
                                {
                                    partialSuccess = true;
                                }
                            }
                            else if (item is Kilt && _food is FriedEggs)
                            {
                                success = true;
                                var braveEggs = new BraveEggs();
                                from.Backpack.AddItem(braveEggs);
                            }
                            else if (item is BodySash && _food is RoastPig)
                            {
                                success = true;
                                var roastPig = new DecoratedRoastPig();
                                from.Backpack.AddItem(roastPig);
                            }
                            else if (item is BatWing && _food is Ribs)
                            {
                                success = true;
                                var ribs = new BatEncrustedRibs();
                                from.Backpack.AddItem(ribs);
                            }
                            else if (item is Lemon && _food is ApplePie)
                            {
                                success = true;
                                var pie = new LemonPie();
                                from.Backpack.AddItem(pie);
                            }
                            else if (item is DaemonBlood && _food is LambLeg)
                            {
                                success = true;
                                var lambLeg = new SacrificialLambLeg();
                                from.Backpack.AddItem(lambLeg);
                            }
                            else if (item is WizardsHat && _food is Quiche)
                            {
                                success = true;
                                var quiche = new PhilosophersQuiche();
                                from.Backpack.AddItem(quiche);
                            }
                            else if (item is Bandage && _food is MeatPie)
                            {
                                success = true;
                                var athletesPie = new AthletesPie();
                                from.Backpack.AddItem(athletesPie);
                            }
                            else if (item is SpidersSilk && _food is CookedBird)
                            {
                                success = true;
                                var chicken = new StickyChicken();
                                from.Backpack.AddItem(chicken);
                            }
                            else if (item is Ginseng && _food is FishSteak)
                            {
                                success = true;
                                var singingFillet = new SingingFillet();
                                from.Backpack.AddItem(singingFillet);
                            }
                        }
                    }

                    if (!success && !partialSuccess)
                    {
                        _cook.SendMessage("Your experiment failed");
                    }
                    else if (partialSuccess)
                    {
                        from.SendMessage("Your experiment has potential, but failed");
                        itemConsume = Utility.Random(5);
                        foodConsume = Utility.Random(5);
                    }
                    else
                    {
                        _cook.SendMessage("Your experiment has worked");
                    }

                    if (itemConsume > 0)
                    {
                        item.Consume(itemConsume);
                    }
                    else
                    {
                        item.Delete();
                    }

                    if (foodConsume > 0)
                    {
                        _food.Consume(foodConsume);
                    }
                    else
                    {
                        _food.Delete();
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

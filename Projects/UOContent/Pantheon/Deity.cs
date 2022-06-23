using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Server.Engines.BulkOrders;
using Server.Engines.Craft;
using Server.Items;
using Server.Mobiles;
using Server.Talent;
using Server.Utilities;
using Server.Network;

namespace Server.Pantheon
{
    public static class Deity
    {
        public const string DeityCurseModName = "DeityCurse";
        public enum Alignment
        {
            Charity,
            Greed,
            Order,
            Chaos,
            Light,
            Darkness,
            None
        }

        public static Alignment AlignmentFromString(string alignment) => (Alignment)Enum.Parse(typeof(Alignment), alignment, true);

        public const int OrderHue = 0xAAE;
        public const int DarknessHue = 0xAB7;
        public const int LightHue = 0xAB2;
        public const int ChaosHue = 0xAA7;
        public const int CharityHue = 0xB95;
        public const int GreedHue = 0xAA1;
        public static Type[] ExcludedTalents(Alignment alignment) {
            switch (alignment) {
                case Alignment.Chaos:
                    {
                        return new[]
                        {
                            typeof(DragonAspect),
                            typeof(LightAffinity),
                            typeof(GuardianLight),
                            typeof(HolyAvenger),
                            typeof(HolyBolt),
                            typeof(NatureAffinity),
                            typeof(RangerCommand),
                            typeof(BondingMaster),
                            typeof(PackLeader),
                            typeof(Cannibalism),
                            typeof(MerchantPorter),
                            typeof(Polymeter),
                            typeof(WhizzyGig),
                            typeof(ThingAMaBob),
                            typeof(Telewarper),
                            typeof(Inventive),
                            typeof(BugFixer),
                            typeof(Automaton),
                            typeof(AbyssalHunter),
                            typeof(Zealot),
                            typeof(DivineProtection),
                            typeof(SummonCelestial),
                            typeof(Heroism),
                            typeof(LayOnHands),
                            typeof(GodlyStrike),
                            typeof(SiphonLife),
                            typeof(WellOfDeath),
                            typeof(BloodLink),
                            typeof(Bribery),
                            typeof(Gluttony),
                            typeof(Deception),
                            typeof(Wisdom),
                            typeof(KeyToTheCity),
                            typeof(Leadership)
                        };
                    }
                case Alignment.Order:
                    {
                        return new[]
                        {
                            typeof(DarkAffinity),
                            typeof(SummonerCommand),
                            typeof(SpectralScream),
                            typeof(MasterOfDeath),
                            typeof(LightAffinity),
                            typeof(GuardianLight),
                            typeof(HolyAvenger),
                            typeof(HolyBolt),
                            typeof(SonicAffinity),
                            typeof(Resonance),
                            typeof(MassConfusion),
                            typeof(DominateCreature),
                            typeof(SmoothTalker),
                            typeof(TaxCollector),
                            typeof(Gambler),
                            typeof(HireHenchman),
                            typeof(ReptilianHunter),
                            typeof(SummonChaosElemental),
                            typeof(FlameWave),
                            typeof(ChaoticGrip),
                            typeof(Zealot),
                            typeof(DivineProtection),
                            typeof(SummonCelestial),
                            typeof(SiphonLife),
                            typeof(WellOfDeath),
                            typeof(BloodLink),
                            typeof(Bribery),
                            typeof(Gluttony),
                            typeof(Deception),
                            typeof(Wisdom),
                            typeof(KeyToTheCity),
                            typeof(Leadership)
                        };
                    }
                case Alignment.Light:
                    {
                        return new[]
                        {
                            typeof(ViperAspect),
                            typeof(VenomBlood),
                            typeof(WyvernAspect),
                            typeof(GreaterPoisonElemental),
                            typeof(DarkAffinity),
                            typeof(SummonerCommand),
                            typeof(SpectralScream),
                            typeof(MasterOfDeath),
                            typeof(SmoothTalker),
                            typeof(TaxCollector),
                            typeof(Gambler),
                            typeof(HireHenchman),
                            typeof(BoneWarmaster),
                            typeof(GraveDigger),
                            typeof(CharmUndead),
                            typeof(Mediumship),
                            typeof(Disenchant),
                            typeof(Enchant),
                            typeof(SummonChaosElemental),
                            typeof(FlameWave),
                            typeof(ChaoticGrip),
                            typeof(Heroism),
                            typeof(LayOnHands),
                            typeof(GodlyStrike),
                            typeof(SiphonLife),
                            typeof(WellOfDeath),
                            typeof(BloodLink),
                            typeof(Bribery),
                            typeof(Gluttony),
                            typeof(Deception),
                            typeof(Wisdom),
                            typeof(KeyToTheCity),
                            typeof(Leadership)

                        };
                    }
                case Alignment.Darkness:
                    {
                        return new[]
                        {
                            typeof(LightAffinity),
                            typeof(GuardianLight),
                            typeof(HolyAvenger),
                            typeof(HolyBolt),
                            typeof(LoreSeeker),
                            typeof(LoreDisciples),
                            typeof(LoreTeacher),
                            typeof(ChainWarmaster),
                            typeof(PlateWarmaster),
                            typeof(UndeadHunter),
                            typeof(NatureAffinity),
                            typeof(RangerCommand),
                            typeof(BondingMaster),
                            typeof(PackLeader),
                            typeof(Cannibalism),
                            typeof(SmoothTalker),
                            typeof(HireHenchman),
                            typeof(SummonChaosElemental),
                            typeof(FlameWave),
                            typeof(ChaoticGrip),
                            typeof(Zealot),
                            typeof(DivineProtection),
                            typeof(SummonCelestial),
                            typeof(Heroism),
                            typeof(LayOnHands),
                            typeof(GodlyStrike),
                            typeof(Bribery),
                            typeof(Gluttony),
                            typeof(Deception),
                            typeof(Wisdom),
                            typeof(KeyToTheCity),
                            typeof(Leadership)
                        };
                    }
                default:
                    {
                        return new[]
                        {
                            typeof(SummonChaosElemental),
                            typeof(FlameWave),
                            typeof(ChaoticGrip),
                            typeof(Zealot),
                            typeof(DivineProtection),
                            typeof(SummonCelestial),
                            typeof(Heroism),
                            typeof(LayOnHands),
                            typeof(GodlyStrike),
                            typeof(SiphonLife),
                            typeof(WellOfDeath),
                            typeof(BloodLink)
                        };
                    }
            }
        }



        public static bool AlignmentCheck(Mobile mobile, Alignment alignment, bool enemy)
        {
            if (mobile is BaseCreature)
            {
                return CreatureAlignmentCheck(mobile.GetType(), alignment, enemy);
            }
            if (mobile is PlayerMobile player)
            {
                var validTarget = alignment switch
                {
                    Alignment.Light    => enemy ? player.Alignment is Alignment.Darkness : player.Alignment is Alignment.Light,
                    Alignment.Darkness => enemy ? player.Alignment is Alignment.Light : player.Alignment is Alignment.Darkness,
                    Alignment.Order    => enemy ? player.Alignment is Alignment.Chaos : player.Alignment is Alignment.Order,
                    Alignment.Chaos    => enemy ? player.Alignment is Alignment.Order : player.Alignment is Alignment.Chaos,
                    _                  => false
                };
                return validTarget;
            }

            return false;
        }

        public static bool CreatureAlignmentCheck(Type type, Alignment alignment, bool enemy)
        {
            var validTarget = alignment switch
            {
                Alignment.Light => BaseTalent.IsMobileType(enemy ? OppositionGroup.UndeadGroup : OppositionGroup.HumanoidGroup, type),
                Alignment.Darkness => BaseTalent.IsMobileType(enemy ? OppositionGroup.HumanoidGroup : OppositionGroup.UndeadGroup, type),
                Alignment.Order => BaseTalent.IsMobileType(enemy ? OppositionGroup.AbyssalGroup : OppositionGroup.ReptilianGroup, type),
                Alignment.Chaos => BaseTalent.IsMobileType(enemy ? OppositionGroup.ReptilianGroup : OppositionGroup.AbyssalGroup, type),
                _ => false
            };
            return validTarget;
        }

        public static bool CanReceiveAlignment(Mobile mobile, Alignment alignment)
        {
            var validTarget = alignment switch
            {
                Alignment.Light or Alignment.Order => mobile switch
                {
                    BaseCreature creature => CreatureAlignmentCheck(creature.GetType(), Alignment.Light, false)
                                             && CreatureAlignmentCheck(creature.GetType(), Alignment.Order, false),
                    PlayerMobile player => player.Alignment is Alignment.None or Alignment.Light or Alignment.Order,
                    _                   => false
                },
                Alignment.Darkness => mobile switch
                {
                    BaseCreature creature =>  CreatureAlignmentCheck(creature.GetType(), Alignment.Darkness, false),
                    PlayerMobile player   => player.Alignment is Alignment.None or Alignment.Darkness,
                    _                     => false
                },
                Alignment.Greed => mobile switch
                {
                    BaseGuard => false,
                    BaseCreature creature => CreatureAlignmentCheck(creature.GetType(), Alignment.Light, false) && !creature.IsInvulnerable,
                    PlayerMobile player   => player.Alignment is Alignment.None or Alignment.Greed,
                    _                     => false
                },
                _ => false
            };
            return validTarget;
        }

        public static void PointsFromExperience(PlayerMobile player, Mobile killed, int baseExp)
        {
            int[] points = Array.Empty<int>();
            if (baseExp > 0 || killed is PlayerMobile)
            {
                var scaledPoints = killed is PlayerMobile killedPlayer ? killedPlayer.Level : baseExp / 25;
                if (AlignmentCheck(killed, player.Alignment, false))
                {
                    points = new[] { scaledPoints * -1 };
                } else if (AlignmentCheck(killed, player.Alignment, true))
                {
                    points = new[] { scaledPoints };
                }
            }
            RewardPoints(player, points, new[] { player.Alignment });
        }

        public static void BeginChallenge(PlayerMobile player)
        {
            Effect(player, player.Alignment);
            int amount = Utility.Random(100) < 50 ? 1 : Utility.RandomMinMax(2, 5);
            var buffs = amount == 1 ? Utility.RandomMinMax(2, 4) : Utility.RandomMinMax(1, 2);
            var minimumExpValue = amount == 1 ? 350 + player.Level * 10 : 450 + player.Level * (8 - amount);
            var maximumExpValue = amount == 1 ? 450 + player.Level * 20 : 450 + player.Level * (18 - amount);
            var mobileTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Namespace?.Contains("Mobiles") == true);

            List<Mobile> challengers = new List<Mobile>();
            foreach (var type in mobileTypes)
            {
                if (CreatureAlignmentCheck(type, player.Alignment, true) || player.Alignment is Alignment.Charity or Alignment.Greed)
                {
                    try
                    {
                        if (Activator.CreateInstance(type) is BaseCreature creature)
                        {
                            var dynamicExp = creature.DynamicExperienceValue();
                            if (
                                creature.IsInvulnerable == false
                                && dynamicExp >= minimumExpValue
                                && dynamicExp <= maximumExpValue
                            )
                            {
                                challengers.Add(creature);
                            }
                        }
                    }
                    catch
                    {
                        // continue
                    }
                }
            }

            if (challengers.Count > 0)
            {
                for (int i = 0; i < amount; i++)
                {
                    BaseCreature challenger = challengers[Utility.Random(challengers.Count)] as BaseCreature;
                    if (amount == 1 || i == 0)
                    {
                        var challengerBuffs = 0;
                        while (challengerBuffs < buffs)
                        {
                            if (Utility.Random(100) < 5 && !challenger.IsIllusionist)
                            {
                                challenger.IsIllusionist = true;
                                challengerBuffs++;
                            }
                            if (Utility.Random(100) < 5 && !challenger.IsCorruptor)
                            {
                                challenger.IsCorruptor = true;
                                challengerBuffs++;
                            }
                            if (Utility.Random(100) < 5 && !challenger.IsEthereal)
                            {
                                challenger.IsEthereal = true;
                                challengerBuffs++;
                            }
                            if (Utility.Random(100) < 5 && !challenger.IsIllusionist)
                            {
                                challenger.IsIllusionist = true;
                                challengerBuffs++;
                            }
                            if (Utility.Random(100) < 5 && !challenger.IsMagicResistant)
                            {
                                challenger.IsMagicResistant = true;
                                challengerBuffs++;
                            }
                            if (Utility.Random(100) < 5 && !challenger.IsFrozen)
                            {
                                challenger.IsFrozen = true;
                                challengerBuffs++;
                            }
                            if (Utility.Random(100) < 5 && !challenger.IsBurning)
                            {
                                challenger.IsBurning = true;
                                challengerBuffs++;
                            }
                            if (Utility.Random(100) < 5 && !challenger.IsElectrified)
                            {
                                challenger.IsElectrified = true;
                                challengerBuffs++;
                            }
                            if (Utility.Random(100) < 5 && !challenger.IsToxic)
                            {
                                challenger.IsToxic = true;
                                challengerBuffs++;
                            }
                            if (Utility.Random(100) < 5 && !challenger.IsReflective)
                            {
                                challenger.IsReflective = true;
                                challengerBuffs++;
                            }
                            if (Utility.Random(100) < 5 && !challenger.IsRegenerative)
                            {
                                challenger.IsRegenerative = true;
                                challengerBuffs++;
                            }
                            if (Utility.Random(100) < 5 && !challenger.IsSoulFeeder)
                            {
                                challenger.IsSoulFeeder = true;
                                challengerBuffs++;
                            }
                        }
                    }
                    bool goodLocation = false;
                    Point3D location = player.Location;
                    while (!goodLocation && location == player.Location)
                    {
                        location = player.Location;
                        location.X += Utility.RandomBool() ? Utility.RandomMinMax(5, 10) : Utility.RandomMinMax(-10, -5);
                        location.Y += Utility.RandomBool() ? Utility.RandomMinMax(5, 10) : Utility.RandomMinMax(-10, -5);
                        location.Y += Utility.RandomBool() ? Utility.RandomMinMax(5, 10) : Utility.RandomMinMax(-10, -5);
                        goodLocation = player.InLOS(location);
                    }
                    challenger.MoveToWorld(location, player.Map);
                }
                player.SendMessage("A challenge awaits you from the pantheon.");
                player.NextDeityChallenge = DateTime.Now.AddDays(1);
                player.DeityChallengers = challengers;
                Timer.StartTimer(TimeSpan.FromSeconds(1), player.ChallengeCheck);
            }
            else
            {
                player.SendMessage("The pantheon deems you too weak for a challenge.");
            }

        }

        public static void Effect(Mobile target, Alignment alignment)
        {
            int sound = 0;
            switch (alignment)
            {
                case Alignment.Order:
                    target.BoltEffect(0);
                    break;
                case Alignment.Chaos:
                    target.FixedParticles(0x3709, 10, 10, 5052, 0, 0, EffectLayer.LeftFoot, 0);
                    sound = 0x208;
                    break;
                case Alignment.Darkness:
                    target.FixedParticles(0x3728, 1, 13, 9912, 1150, 7, EffectLayer.Head);
                    target.FixedParticles(0x3779, 1, 15, 9502, 67, 7, EffectLayer.Head);
                    sound = 0xFC;
                    break;
                case Alignment.Light:
                    Effects.SendLocationParticles(
                        EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration),
                        0x3728,
                        8,
                        20,
                        5042
                    );
                    sound = 0x201;
                    break;
                case Alignment.Greed:
                case Alignment.Charity:
                    target.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                    sound = 0x1EA;
                    break;
                case Alignment.None:
                    break;
            }

            if (sound > 0)
            {
                target.SendSound(sound);
            }
        }

        public static readonly Type[] TalentTypes =
        {
            typeof(SummonChaosElemental),
            typeof(FlameWave)
        };

        public static bool HasDeityTalents(PlayerMobile player)
        {
            var hasTalent = false;
            foreach (var talentType in TalentTypes)
            {
                hasTalent = player.GetTalent(talentType) is not null;
                if (hasTalent)
                {
                    break;
                }
            }
            return hasTalent;
        }

        public static void BestowFavor(PlayerMobile player)
        {
            if (player.DeityPoints > 500 && HasDeityTalents(player))
            {
                player.HasDeityFavor = true;
                Effect(player, player.Alignment);
            }
            else
            {
                player.SendMessage("You do not meet the requirements to seek favor from the pantheon. You need to have an item with deity power equipped and at least 500 deity loyalty.");
            }
        }
        public static void RemoveFavor(PlayerMobile player)
        {
            player.HasDeityFavor = false;
        }

        public static void DestroyItem(Item[] items, PlayerMobile player, string context)
        {
            Item item = items[Utility.Random(items.Length)];
            player.SendMessage($"The gods have punished your disloyalty by destroying a {item.Name} from your {context}.");
            items[Utility.Random(items.Length)].Delete();
        }

        public static void BestowCurse(PlayerMobile player)
        {
            Effect(player, player.Alignment);
            if (Utility.Random(100) < 15)
            {
                int amount = Utility.RandomMinMax(3, 10);
                player.AddStatMod(new StatMod(StatType.All, DeityCurseModName, -amount, TimeSpan.FromMinutes(9)));
                player.SendMessage("The gods have punished your disloyalty with a curse.");
            } else if (Utility.Random(100) < 15 && player.Backpack is not null)
            {
                Item[] items = player.Backpack?.FindItemsByType(typeof(Item));
                if (items.Length > 0)
                {
                    DestroyItem(items, player, "backpack");
                } else if (player.BankBox is not null)
                {
                    items = player.BankBox?.FindItemsByType(typeof(Item));
                    if (items.Length > 0)
                    {
                        DestroyItem(items, player, "bank box");
                    }
                }
            } else if (Utility.Random(100) < 15 && player.AllFollowers.Count > 0)
            {
                Mobile follower = player.AllFollowers[Utility.Random(player.AllFollowers.Count)];
                player.SendMessage($"The gods have punished your disloyalty by removing one of your {follower.Name} followers.");
                follower.Delete();
            }
            else
            {
                player.Hunger -= Utility.RandomMinMax(1, 4);
                player.Thirst -= Utility.RandomMinMax(1, 4);
                player.SendMessage($"The gods have punished your disloyalty making you more hungry and thirsty.");
            }
        }

        public static void BestowReward(PlayerMobile player)
        {
            if (player.NextPrayer <= DateTime.Now)
            {
                List<SkillName> craftingSkillNames = BaseTalent.GetPlayerSkillNames(player, true);
                List<SkillName> combatSkillNames = BaseTalent.GetPlayerSkillNames(player, false);
                if (BestowHighReward(player))
                {
                    player.NextPrayer = DateTime.Now.AddHours(12);
                    player.DeityPoints -= 1000;
                    return;
                }
                if (BestowMediumReward(player, craftingSkillNames, combatSkillNames))
                {
                    player.NextPrayer = DateTime.Now.AddHours(6);
                    player.DeityPoints -= 500;
                    return;
                }
                if (BestowLowReward(player, craftingSkillNames, combatSkillNames))
                {
                    player.DeityPoints -= 100;
                }
                else
                {
                    player.SendMessage("You have not appeased your deity enough.");
                }
                player.NextPrayer = DateTime.Now.AddHours(1);
            }
            else
            {
                player.SendMessage("You cannot pray to the pantheon for a reward yet.");
            }
        }

        public static void RewardMessage(PlayerMobile player)
        {
            player.LocalOverheadMessage(MessageType.Regular, 0x0481, false, "*** The pantheon have granted you a reward ***");
        }

        public static bool TrySacrifice(PlayerMobile player, int chance, LootPack? lootPack, Item? item)
        {
            if (Utility.Random(100) < chance)
            {
                BaseCreature sacrifice = player.Alignment switch
                {
                    Alignment.Chaos    => new Gargoyle(),
                    Alignment.Order    => new Lizardman(),
                    Alignment.Darkness => new Shade(),
                    Alignment.Light    => new Orc(),
                    Alignment.Greed    => new EvilHealer(),
                    Alignment.Charity  => new Thief(),
                    _                  => null
                };
                if (sacrifice is not null)
                {
                    BaseTalent.EmptyCreatureBackpack(sacrifice);
                    if (lootPack is not null)
                    {
                        sacrifice.ForceRandomLoot(lootPack, Utility.RandomMinMax(1, 3), 1);
                    } else if (item is not null)
                    {
                        sacrifice.AddToBackpack(item);
                    }

                    Point3D newLocation = new Point3D(player.Location.X + 1, player.Location.Y + 1, player.Location.Z);
                    sacrifice.MoveToWorld(newLocation, player.Map);
                    Effect(sacrifice, player.Alignment);
                    sacrifice.Kill();
                    sacrifice.LastKiller = player;
                    RewardMessage(player);
                    return true;
                }
            }
            return false;
        }

        private static bool BestowLowReward(PlayerMobile player, List<SkillName> craftingSkillNames, List<SkillName> combatSkillNames)
        {
            if (player.DeityPoints > 100)
            {
                // lets try rewarding a crafting skill
                if (craftingSkillNames.Count > 0)
                {
                    SkillName name = craftingSkillNames[Utility.Random(craftingSkillNames.Count)];
                    Bag bag = name switch
                    {
                        SkillName.Alchemy or SkillName.Inscribe     => new BagOfReagents(),
                        SkillName.Blacksmith or SkillName.Tinkering => new BagOfingots(),
                        _                                           => null
                    };
                    if (bag is not null && TrySacrifice(player, 15, null, bag))
                    {
                        return true;
                    }
                    Item item = name switch
                    {
                        SkillName.Cooking     => new SmallCookingBOD(),
                        SkillName.Cartography => new TreasureMap(1, player.Map),
                        SkillName.Carpentry   => new Board(Utility.RandomMinMax(500,1000)),
                        SkillName.Fletching   => new Feather(Utility.RandomMinMax(500,1000)),
                        SkillName.Tailoring   => new Leather(Utility.RandomMinMax(500,1000)),
                        _                     => null
                    };
                    if (item is not null && TrySacrifice(player, 15, null, item))
                    {
                        return true;
                    }
                } else if (combatSkillNames.Count > 0)
                {
                    if (TrySacrifice(player, 15, LootPack.Average, null))
                    {
                        return true;
                    }
                    if (TrySacrifice(player, 15, LootPack.Meager, null))
                    {
                        return true;
                    }
                    if (TrySacrifice(player, 33, null, new Gold(Utility.RandomMinMax(2500, 5000))))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool BestowMediumReward(PlayerMobile player, List<SkillName> craftingSkillNames, List<SkillName> combatSkillNames)
        {
            if (player.DeityPoints > 500)
            {
                // lets try rewarding a crafting skill
                if (craftingSkillNames.Count > 0)
                {
                    SkillName name = craftingSkillNames[Utility.Random(craftingSkillNames.Count)];
                    Item item = null;
                    if (name == SkillName.Cartography)
                    {
                        item = new TreasureMap(Utility.RandomMinMax(1, 3), player.Map);
                    } else if (name == SkillName.Cooking)
                    {
                        item = new LargeCookingBOD();
                    }
                    else
                    {
                        int recipe = name switch
                        {
                            SkillName.Alchemy    => DefAlchemy.CraftSystem.RandomRecipe(),
                            SkillName.Blacksmith => DefBlacksmithy.CraftSystem.RandomRecipe(),
                            SkillName.Carpentry  => DefBlacksmithy.CraftSystem.RandomRecipe(),
                            SkillName.Fletching  => DefBowFletching.CraftSystem.RandomRecipe(),
                            SkillName.Inscribe   => DefInscription.CraftSystem.RandomRecipe(),
                            SkillName.Tailoring  => DefTailoring.CraftSystem.RandomRecipe(),
                            SkillName.Tinkering  => DefTinkering.CraftSystem.RandomRecipe(),
                            _                    => -1
                        };
                        if (recipe != -1)
                        {
                            item = new RecipeScroll(recipe);
                        }
                    }
                    if (item is not null && TrySacrifice(player, 8, null, item))
                    {
                        return true;
                    }
                } else if (combatSkillNames.Count > 0)
                {
                    if (TrySacrifice(player, 5, LootPack.FilthyRich, null))
                    {
                        return true;
                    }
                    if (TrySacrifice(player, 15, LootPack.Rich, null))
                    {
                        return true;
                    }
                }

            }
            return false;
        }

        public static bool BestowHighReward(PlayerMobile player)
        {
            if (player.DeityPoints > 1000)
            {
                if (Utility.Random(800) < 1)
                {
                    var excludedTalents = ExcludedTalents(player.Alignment);
                    // Charity and Greed has no talent exclusions, but only allow + 1
                    var talentLevel = player.Alignment is Alignment.Charity or Alignment.Greed ? 1 : Utility.RandomMinMax(1, 3);
                    var resolved = false;
                    while (!resolved)
                    {
                        Type talentType = BaseTalent.TalentTypes[Utility.Random(BaseTalent.TalentTypes.Length)];
                        if (!Array.Exists(excludedTalents, type => type == talentType))
                        {
                            var scroll = new PantheonScroll
                            {
                                TalentIndex = Utility.Random(BaseTalent.TalentTypes.Length),
                                TalentLevel = talentLevel,
                                AlignmentRaw = player.Alignment.ToString()
                            };
                            player.AddToBackpack(scroll);
                            resolved = true;
                        }
                    }
                    RewardMessage(player);
                    return true;
                }
                if (Utility.Random(500) < 1)
                {
                    var rune = new RuneWord();
                    player.AddToBackpack(rune);
                    RewardMessage(player);
                    return true;
                }
                if (Utility.Random(100) < 5)
                {
                    BaseCreature mount = Utility.RandomBool() ? new Horse() : new Llama();
                    mount = player.Alignment switch
                    {
                        Alignment.Chaos    => new ChaosSteed(),
                        Alignment.Order    => new OrderSteed(),
                        Alignment.Darkness => new DarknessSteed(),
                        Alignment.Light    => new LightSteed(),
                        Alignment.Greed    => new GreedSteed(),
                        Alignment.Charity  => new CharitySteed(),
                        _                  => mount
                    };
                    mount.SetControlMaster(player);
                    RewardMessage(player);
                    return true;
                }
            }
            return false;
        }

        public static void RewardPoints(PlayerMobile player, int[] points, Alignment[] alignments)
        {
            if (points.Length == alignments.Length)
            {
                int maxPoints = player.Level * 35;
                for (var i = 0; i < alignments.Length; i++)
                {
                    if (player.Alignment == alignments[i] && player.DeityPoints < maxPoints)
                    {
                        player.DeityPoints += points[i];
                        if (player.DeityPoints > maxPoints)
                        {
                            player.DeityPoints = maxPoints;
                        }
                    }
                }
            }
        }
    }
}


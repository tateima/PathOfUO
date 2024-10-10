using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Server.Engines.BulkOrders;
using Server.Engines.Craft;
using Server.Ethics;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Talent;
using Server.Network;

namespace Server.Pantheon
{
    public static class Deity
    {
        public const string DeityCurseModName = "DeityCurse";
        public const string DeityFavorModName = "DeityFavor";
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
                        return TalentTypes;
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

        public static Alignment GetCreatureAlignment(Type type)
        {
            Alignment alignment = Alignment.None;
            foreach (var alignmentType in new[]
                     {
                         Alignment.Light,
                         Alignment.Darkness,
                         Alignment.Chaos,
                         Alignment.Order
                     }
                    )
            {
                if (CreatureAlignmentCheck(type, alignmentType, false))
                {
                    alignment = alignmentType;
                }
            }
            return alignment;
        }

        public static bool CreatureAlignmentCheck(Type type, Alignment alignment, bool enemy)
        {
            var validTarget = alignment switch
            {
                Alignment.Light => BaseTalent.IsMobileType(enemy ? OppositionGroup.DarknessAndLight[1] : OppositionGroup.DarknessAndLight[0], type),
                Alignment.Darkness => BaseTalent.IsMobileType(enemy ? OppositionGroup.DarknessAndLight[0] : OppositionGroup.DarknessAndLight[1], type),
                Alignment.Order => BaseTalent.IsMobileType(enemy ? OppositionGroup.ChaosAndOrder[1] : OppositionGroup.ChaosAndOrder[0], type),
                Alignment.Chaos => BaseTalent.IsMobileType(enemy ? OppositionGroup.ChaosAndOrder[0] : OppositionGroup.ChaosAndOrder[1], type),
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
                if (scaledPoints < 1)
                {
                    scaledPoints = 1;
                }
                if (AlignmentCheck(killed, player.Alignment, false))
                {
                    points = new[] { scaledPoints * -3 };
                } else if (AlignmentCheck(killed, player.Alignment, true))
                {
                    points = new[] { scaledPoints };
                }
            }
            RewardPoints(player, points, new[] { player.Alignment });
        }

        public static List<Mobile> FindGuardians(PlayerMobile player, int maximum = 5, int minimum = 2, bool allowNonAlignment = false, bool enemy = true)
        {
            int amount = Utility.Random(100) < 50 ? 1 : Utility.RandomMinMax(minimum, maximum);
            var minimumExpValue = amount == 1 ? 150 + player.Level * 10 : 450 + player.Level * (8 - amount);
            var maximumExpValue = amount == 1 ? 550 + player.Level * 20 : 450 + player.Level * (18 - amount);
            var mobileTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Namespace?.Contains("Mobiles") == true);
            List<Mobile> challengers = new List<Mobile>();
            foreach (var type in mobileTypes)
            {
                if (CreatureAlignmentCheck(type, player.Alignment, enemy) || player.Alignment is Alignment.Charity or Alignment.Greed || allowNonAlignment && player.Alignment is Alignment.None)
                {
                    try
                    {
                        if (Activator.CreateInstance(type) is BaseCreature creature)
                        {
                            var dynamicExp = creature.DynamicExperienceValue();
                            if (
                                !creature.IsInvulnerable
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
            return challengers;
        }


        public static void BeginChallenge(PlayerMobile player, int maximum = 5, int minimum = 2)
        {
            Effect(player, player.Alignment);
            int amount = Utility.Random(100) < 50 ? 1 : Utility.RandomMinMax(minimum, maximum);
            var buffs = amount == 1 ? Utility.RandomMinMax(1, 3) : Utility.RandomMinMax(1, 1);
            List<Mobile> challengers = FindGuardians(player, maximum, minimum);
            if (challengers.Count > 0)
            {
                for (int i = 0; i < amount; i++)
                {
                    BaseCreature challenger = challengers[Utility.Random(challengers.Count)] as BaseCreature;
                    if (challenger is not null)
                    {
                        challenger.SetLevel();
                        if (!MonsterBuff.CannotBeAltered(challenger))
                        {
                            MonsterBuff.BalanceCreatureAgainstMobile(challenger, player);
                        }
                        if (Utility.Random(100) < 50)
                        {
                            challenger.IsVeteran = true;
                        }
                        else
                        {
                            challenger.IsHeroic = true;
                        }
                        if (amount == 1 || i == 0)
                        {
                            MonsterBuff.RandomMonsterBuffs(challenger, buffs);
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
                }
                player.SendMessage("A challenge awaits you from the pantheon.");
                player.NextDeityChallenge = DateTime.Now.AddDays(1);
                player.DeityChallengers = challengers;
                Timer.StartTimer(TimeSpan.FromSeconds(10), player.ChallengeCheck, out player.ChallengeTimer);
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
                    target.FixedParticles(0x3709, 1, 15, 5052, 0, 0, EffectLayer.LeftFoot, 0);
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
                    target.FixedParticles(0x374A, 1, 15, 5021, EffectLayer.Waist);
                    target.PlaySound(0x205);
                    break;
                case Alignment.Charity:
                    target.FixedParticles(0x373A, 1, 15, 5018, EffectLayer.Waist);
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
            typeof(BloodLink),
            typeof(Bribery),
            typeof(Gluttony),
            typeof(Deception),
            typeof(Wisdom),
            typeof(KeyToTheCity),
            typeof(Leadership)
        };

        public static bool HasDeityTalents(PlayerMobile player)
        {
            var hasTalent = false;
            foreach (var talentType in TalentTypes)
            {
                hasTalent = player.HasTalent(talentType);
                if (hasTalent)
                {
                    break;
                }
            }
            return hasTalent;
        }

        public static void BestowFavor(PlayerMobile player,  Alignment effectAlignment, double favorStrength = 1.0)
        {
            if (player.DeityPoints > 250 && !player.HasDeityFavor)
            {
                Effect(player, effectAlignment);
                player.HasDeityFavor = true;
                Timer.StartTimer(TimeSpan.FromHours(1 * favorStrength),() => RemoveFavor(player));
                player.SendMessage("The gods have rewarded a buff to hunt down your sworn enemies with increased damage you additional protection.");
                player.DeityPoints -= 250;
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

        public static void DestroyItem(PlayerMobile player, string contextOverride = "")
        {
            Container.FindItemsByTypeEnumerator<Item> items = player.Backpack.FindItemsByType(typeof(Item));
            Container.FindItemsByTypeEnumerator<Item> bankItems = player.BankBox.FindItemsByType(typeof(Item));
            string context = "";
            Item item = null;
            foreach (var backpackItem in items)
            {
                context = "backpack";
                item = backpackItem;
                break;
            }

            if (string.IsNullOrEmpty(context))
            {
                foreach (var bankItem in bankItems)
                {
                    context = "bank box";
                    item = bankItem;
                    break;
                }
            }

            if (item is not null)
            {
                if (contextOverride.Length > 0)
                {
                    context = contextOverride;
                }
                item.Delete();
                player.SendMessage($"The gods have punished you by destroying a {SocketBonus.GetItemName(item)} from your {context}.");
            }
        }

        public static void BestowCurse(PlayerMobile player, Alignment effectAlignment, double curseStrength = 1.0)
        {
            Effect(player, effectAlignment);
            if (Utility.Random(100) < 15)
            {
                DecideBuff(player, DeityCurseModName, curseStrength, true);
                player.SendMessage("The gods have punished you with a curse.");
            } else if (Utility.Random(100) < 15 && player.Backpack is not null)
            {
                DestroyItem(player);
            } else if (Utility.Random(100) < 15 && player.AllFollowers.Count > 0)
            {
                Mobile follower = player.AllFollowers.ElementAt(Utility.Random(player.AllFollowers.Count));
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

        private static void RemoveSkills(PlayerMobile player, List<DefaultSkillMod> skillMods)
        {
            foreach (var skillMod in skillMods)
            {
                player.RemoveSkillMod(skillMod);
            }
        }

        public static void BestowReward(PlayerMobile player)
        {
            if (player.NextPrayer <= DateTime.Now)
            {
                List<SkillName> craftingSkillNames = BaseTalent.GetPlayerSkillNames(player, true, false);
                List<SkillName> combatSkillNames = BaseTalent.GetPlayerSkillNames(player, false, false);
                if (BestowHighReward(player))
                {
                    player.FailedDeityPrayers = 0;
                    player.NextPrayer = DateTime.Now.AddHours(12);
                    player.DeityPoints -= 1000;
                    return;
                }
                if (BestowMediumReward(player, craftingSkillNames, combatSkillNames))
                {
                    player.FailedDeityPrayers = 0;
                    player.NextPrayer = DateTime.Now.AddHours(6);
                    player.DeityPoints -= 500;
                    return;
                }
                if (BestowLowReward(player, craftingSkillNames, combatSkillNames))
                {
                    player.FailedDeityPrayers = 0;
                    player.DeityPoints -= 100;
                }
                else
                {
                    if (player.DeityPoints >= 100)
                    {
                        player.FailedDeityPrayers++;
                    }
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
            if (Utility.Random(100) < chance + player.FailedDeityPrayers)
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
                    ((Corpse)sacrifice.Corpse).Owner = player;
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
                if (Utility.Random(500) < 1 + player.FailedDeityPrayers)
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
                if (Utility.Random(400) < 1 + player.FailedDeityPrayers)
                {
                    var rune = new RuneWord();
                    player.AddToBackpack(rune);
                    RewardMessage(player);
                    return true;
                }
                if (Utility.Random(100) < 5 + player.FailedDeityPrayers)
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

        public static void DecideBuff(PlayerMobile player, string deityModName, double strength, bool isDebuff)
        {
            double amount = Utility.RandomMinMax(3, 10) * strength;
            if (Utility.Random(100) < 50)
            {
                amount *= isDebuff ? -1 : 1;
                player.AddStatMod(new StatMod(StatType.All, deityModName, (int)amount, TimeSpan.FromMinutes(15 * strength)));
            }
            else
            {
                double modifier = isDebuff ? -10.0 : 10.0;
                List<Skill> skills = new List<Skill>();
                BaseTalent.GetTopSkills(player, ref skills, 3);
                List<DefaultSkillMod> skillMods = BaseTalent.GetTopDefaultSkillMods(skills, modifier * strength, deityModName);
                foreach (var skillMod in skillMods)
                {
                    player.AddSkillMod(skillMod);
                }
                Timer.StartTimer(TimeSpan.FromMinutes(15 * strength),() => RemoveSkills(player, skillMods));
            }
        }
    }
}


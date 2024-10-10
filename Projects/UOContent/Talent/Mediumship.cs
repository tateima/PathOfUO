using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Regions;

namespace Server.Talent
{
    public class Mediumship : BaseTalent
    {
        public Mediumship()
        {
            DisplayName = "Mediumship";
            CanBeUsed = true;
            CooldownSeconds = 60;
            Description =
                "Allows communication with dead spirits at specific locations. Each level unlocks more events. Requires 20-100 spiritspeak.";
            ImageID = 407;
            MaxLevel = 5;
            GumpHeight = 95;
            AddEndY = 105;
            MaxLevel = 5;
        }

        public override bool HasSkillRequirement(Mobile mobile)
        {
            return Level switch
            {
                0 => mobile.Skills.SpiritSpeak.Base >= 20.0,
                1 => mobile.Skills.SpiritSpeak.Base >= 40.0,
                2 => mobile.Skills.SpiritSpeak.Base >= 60.0,
                3 => mobile.Skills.SpiritSpeak.Base >= 80.0,
                4 => mobile.Skills.SpiritSpeak.Base >= 100.0,
                5 => true,
                _ => false
            };
        }

        public override void OnUse(Mobile from)
        {
        }

        public static HauntedScroll GetPlayerScroll(Mobile from)
        {
            if (from.Backpack != null)
            {
                var scrolls = from.Backpack.FindItemsByType(typeof(HauntedScroll));
                HauntedScroll scroll = scrolls.Current as HauntedScroll;
                if (scroll == null)
                {
                    from.SendMessage(
                        "Thou spirit connection cannot sustain more than one story at a time. One scroll in thy pack is required."
                    );
                }
                else
                {
                    return scroll;
                }
            }

            return null;
        }

        public static SpiritBoard GetSpiritBoard(Mobile from) => from.Backpack?.FindItemByType<SpiritBoard>();

        public void CheckConnection(Mobile from)
        {
            if (!OnCooldown && HasSkillRequirement(from))
            {
                OnCooldown = true;
                var scroll = GetPlayerScroll(from);
                var roll = Utility.Random(5);
                if (scroll != null && (from.Map == Map.Trammel || from.Map == Map.Felucca))
                {
                    var chapterLocation = scroll.ChapterLocation;
                    if (from.InRange(chapterLocation, 3))
                    {
                        UpdateHauntedScroll(from);
                    }
                }
                else if (roll < Level)
                {
                    GiveHauntedScroll(from);
                }

                Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
            }
        }

        public static void ProcessItemReward(
            double packChance, double weaponChance, double dustChance, double mapChance, double monsterChance,
            ref Item item, ref LootPack loot, ref TreasureMap map, Mobile from, ref BaseCreature enemy
        )
        {
            if (Utility.Random(100) < monsterChance && !from.Region.IsPartOf<TownRegion>())
            {
                // its a trap!
                enemy = MasterOfDeath.RandomUndead();
                enemy.SetLevel();
            }

            if (Utility.Random(100) < dustChance)
            {
                item = new EnchanterDust(Utility.Random(5));
            }
            else if (Utility.Random(100) < packChance)
            {
                loot = LootPack.Average;
            }
            else if (Utility.Random(100) < weaponChance)
            {
                item = Loot.RandomWeapon();
            }

            if (Utility.Random(200) < mapChance)
            {
                map = new TreasureMap(Utility.RandomMinMax(1, 3), from.Map);
            }
        }

        public void UpdateHauntedScroll(Mobile from)
        {
            var scroll = GetPlayerScroll(from);
            if (scroll != null)
            {
                var maxHookTier = Level == MaxLevel ? 5 : Level + 1;
                if (scroll.HookNumber < maxHookTier)
                {
                    scroll.HookNumber++;
                    var hooks = new List<string>(scroll.Content);
                    var hook = "";
                    BaseCreature enemy = null;
                    Item item = null;
                    LootPack loot = null;
                    var gold = new Gold(Utility.Random(scroll.HookNumber * 150));
                    TreasureMap map = null;
                    switch (scroll.HookNumber)
                    {
                        case 2:
                            ProcessItemReward(3, 0, 0, 0, 10, ref item, ref loot, ref map, from, ref enemy);
                            hook = HauntedHook.RandomHook("hook_two");
                            break;
                        case 3:
                            ProcessItemReward(4, 5, 0, 0, 10, ref item, ref loot, ref map, from, ref enemy);
                            hook = HauntedHook.RandomHook("hook_three");
                            break;
                        case 4:
                            ProcessItemReward(8, 8, 3, 1, 15, ref item, ref loot, ref map, from, ref enemy);
                            hook = HauntedHook.RandomHook("hook_four");
                            break;
                        case 5:
                            ProcessItemReward(13, 13, 3, 2, 20, ref item, ref loot, ref map, from, ref enemy);
                            hook = HauntedHook.RandomHook("hook_five");
                            break;
                        case 6:
                            ProcessItemReward(15, 15, 5, 3, 30, ref item, ref loot, ref map, from, ref enemy);
                            if (Utility.Random(1000) < 1)
                            {
                                var bird = new Eagle
                                {
                                    Name = "a spectral parrot",
                                    Hue = MonsterBuff.EtherealHue
                                };
                                bird.MoveToWorld(from.Location, from.Map);
                                from.SendMessage("An unusual bird from the netherworld flies out of the book");
                            }

                            if (Utility.Random(2000) < 1)
                            {
                                EtherealMount creature = Utility.RandomMinMax(1, 3) switch
                                {
                                    1 => new EtherealHorse(),
                                    2 => new EtherealOstard(),
                                    3 => new EtherealLlama(),
                                    _ => null
                                };

                                if (creature != null)
                                {
                                    creature.MoveToWorld(from.Location, from.Map);
                                    from.SendMessage("An unusual mount from the netherworld leaps out of the book");
                                }
                            }

                            break;
                    }

                    from.PublicOverheadMessage(
                        MessageType.Regular,
                        0x3B2,
                        false,
                        "* rewards from the netherworld appear in your backpack for solving the chapter *"
                    );
                    if (item != null)
                    {
                        if (item is BaseWeapon weapon)
                        {
                            weapon.Haunted = true;
                        }

                        from.Backpack.AddItem(item);
                    }

                    loot?.ForceGenerate(from, from.Backpack, loot.RandomEntry(), 1);

                    if (map != null)
                    {
                        from.Backpack.AddItem(map);
                    }

                    if (enemy != null)
                    {
                        from.SendMessage($"{enemy.Name} appears out of the book!");
                        var enemyLocation = new Point3D(scroll.ChapterLocation.X, scroll.ChapterLocation.Y, from.Z);
                        enemy.MoveToWorld(enemyLocation, from.Map);
                        Effects.PlaySound(from.Location, from.Map, 0x1FB);
                        Effects.SendLocationParticles(
                            EffectItem.Create(enemy.Location, enemy.Map, EffectItem.DefaultDuration),
                            0x37CC,
                            1,
                            40,
                            97,
                            3,
                            9917,
                            0
                        );
                    }

                    from.Backpack.AddItem(gold);
                    ((PlayerMobile)from).NonCraftExperience += Utility.Random(Level * 105);
                    string message;
                    if (scroll.HookNumber >= maxHookTier)
                    {
                        message = "The book shatters as you have resolved it as much as you are able";
                        from.SendSound(0x040);
                        scroll.Delete();
                    }
                    else
                    {
                        var author = scroll.Protagonist;
                        hook = hook.Replace("@author", author);
                        var location = HauntedHook.DecideLocation(ref hook, scroll.MapKey);
                        hooks.Add($"Chapter {scroll.HookNumber.ToString()}, {HauntedHook.Parse(hook)}.");
                        hooks.Add(ScrambleLocation(location.ToString()));
                        var newHooks = hooks.ToArray();
                        scroll.Content = newHooks;
                        scroll.ChapterLocation = location;
                        message =
                            "A voice from the netherworld speaks to you. New words appear in the scroll before your eyes.";
                        Effects.PlaySound(from.Location, from.Map, 0x1FB);
                    }

                    from.PublicOverheadMessage(
                        MessageType.Regular,
                        0x3B2,
                        false,
                        message
                    );
                }
                else
                {
                    from.SendMessage("You cannot progress this book any further at this time.");
                }

                return;
            }

            from.SendMessage("There are no haunted books in thy pack.");
        }

        public static string ScrambleLocation(string line)
        {
            const string pattern = @"(\d)\d?";
            return $"The next clue can be found at the co-ordinates {Regex.Replace(line, pattern, "$1?")}";
        }

        public static void GiveHauntedScroll(Mobile from)
        {
            const string mapKey = "trammel_";
            //if (from.Map == Map.Ilshenar) {
            //    mapKey = "ilshenar_"; // to do
            //} else if (from.Map == Map.Malas) {
            //    mapKey = "malas_"; // to do
            //} else if (from.Map == Map.TerMur) {
            //    mapKey = "termur_"; // to do
            //} else if (from.Map == Map.Tokuno) {
            //    mapKey = "tokuno_"; // to do
            //}
            var protagonist = Utility.RandomBool() ? NameList.RandomName("male") : NameList.RandomName("female");
            var hook = HauntedHook.RandomHook("hook_one");
            hook = hook.Replace("@author", protagonist);
            var location = HauntedHook.DecideLocation(ref hook, mapKey);
            var hooks = new List<string>
            {
                $"Chapter 1, {HauntedHook.Parse(hook)}.",
                ScrambleLocation(location.ToString())
            };
            var scroll = new HauntedScroll(protagonist, hooks, mapKey)
            {
                ChapterLocation = location
            };
            if (from.Backpack != null)
            {
                from.Backpack.DropItem(scroll);
                var board = GetSpiritBoard(from) ?? new SpiritBoard();
                from.Backpack.DropItem(board);
                from.PublicOverheadMessage(
                    MessageType.Regular,
                    0x3B2,
                    false,
                    "A voice from the netherworld speaks to you. A scroll materializes before your eyes with words from the grave"
                );
                Effects.PlaySound(from.Location, from.Map, 0x1FB);
            }
        }
    }
}

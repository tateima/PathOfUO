using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Regions;

namespace Server.Talent
{
    public class Detective : BaseTalent, ITalent
    {
        public Detective() : base()
        {
            DisplayName = "Detective";
            CanBeUsed = true;
            Description = "Unlocks crime cases to solve. Requires 40+ Forensic and 40+ Detect Hidden";
            ImageID = 408;
            MaxLevel = 5;
            GumpHeight = 95;
            AddEndY = 105;
            MaxLevel = 5;
        }

        
        public override bool HasSkillRequirement(Mobile mobile)
        {
            switch(Level)
            {
                case 0:
                    return mobile.Skills.Forensics.Base >= 40.0 && mobile.Skills.DetectHidden.Base >= 40.0;
                    break;
                case 1:
                    return mobile.Skills.Forensics.Base >= 50.0 && mobile.Skills.DetectHidden.Base >= 50.0;
                    break;
                case 2:
                    return mobile.Skills.Forensics.Base >= 60.0 && mobile.Skills.DetectHidden.Base >= 60.0;
                    break;
                case 3:
                    return mobile.Skills.Forensics.Base >= 70.0 && mobile.Skills.DetectHidden.Base >= 70.0;
                    break;
                case 4:
                    return mobile.Skills.Forensics.Base >= 80.0 && mobile.Skills.DetectHidden.Base >= 80.0;
                    break;
                case 5:
                    return mobile.Skills.Forensics.Base >= 90.0 && mobile.Skills.DetectHidden.Base >= 90.0;
                    break;
            }
            return false;
        }

        public CaseNote GetPlayerCaseNote(Mobile from) {
            if (from.Backpack != null) {
                var caseNotes = from.Backpack.FindItemsByType<CaseNote>();
                 if (caseNotes.Count > 1) {
                    from.SendMessage("You can only work on one case at a time.");
                } else {
                    return caseNotes[1];
                }
            }
            return null;
        }

        public void CheckClue(Mobile from) {
            CaseNote note = GetPlayerCaseNote(from);
            int roll = Utility.Random(5);
            if (note != null && (from.Map == Map.Trammel || from.Map == Map.Felucca)) {
                foreach(Clue clue in note.Clues) {
                    if (from.InRange(clue.Location, 1) && from.CheckSkill(SkillName.DetectHidden, (double)(clue.Difficulty - Utility.Random(10)), (double)clue.Difficulty)) {
                        clue.CreateClueItem();
                        from.PublicOverheadMessage(
                            MessageType.Regular,
                            0x3B2,
                            false,
                            "* You find a clue! *"
                        );
                        if (from.Backpack != null) {
                            from.Backpack.DropItem(clue.Item);
                        }
                        break;
                    }
                }
            } else {
                from.PublicOverheadMessage(
                        MessageType.Regular,
                        0x3B2,
                        false,
                        "* You do not have a case *"
                        );
            }
        }

        public Clue CheckSolve(Mobile from, Item item) {
            CaseNote note = GetPlayerCaseNote(from);
            if (note != null) {
                Clue itemClue = note.Clues.Find(c => c.Item.Serial == item.Serial);
                if (itemClue != null && !itemClue.Solved) {
                    itemClue.Solved = from.CheckSkill(SkillName.Forensics, (double)(itemClue.Difficulty-Utility.Random(10)), (double)(itemClue.Difficulty));
                    string message = "* You fail to find anything on this item *";
                    if (itemClue.Solved) {
                        message = "* You have solved the clue! *"; 
                        var index = note.Clues.FindIndex(c => c.Item.Serial == item.Serial);
                        note.Clues[index] = itemClue;
                    } 
                    from.PublicOverheadMessage(
                            MessageType.Regular,
                            0x3B2,
                            false,
                            message
                            );
                }                
                return itemClue;
            }
            return null;
        }

        public bool GiveRewards(PlayerMobile from, CaseNote caseNote) {
            List<Clue> solved = caseNote.Clues.Where(c => c.Solved).ToList<Clue>();
            if (from.Backpack != null && solved.Count > 0) {
                int goldAmount = Utility.Random(solved.Count*Level);
                from.NonCraftExperience += Utility.Random(75 * Level);
                from.Backpack.DropItem(new Gold(goldAmount));
                Item item = null;
                LootPack loot = null;
                TreasureMap map = null;
                ProcessItemReward(0.5 * Utility.Random(solved.Count + Level), 1.5 * Utility.Random(solved.Count + Level), 0.3 * Utility.Random(solved.Count + Level), ref item, ref loot, ref map, from);
                if (item != null) {
                    from.Backpack.AddItem(item);
                }
                if (loot != null) {
                    loot.ForceGenerate(from, from.Backpack, loot.RandomEntry(), 1);
                }
                if (map != null) {
                    from.Backpack.AddItem(map);
                }
                if (Utility.Random(10000) < 1) {
                    from.Backpack.AddItem(new DetectiveBoots());
                } else if (Utility.Random(10000) < 1) {
                    from.Backpack.AddItem(new LieutenantOfTheBritannianRoyalGuard());
                } else if (Utility.Random(1000) < 1) {
                    from.Backpack.AddItem(new SamaritanRobe());
                }
                foreach(Clue clue in solved) {
                    clue.Item.Delete();
                }
                caseNote.Delete();
                return true;
            }
            return false;
        }

        public void ProcessItemReward(double packChance, double weaponChance, double mapChance,
                                    ref Item item, ref LootPack loot, ref TreasureMap map, Mobile from) {
            if (Utility.Random(100) < packChance) { 
                loot = LootPack.Average;
            } else if (Utility.Random(100) < weaponChance) {
                item = Loot.RandomWeapon();
            }
            if (Utility.Random(200) < mapChance) {
                map = new TreasureMap(Utility.RandomMinMax(1,3), from.Map);
            }
        }

        public string ScrambleLocation(string line) {
            var pattern = @"(\d)\d?";
            return string.Format("The next clue can be found at the co-ordinates {0}", Regex.Replace(line, pattern, "$1?"));
        }

        public void GiveCaseNote(Mobile from) {
            CaseNote caseNote = new CaseNote();
            if (from.Backpack != null) {
                from.Backpack.DropItem(caseNote);
                from.PublicOverheadMessage(
                       MessageType.Regular,
                       0x3B2,
                       false,
                       "* The guard gives you an authority document concerning a case *"
                     );
                Effects.PlaySound(from.Location, from.Map, 0x1FB);
            }
        }
    }
}

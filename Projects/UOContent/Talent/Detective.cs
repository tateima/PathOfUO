using System.Linq;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Talent
{
    public class Detective : BaseTalent
    {
        public Detective()
        {
            DisplayName = "Detective";
            CanBeUsed = true;
            Description = "Unlocks crime cases to solve. Requires 40+ Forensic and 40+ Detect Hidden";
            ImageID = 408;
            MaxLevel = 5;
            GumpHeight = 95;
            AddEndY = 85;
            MaxLevel = 5;
        }


        public override bool HasSkillRequirement(Mobile mobile)
        {
            return Level switch
            {
                0 => mobile.Skills.Forensics.Base >= 40.0 && mobile.Skills.DetectHidden.Base >= 40.0,
                1 => mobile.Skills.Forensics.Base >= 50.0 && mobile.Skills.DetectHidden.Base >= 50.0,
                2 => mobile.Skills.Forensics.Base >= 60.0 && mobile.Skills.DetectHidden.Base >= 60.0,
                3 => mobile.Skills.Forensics.Base >= 70.0 && mobile.Skills.DetectHidden.Base >= 70.0,
                4 => mobile.Skills.Forensics.Base >= 80.0 && mobile.Skills.DetectHidden.Base >= 80.0,
                5 => mobile.Skills.Forensics.Base >= 90.0 && mobile.Skills.DetectHidden.Base >= 90.0,
                _ => false
            };
        }

        public static CaseNote GetPlayerCaseNote(Mobile from)
        {
            if (from.Backpack != null)
            {
                var caseNotes = from.Backpack.FindItemsByType<CaseNote>();
                if (caseNotes.Count > 1)
                {
                    from.SendMessage("You can only work on one case at a time.");
                }
                else if (caseNotes.Any())
                {
                    return caseNotes.First();
                }
            }

            return null;
        }

        public static bool CheckClue(Mobile from)
        {
            var note = GetPlayerCaseNote(from);
            if (note != null && (from.Map == Map.Trammel || from.Map == Map.Felucca))
            {
                foreach (var clue in note.Clues)
                {
                    int skillRange = (int) from.Skills.DetectHidden.Value / 20;
                    if (from.InRange(clue.Location, skillRange) && from.CheckSkill(
                            SkillName.DetectHidden,
                            clue.Difficulty - Utility.Random(10),
                            clue.Difficulty
                        ))
                    {
                        clue.CreateClueItem();
                        from.PublicOverheadMessage(
                            MessageType.Regular,
                            0x3B2,
                            false,
                            "* You find a clue! *"
                        );
                        clue.Solved = false;
                        from.Backpack?.DropItem(clue.Item);
                        Effects.PlaySound(from.Location, from.Map, 0x244);
                        return true;
                    }
                }
            }
            else
            {
                from.PublicOverheadMessage(
                    MessageType.Regular,
                    0x3B2,
                    false,
                    "* You do not have a case *"
                );
            }

            return false;
        }

        public static void CheckSolve(Mobile from, Item item)
        {
            var note = GetPlayerCaseNote(from);
            var itemClue = note?.Clues.Find(c => c.Item.Serial == item.Serial);
            if (itemClue is { Solved: false })
            {
                itemClue.Solved = from.CheckSkill(
                    SkillName.Forensics,
                    itemClue.Difficulty - Utility.Random(10),
                    itemClue.Difficulty
                );
                var message = "* You fail to find anything on this item *";
                if (itemClue.Solved)
                {
                    message = "* You have solved the clue! *";
                    Effects.PlaySound(from.Location, from.Map, 0x245);
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
        }

        public bool GiveRewards(PlayerMobile from, CaseNote caseNote)
        {
            var solved = caseNote.Clues.Where(c => c.Solved).ToList();
            if (from.Backpack != null && solved.Count > 0)
            {
                var goldAmount = Utility.Random(50) * solved.Count * Level;
                from.NonCraftExperience += Utility.Random(75 * Level);
                from.Backpack.DropItem(new Gold(goldAmount));
                Item item = null;
                LootPack loot = null;
                TreasureMap map = null;
                ProcessItemReward(
                    0.5 * Utility.Random(solved.Count + Level),
                    1.5 * Utility.Random(solved.Count + Level),
                    0.3 * Utility.Random(solved.Count + Level),
                    ref item,
                    ref loot,
                    ref map,
                    from
                );
                if (item != null)
                {
                    from.Backpack.AddItem(item);
                }

                loot?.ForceGenerate(from, from.Backpack, loot.RandomEntry(), 1);

                if (map != null)
                {
                    from.Backpack.AddItem(map);
                }

                if (Utility.Random(10000) < 1)
                {
                    from.Backpack.AddItem(new DetectiveBoots());
                }
                else if (Utility.Random(10000) < 1)
                {
                    from.Backpack.AddItem(new LieutenantOfTheBritannianRoyalGuard());
                }
                else if (Utility.Random(1000) < 1)
                {
                    from.Backpack.AddItem(new SamaritanRobe());
                }

                foreach (var clue in solved)
                {
                    clue.Item.Delete();
                }

                caseNote.Delete();
                return true;
            }

            return false;
        }

        public static void ProcessItemReward(
            double packChance, double weaponChance, double mapChance,
            ref Item item, ref LootPack loot, ref TreasureMap map, Mobile from
        )
        {
            if (Utility.Random(100) < packChance)
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

        public static void GiveCaseNote(Mobile from)
        {
            var caseNote = new CaseNote();
            if (from.Backpack != null)
            {
                from.Backpack.DropItem(caseNote);
                from.PublicOverheadMessage(
                    MessageType.Regular,
                    0x3B2,
                    false,
                    "* The guard gives you an authority document concerning a case *"
                );
                Effects.PlaySound(from.Location, from.Map, 0x243);
            }
        }
    }
}

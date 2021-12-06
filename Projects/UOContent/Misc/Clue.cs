using System;
using System.Collections.Generic;
using Server.Items;
using Server.Misc;

namespace Server
{
    public class Clue
    {
        public Point2D Location { get; set; }

        public Item Item { get; set; }

        public bool Solved { get; set; }

        public string Trait { get; set; }

        public string Role { get; set; }

        public string Organisation { get; set; }

        public string Profession { get; set; }

        public int Difficulty { get; set; }

        public string Detail { get; set; }

        public void CreateClueItem()
        {
            List<Type> items = new List<Type> {
                typeof(Scissors), typeof(CheeseWheel), typeof(Hammer), typeof(Shovel), typeof(Wool), typeof(SmithHammer), typeof(Dagger), typeof(CandleSkull), typeof(SewingKit),
                typeof(Backgammon), typeof(CheckerBoard), typeof(Chessboard), typeof(Dices), typeof(WizardsHat), typeof(LongPants), typeof(ShortPants), typeof(Shirt),
                typeof(Torch), typeof(Shoes), typeof(Sandals), typeof(Cloak), typeof(LeftLeg), typeof(Head), typeof(RightArm), typeof(RightLeg), typeof(LeftArm),
                typeof(Torso), typeof(BonePile), typeof(Bone), typeof(HarmWand), typeof(Diamond), typeof(Sapphire), typeof(GoldBracelet), typeof(SilverBracelet), typeof(GoldEarrings),
                typeof(SilverEarrings), typeof(SilverNecklace), typeof(GoldNecklace), typeof(GoldRing), typeof(SilverRing), typeof(FishingPole), typeof(BagOfAllReagents), typeof(BagOfingots), typeof(BagOfNecromancerReagents),
                typeof(Bandage), typeof(Bandana), typeof(Nightshade), typeof(Arrow), typeof(Bedroll), typeof(Hatchet), typeof(Spellbook), typeof(NecromancerSpellbook), typeof(Lute),
                typeof(Harp), typeof(Lockpick), typeof(Clock), typeof(Gears), typeof(Spyglass), typeof(RollingPin), typeof(Froe), typeof(SmoothingPlane), typeof(ClumsyWand),
                typeof(Longsword), typeof(Halberd), typeof(VikingSword), typeof(Katana), typeof(Bardiche), typeof(Bow), typeof(Crossbow), typeof(HeavyCrossbow), typeof(Mace)
            };
            int index = Utility.Random(items.Count);
            Item = Activator.CreateInstance(items[index]) as Item;
            string itemName = SocketBonus.GetItemName(Item);
            if (Item != null)
            {
                Item.Name = $"{itemName} - (clue)";
            }
        }

        public Clue(bool automate = true) {
            if (automate) {
                Trait = HauntedHook.RandomHook("trait");
                Role = HauntedHook.RandomHook("role");
                Organisation = HauntedHook.RandomHook("organisation");
                Profession = HauntedHook.RandomHook("profession");
                Difficulty = Utility.RandomMinMax(50, 100);
                Detail = HauntedHook.Parse(HauntedHook.RandomHook("item_clue"));
                Solved = false;
            }
        }
    }
}

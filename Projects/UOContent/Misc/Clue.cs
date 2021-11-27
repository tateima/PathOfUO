using System;
using System.Collections.Generic;
using Server.Items;

namespace Server
{
    public class Clue
    {
        private Point2D m_Location;
        public Point2D Location {
            get { return m_Location; } 
            set { m_Location = value; }
        }

        private Item m_Item;
        public Item Item {
            get { return m_Item; } 
            set { m_Item = value; }
        }
        
        private bool m_Solved;
        public bool Solved {
            get { return m_Solved; }
            set { m_Solved = value; }
        }

        private string m_Trait;
        public string Trait {
            get { return m_Trait; }
            set { m_Trait = value; }
        }

        private string m_Role;
        public string Role {
            get { return m_Role; }
            set { m_Role = value; }
        }

        private string m_Organisation;
        public string Organisation {
            get { return m_Organisation; }
            set { m_Organisation = value; }
        }

        private string m_Profession;
        public string Profession {
            get { return m_Profession; }
            set { m_Profession = value; }
        }

        private int m_Difficulty;
        public int Difficulty {
            get { return m_Difficulty; }
            set { m_Difficulty = value; }
        }

        private string m_Detail;
        public string Detail {
            get { return m_Detail; }
            set { m_Detail = value; }
        }

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
            m_Item = Activator.CreateInstance(items[index]) as Item;
            m_Item.Name += " (clue)";
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

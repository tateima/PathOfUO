using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Talent
{
    public class GraveDigger : BaseTalent, ITalent
    {
        public GraveDigger() : base()
        {
            DisplayName = "Grave digger";
            Description = "Allows you to dig up items from graveyards as well as other things! Each level improves rewards. Requires 70+ mining.";
            ImageID = 402;
            GumpHeight = 85;
            AddEndY = 105;
        }
        public override bool HasSkillRequirement(Mobile mobile) {
            return mobile.Skills.Mining.Base >= 70;
        }
        public void Dig(Item tool, Mobile mobile, Point3D point) {
            if (Utility.Random(100) < 10) { 
                // spawn undead
                BaseCreature undead = null;
                switch (Utility.RandomMinMax(1, 7 + Level))
                {
                    case 1:
                        undead = new Shade();
                        break;
                    case 2:
                        undead = new Spectre();
                        break;
                    case 3:
                        undead = new Wraith();
                        break;
                    case 4:
                        undead = new Skeleton();
                        break;
                    case 5:
                        undead = new Zombie();
                        break;
                    case 6:
                        undead = new Ghoul();
                        break;
                    case 7:
                        undead = new RottingCorpse();
                        break;
                    case 8:
                        undead = new SkeletalKnight();
                        break;
                    case 9:
                        undead = new BoneKnight();
                        break;
                    case 10:
                        undead = new BoneMagi();
                        break;
                    case 11:
                        undead = new SkeletalMage();
                        break;  
                    case 12:
                        undead = new Lich();
                       break;
                }
                if (undead != null)
                {
                    Effects.PlaySound(mobile.Location, mobile.Map, 0x1FB);
                    Effects.SendLocationParticles(
                        EffectItem.Create(undead.Location, undead.Map, EffectItem.DefaultDuration),
                        0x37CC,
                        1,
                        40,
                        97,
                        3,
                        9917,
                        0
                    );
                    undead.MoveToWorld(point, mobile.Map);
                }
            } else if (Utility.Random(100) < Level) {
                if (Utility.RandomBool()) {
                    Brigand brigand = new Brigand();
                    brigand.MoveToWorld(point, mobile.Map);
                    mobile.SendMessage("You dig up a murderer who was buried alive.");
                } else {
                    if (mobile.Backpack != null) {
                        switch (Utility.RandomMinMax(1, 12)) {
                            case 1:
                                var gem = Loot.RandomGem();
                                mobile.Backpack.AddItem(gem);
                            break;
                            case 2:
                                var gold = new Gold(Utility.Random(Level));
                                mobile.Backpack.AddItem(gold);
                            break;
                            case 3:
                                var jewelry = Loot.RandomJewelry();
                                mobile.Backpack.AddItem(jewelry);
                            break;
                            case 4:
                                var book = Loot.RandomLibraryBook();
                                mobile.Backpack.AddItem(book);
                            break;
                            case 5:
                                var reagent = Loot.RandomNecromancyReagent();
                                mobile.Backpack.AddItem(reagent);
                            break;
                            case 6:
                                var potion = Loot.RandomPotion();
                                mobile.Backpack.AddItem(potion);
                            break;
                            case 7:
                                var scroll = Loot.RandomScroll(0, 15, SpellbookType.Necromancer);
                                if (Utility.RandomBool()) {
                                    scroll = Loot.RandomScroll(0, 63, SpellbookType.Regular);
                                }
                                mobile.Backpack.AddItem(scroll);
                            break;
                            case 8:
                                var weapon = Loot.RandomWeapon();
                                mobile.Backpack.AddItem(weapon);
                            break;
                            case 9:
                                var wand = Loot.RandomWand();
                                mobile.Backpack.AddItem(wand);
                            break;
                            case 10:
                                var clothing = Loot.RandomClothing();
                                mobile.Backpack.AddItem(clothing);
                            break;
                            case 11:
                                var hat = Loot.RandomHat();
                                mobile.Backpack.AddItem(hat);
                            break;
                            case 12:
                                var shield = Loot.RandomShield();
                                mobile.Backpack.AddItem(shield);
                            break;
                        }
                        mobile.SendMessage("You dig up an item from the grave.");
                    }
                }
            } else {
                mobile.SendMessage("You fail to dig up anything useful.");
            }
            tool.Consume();
        }
    }
}

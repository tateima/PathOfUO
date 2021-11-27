using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Server.Items;
using Server.Talent;
using Server.Network;

namespace Server
{
    public class SocketBonus
    {
        public static AosAttributes AddAosAttribute(AosAttribute attribute, AosAttributes parentAttributes, int attributeBonus) {
            switch(attribute) {
                case AosAttribute.AttackChance:
                    parentAttributes.AttackChance += attributeBonus;
                break;
                case AosAttribute.BonusDex:
                    parentAttributes.BonusDex += attributeBonus;
                break;
                case AosAttribute.BonusHits:
                    parentAttributes.BonusHits += attributeBonus;
                break;
                case AosAttribute.BonusInt:
                    parentAttributes.BonusInt += attributeBonus;
                break;
                case AosAttribute.BonusMana:
                    parentAttributes.BonusMana += attributeBonus;
                break;
                case AosAttribute.BonusStam:
                    parentAttributes.BonusStam += attributeBonus;
                break;
                case AosAttribute.BonusStr:
                    parentAttributes.BonusStr += attributeBonus;
                break;
                case AosAttribute.CastRecovery:
                    parentAttributes.CastRecovery += attributeBonus;
                break;
                case AosAttribute.CastSpeed:
                    parentAttributes.CastSpeed += attributeBonus;
                break;
                case AosAttribute.DefendChance:
                    parentAttributes.DefendChance += attributeBonus;
                break;
                case AosAttribute.EnhancePotions:
                    parentAttributes.EnhancePotions += attributeBonus;
                break;
            }
            return parentAttributes;
        }
        public static void AddItem(Mobile from, Item item, Item socketItem) {
            string overheadMessage = "";
            Item runeWordItem = null;
            string itemName = socketItem.Name;
            if (string.IsNullOrEmpty(itemName)) {
                itemName = socketItem.ItemData.Name;
            }
            itemName = Regex.Replace(itemName, @"[^0-9a-zA-Z\._]", "");
            if (item is BaseWeapon weapon && weapon.SocketAmount > 0 && weapon.Sockets.Count < weapon.SocketAmount) { // only crafted items
                weapon.Sockets.Add(socketItem);
                if (socketItem is RuneWord) {
                    runeWordItem = SocketBonus.GetRuneWordWeapon(weapon);
                }
                overheadMessage = string.Format("* You add {0} to the weapon *", itemName);
            } else if (item is BaseArmor armor && armor.SocketAmount > 0 && armor.Sockets.Count < armor.SocketAmount) { 
                armor.Sockets.Add(socketItem);
                if (socketItem is RuneWord) {
                    runeWordItem = SocketBonus.GetRuneWordArmor(armor);
                }
                overheadMessage = string.Format("* You add {0} to the armor *", itemName);
            } else if (item is BaseJewel jewellery && jewellery.SocketAmount > 0 && jewellery.Sockets.Count < jewellery.SocketAmount) { 
                jewellery.Sockets.Add(socketItem);
                if (socketItem is RuneWord) {
                    runeWordItem = SocketBonus.GetRuneWordJewellery(jewellery);
                }
                overheadMessage = string.Format("* You add {0} to the armor *", itemName);
            } else if (item is BaseShield shield && shield.SocketAmount > 0 && shield.Sockets.Count < shield.SocketAmount) { 
                shield.Sockets.Add(socketItem);
                if (socketItem is RuneWord) {
                    runeWordItem = SocketBonus.GetRuneWordShield(shield);
                }
                overheadMessage = string.Format("* You add {0} to the armor *", itemName);
            } else if (item is BaseClothing clothing && clothing.PocketAmount > 0 && clothing.Pockets.Count < clothing.PocketAmount 
                    && (clothing is BaseHat || clothing is BaseWaist || clothing is BaseOuterTorso)) {
                clothing.Pockets.Add(socketItem);
                if (socketItem is RuneWord) {
                    runeWordItem = SocketBonus.GetRuneWordClothing(clothing);
                }
                overheadMessage = string.Format("* You add {0} to the clothing *", itemName);
            } else {
                from.SendMessage(string.Format("You cannot place {0} into this item", itemName));
            }
            if (runeWordItem != null) {
                item.Delete();
                from.Backpack.DropItem(runeWordItem);
                from.SendSound(0x1F7);
                overheadMessage = string.Format("* Your runewords create a powerful item *", runeWordItem.Name);
            }
            if (!string.IsNullOrEmpty(overheadMessage)) {
                item.InvalidateProperties();
                socketItem.Delete();
                from.PublicOverheadMessage(
                    MessageType.Regular,
                    0x3B2,
                    false,
                    overheadMessage
                );
            }
        }

        public static void AddSocketProperties(Item socket, Item parent, ObjectPropertyList list) {
            List<ResistanceMod> resistanceMods = GetSocketItemResistanceBonus(socket);
            foreach(ResistanceMod mod in resistanceMods) {
                if (mod.Type == ResistanceType.Cold) {
                    list.Add(1060445, mod.Offset.ToString());
                } else if (mod.Type == ResistanceType.Energy) {
                    list.Add(1060446, mod.Offset.ToString());
                } else if (mod.Type == ResistanceType.Fire) {
                    list.Add(1060447, mod.Offset.ToString());
                } else if (mod.Type == ResistanceType.Poison) {
                    list.Add(1060449, mod.Offset.ToString()); 
                } else if (mod.Type == ResistanceType.Physical) {
                    list.Add(1060448, mod.Offset.ToString()); 
                }
            }
            List<AosAttribute> attributes = GetSocketItemAttributeBonus(socket, parent);
            string attributeBonus = "5";
            int labelNumber = 0;
            foreach(AosAttribute attribute in attributes) {
                labelNumber = 0;
                if (attribute == AosAttribute.WeaponSpeed) {
                    labelNumber = 1060486; // swing speed increase ~1_val~%
                } else if (attribute == AosAttribute.BonusStr) {
                    labelNumber = 1060485; // strength bonus ~1_val~                     
                } else if (attribute == AosAttribute.RegenMana) {
                    labelNumber = 1060440; // mana regeneration ~1_val~
                } else if (attribute == AosAttribute.SpellDamage) {
                    labelNumber = 1060483; // spell damage increase ~1_val~%
                } else if (attribute == AosAttribute.SpellChanneling) {
                    labelNumber = 1060482;
                } else if (attribute == AosAttribute.RegenHits) {
                    labelNumber = 1060444; // hit point regeneration ~1_val~
                } else if (attribute == AosAttribute.LowerRegCost) {
                    labelNumber = 1060484; // stamina increase ~1_val~
                } else if (attribute == AosAttribute.BonusStam) {
                    labelNumber = 1060484; // stamina increase ~1_val~
                } else if (attribute == AosAttribute.NightSight) {
                   labelNumber = 1060441;
                } else if (attribute == AosAttribute.BonusMana) {
                    labelNumber = 1060439; // mana increase ~1_val~
                } else if (attribute == AosAttribute.Luck) {
                    labelNumber = 1060436; // luck ~1_val~
                } else if (attribute == AosAttribute.LowerManaCost) {
                    labelNumber = 1060433; // lower mana cost ~1_val~%
                } else if (attribute == AosAttribute.LowerRegCost) {
                    labelNumber = 1060434; // lower reagent cost ~1_val~%
                } else if (attribute == AosAttribute.BonusInt) {
                    labelNumber = 1060432; // intelligence bonus ~1_val~
                } else if (attribute == AosAttribute.BonusHits) {
                    labelNumber = 1060435;
                } else if (attribute == AosAttribute.AttackChance) {
                    labelNumber = 1060415; // hit chance increase ~1_val~%
                } else if (attribute == AosAttribute.CastSpeed) {
                    labelNumber = 1060413; // faster casting ~1_val~
                } else if (attribute == AosAttribute.CastRecovery) {
                    labelNumber = 1060412; // faster cast recovery ~1_val~
                } else if (attribute == AosAttribute.EnhancePotions) {
                    labelNumber = 1060411; // enhance potions ~1_val~%
                } else if (attribute == AosAttribute.BonusDex) {
                    labelNumber = 1060409; // dexterity bonus ~1_val~
                } else if (attribute == AosAttribute.DefendChance) {
                    labelNumber = 1060408; // defense chance increase ~1_val~%
                } else if (attribute == AosAttribute.WeaponDamage) {
                    labelNumber = 1060401; // damage increase ~1_val~%
                } else if (attribute == AosAttribute.BonusDex) {
                    labelNumber = 1060409; // dexterity bonus ~1_val~
                } else if (attribute == AosAttribute.ReflectPhysical) {
                    labelNumber = 1060442; // reflect physical damage ~1_val~%s
                } else if (attribute == AosAttribute.IncreasedKarmaLoss) {
                    labelNumber = 1075210; // Increased Karma Loss ~1val~%
                }
                if (labelNumber > 0) {
                    list.Add(labelNumber, attributeBonus);
                }
            }
            if (parent is BaseWeapon) {
                List<AosWeaponAttribute> weaponAttributes = GetSocketItemWeaponAttributeBonus(socket);
                foreach(AosWeaponAttribute weaponAttribute in weaponAttributes) {
                    labelNumber = 0;
                    switch(weaponAttribute) {
                        case AosWeaponAttribute.HitColdArea:
                            labelNumber = 1060416;
                        break;
                        case AosWeaponAttribute.HitLeechHits:
                            labelNumber = 1060422;
                        break;
                        case AosWeaponAttribute.HitEnergyArea:
                            labelNumber = 1060418;
                        break;
                        case AosWeaponAttribute.HitFireArea:
                            labelNumber = 1060419;
                        break;
                        case AosWeaponAttribute.HitLeechMana:
                            labelNumber = 1060427;
                        break;
                        case AosWeaponAttribute.HitLightning:
                            labelNumber = 1060423;
                        break;
                        case AosWeaponAttribute.HitLowerDefend:
                            labelNumber = 1060425;
                        break;
                    }
                    if (labelNumber > 0) {
                        list.Add(labelNumber, attributeBonus);
                    }
                }
                List<AosElementAttribute> elementAttributes = GetSocketItemElementAttributeBonus(socket);
                foreach(AosElementAttribute elementAttribute in elementAttributes) {
                    labelNumber = 0;
                    switch(elementAttribute) {
                        case AosElementAttribute.Chaos: 
                            labelNumber = 1072846;
                        break;
                        case AosElementAttribute.Energy:
                            labelNumber = 1060407;
                        break;
                        case AosElementAttribute.Cold:
                            labelNumber = 1060404;
                        break;
                        case AosElementAttribute.Fire:
                            labelNumber = 1060405;
                        break;
                    }
                    if (labelNumber > 0) {
                        list.Add(labelNumber, attributeBonus);
                    }
                }
            } else if (parent is BaseArmor) {
                List<AosArmorAttribute> armorAttributes = GetSocketArmorAttributeBonus(socket);
                foreach(AosArmorAttribute armorAttribute in armorAttributes) {
                    labelNumber = 0;
                    switch(armorAttribute) {
                        case AosArmorAttribute.LowerStatReq:
                            labelNumber = 1060435;
                        break;
                        case AosArmorAttribute.MageArmor:
                            labelNumber = 1060437;
                        break;
                        case AosArmorAttribute.SelfRepair:
                            labelNumber = 1060450;
                        break;
                    }
                    if (labelNumber > 0) {
                        list.Add(labelNumber, attributeBonus);
                    }
                }
            }
            
        }

        public static void CheckSockets(Mobile from, bool remove, List<Item> sockets, Item parent) {
            if (sockets != null && sockets.Count > 0) {
                List<ResistanceMod> socketMods = new List<ResistanceMod>();
                List<AosElementAttribute> elements = new List<AosElementAttribute>();
                List<AosWeaponAttribute> weaponAttributes = new List<AosWeaponAttribute>();
                List<AosAttribute> attributes = new List<AosAttribute>();
                int attributeBonus = remove ? -5 : +5;
                foreach(Item item in sockets) {
                    socketMods.AddRange(GetSocketItemResistanceBonus(item));
                    if (parent is BaseWeapon) {
                        elements.AddRange(GetSocketItemElementAttributeBonus(item));
                        weaponAttributes.AddRange(GetSocketItemWeaponAttributeBonus(item));
                    }
                    attributes.AddRange(GetSocketItemAttributeBonus(item, parent));
                }
                if (parent is BaseWeapon weapon) {
                    AosElementAttributes parentElementDamages = weapon.AosElementDamages;
                    foreach(AosElementAttribute elementAttribute in elements) {
                        if (elementAttribute == AosElementAttribute.Chaos) {
                            parentElementDamages.Chaos += attributeBonus;
                        } else if (elementAttribute == AosElementAttribute.Cold) {
                            parentElementDamages.Cold += attributeBonus;
                        } else if (elementAttribute == AosElementAttribute.Energy) {
                            parentElementDamages.Energy += attributeBonus;
                        } else if (elementAttribute == AosElementAttribute.Fire) {
                            parentElementDamages.Fire += attributeBonus;
                        } 
                    }
                    weapon.AosElementDamages = parentElementDamages;
                    AosWeaponAttributes parentWeaponAttributes = weapon.WeaponAttributes;
                    foreach(AosWeaponAttribute attribute in weaponAttributes) {
                        switch(attribute) {
                            case AosWeaponAttribute.HitColdArea:
                                parentWeaponAttributes.HitColdArea += attributeBonus;
                            break;
                            case AosWeaponAttribute.HitLeechHits:
                                parentWeaponAttributes.HitLeechHits += attributeBonus;
                            break;
                            case AosWeaponAttribute.HitEnergyArea:
                                parentWeaponAttributes.HitEnergyArea += attributeBonus;
                            break;
                            case AosWeaponAttribute.HitFireArea:
                                parentWeaponAttributes.HitFireArea += attributeBonus;
                            break;
                            case AosWeaponAttribute.HitLeechMana:
                                parentWeaponAttributes.HitLeechMana += attributeBonus;
                            break;
                            case AosWeaponAttribute.HitLightning:
                                parentWeaponAttributes.HitLightning += attributeBonus;
                            break;
                            case AosWeaponAttribute.HitLowerDefend:
                                parentWeaponAttributes.HitLowerDefend += attributeBonus;
                            break;
                        }
                    }
                    weapon.WeaponAttributes = parentWeaponAttributes;

                    AosAttributes parentAttributes = weapon.Attributes;
                    foreach(AosAttribute attribute in attributes) {
                        parentAttributes = AddAosAttribute(attribute, parentAttributes, attributeBonus);
                    }
                    weapon.Attributes = parentAttributes;
                }
                if (parent is BaseArmor armor) {
                    AosAttributes parentAttributes = armor.Attributes;
                    foreach(AosAttribute attribute in attributes) {
                        parentAttributes = AddAosAttribute(attribute, parentAttributes, attributeBonus);
                    }
                    armor.Attributes = parentAttributes;
                }
                
                foreach(ResistanceMod mod in socketMods) {
                    if (remove) {
                        from.RemoveResistanceMod(mod);
                    } else {
                        from.AddResistanceMod(mod);
                    }
                }
            }
        }

        public static string GetRuneWords(List<Item> sockets) {
            string runeWords = "";
            foreach(Item item in sockets) {
                if (item is RuneWord) {
                    runeWords += item.Name;
                }
            }
            return runeWords;
        }

        public static BaseClothing GetRuneWordClothing(BaseClothing clothing) {
            List<Item> pockets = clothing.Pockets;
            string runeWords = GetRuneWords(pockets);
            if (!string.IsNullOrEmpty(runeWords)) {
                switch(runeWords) {
                    case "ZetAmnMar":
                        if (clothing is HalfApron) {
                            return new CrimsonCincture();
                        }
                    break;
                    case "LuxAmnMarAoe":
                        if (clothing is Robe) {
                            return new RobeOfTheEclipse();
                        }
                    break;
                    case "CurOrtZetMar":
                        if (clothing is Robe) {
                            return new RobeOfTheEquinox();
                        }
                    break;
                    case "MarNexLeqMar":
                        if (clothing is BearMask) {
                            return new SpiritOfTheTotem();
                        }
                    break;
                    case "NexMarUmDrux":
                        if (clothing is DeerMask) {
                            return new HuntersHeaddress();
                        }
                    break;
                    case "DruxMarPaxZaq":
                        if (clothing is WizardsHat) {
                            return new HatOfTheMagi();
                        }
                    break;
                    case "PaxZaqOrtZet":
                        if (clothing is HornedTribalMask) {
                            return new DivineCountenance();
                        }
                    break;
                    case "AlozAeoLux":
                        if (clothing is Kasa) {
                        return new AncientFarmersKasa();
                        }
                    break;
                    case "LuxZetOrt":
                        if (clothing is JesterHat) {
                            return new JesterHatofChuckles();
                        }
                    break;
                    case "AeoMeaAoe":
                        if (clothing is ClothNinjaHood) {
                            return new BlackLotusHood();
                        }
                    break;
                    case "MeaAlozOrtKres":
                        if (clothing is Kasa) {
                            return new KasaOfTheRajin();
                        }
                    break;
                    case "ZaOrtZeAloz":
                        if (clothing is Bandana) {
                            return new BurglarsBandana();
                        }
                    break;
                    case "OrtVexMar":
                        if (clothing is BearMask) {
                            return new PolarBearMask();
                        }
                    break;
                    case "KresZeAlozOrt":
                        if (clothing is TricorneHat) {
                            return new DreadPirateHat();
                        }
                    break;
                    case "ChaZoAmnZoat":
                        if (clothing is TricorneHat) {
                            return new CaptainJohnsHat();
                        }
                    break;
                    case "ChaAmnOrtZet":
                        if (clothing is Cloak) {
                            return new EmbroideredOakLeafCloak();
                        }
                    break;
                    case "ChaOrtAlozMea":
                        if (clothing is Bandana) {
                            return new CrownOfTalKeesh();
                        }
                    break;
                }
            }
            return null;
        }

        public static BaseJewel GetRuneWordJewellery(BaseJewel jewellery) {
            List<Item> sockets = jewellery.Sockets;
            string runeWords = GetRuneWords(sockets);
            if (!string.IsNullOrEmpty(runeWords)) {
                switch(runeWords) {
                    case "AmnMarLeqDruxZaq":
                        if (jewellery is GoldBracelet) {
                            return new BraceletOfHealth();
                        }
                    break;
                    case "NexAmnVasUmZet":
                        if (jewellery is GoldBracelet) {
                            return new OrnamentOfTheMagician();
                        }
                    break;
                    case "VasZetUmZetOrt":
                        if (jewellery is GoldRing) {
                            return new RingOfTheElements();
                        }
                    break;
                    case "OrtMarLeqAmnOrt":
                        if (jewellery is GoldRing) {
                            return new RingOfTheVile();
                        }
                    break;
                    case "OrtVasDruxCurMox":
                        if (jewellery is GoldBracelet) {
                            return new AlchemistsBauble();
                        }
                    break;
                    case "EspCurAmnOrt":
                        if (jewellery is SilverRing) {
                            return new DjinnisRing();
                        }
                    break;
                }
            }
            return null;
        }
        public static BaseShield GetRuneWordShield(BaseShield shield) {
            List<Item> sockets = shield.Sockets;
            string runeWords = GetRuneWords(sockets);
            if (!string.IsNullOrEmpty(runeWords)) {
                switch(runeWords) {
                    case "ZetAmnMarLeq":
                        if (shield is WoodenKiteShield) {
                            return new ArcaneShield();
                        }
                    break;
                    case "PaxZaqMarOrtNex":
                        if (shield is HeaterShield) {
                            return new Aegis();
                        }
                    break;
                    case "ZaqMarOrt":
                        if (shield is BaseShield) {
                            return new DupresShield();
                        }
                    break;
                    case "ZeZaKazMoxVex":
                        if (shield is MetalShield) {
                            return new ShieldOfInvulnerability();
                        }
                    break;
                }
            }
            return null;
        }
        
        public static BaseArmor GetRuneWordArmor(BaseArmor armor) {
            List<Item> sockets = armor.Sockets;
            string runeWords = GetRuneWords(sockets);
            if (!string.IsNullOrEmpty(runeWords)) {
                switch(runeWords) {
                    case "DruxOrtHemZetVax":
                        if (armor is StuddedChest) {
                            return new ArmorOfFortune();
                        }
                    break;
                    case "NexDruxVasHemZaq":
                        if (armor is RingmailGloves) {
                            return new GauntletsOfNobility();
                        }
                    break;
                    case "MarNexMarAmnVas":
                        if (armor is PlateHelm) {
                            return new HelmOfInsight();
                        }
                    break;
                    case "DruxZaqUmOrtUm":
                        if (armor is PlateChest) {
                            return new HolyKnightsBreastplate();
                        }
                    break;
                    case "HemOrtMarVasNex":
                        if (armor is PlateGloves) {
                            return new InquisitorsResolution();
                        }
                    break;
                    case "DothZetAmnLeqZaq":
                        if (armor is PlateGorget) {
                            return new JackalsCollar();
                        }
                    break;
                    case "OrtZetMarDruxAmn":
                        if (armor is ChainLegs) {
                            return new LeggingsOfBane();
                        }
                    break;
                    case "MarAmnNexAmnMar":
                        if (armor is BoneArms) {
                            return new MidnightBracers();
                        }
                    break;
                    case "ZetVaxLeqDruxOrt":
                        if (armor is BoneHelm) {
                            return new OrnateCrownOfTheHarrower();
                        }
                    break;
                    case "PaxZetUmLeqDoth":
                        if (armor is LeatherLegs) {
                            return new ShadowDancerLeggings();
                        }
                    break;
                    case "DruxPaxNexOrtNex":
                        if (armor is ChainChest) {
                            return new TunicOfFire();
                        }
                    break;
                    case "UmDruxZaqHemZet":
                        if (armor is LeatherGorget) {
                            return new VoiceOfTheFallenKing();
                        }
                    break;
                    case "MeaZoatKres":
                        if (armor is PlateDo) {
                            return new AncientSamuraiDo();
                        }
                    break;                    
                    case "ZoatKresOrt":
                        if (armor is LeatherHiroSode) {
                            return new ArmsOfTacticalExcellence();
                        }
                    break;
                    case "HurLuxAloz":
                        if (armor is PlateBattleKabuto) {
                            return new DaimyosHelm();
                        }
                    break;
                    case "MeaZoatKresAloz":
                        if (armor is PlateDo) {
                            return new AncientSamuraiDo();
                        }
                    break;
                    case "AmnDothHurAloz":
                        if (armor is LeatherNinjaMitts) {
                            return new GlovesOfTheSun();
                        }
                    break;
                    case "HurZaqMarAmn":
                        if (armor is PlateSuneate) {
                            return new LegsOfStability();
                        }
                    break;
                    case "ZaqAmnOrtKresLux":
                        if (armor is LeatherMempo) {
                            return new LeurociansMempoOfFortune();
                        }
                    break;
                    case "AmnZoatOrtDruxLux":
                        if (armor is PlateDo) {
                            return new RuneBeetleCarapace();
                        }
                    break;
                    case "HurAlozOrtVasMea":
                        if (armor is LeatherNinjaMitts) {
                            return new Stormgrip();
                        }
                    break;
                    case "EspMoxCur":
                        if (armor is DragonHelm) {
                            return new AegisOfGrace();
                        }
                    break;
                    case "ZoatOrtCur":
                        if (armor is ElvenGlasses) {
                            return new BrightsightLenses();
                        }
                    break;
                    case "ZoatZaOrtKazDem":
                        if (armor is ElvenGlasses) {
                            return new MaritimeGlasses();
                        }
                    break;
                    case "VexAmnVaxKazDem":
                        if (armor is ElvenGlasses) {
                            return new WizardsGlasses();
                        }
                    break;
                    case "KazVaxZaDem":
                        if (armor is ElvenGlasses) {
                            return new TradeGlasses();
                        }
                    break;
                    case "KazOrtDemDum":
                        if (armor is ElvenGlasses) {
                            return new LyricalGlasses();
                        }
                    break;
                    case "DumVoxAlozVexPax":
                        if (armor is ElvenGlasses) {
                            return new NecromanticGlasses();
                        }
                    break;
                    case "MeaAeoKresHurDum":
                        if (armor is ElvenGlasses) {
                            return new LightOfWayGlasses();
                        }
                    break;
                    case "EspDruxMirZaLux":
                        if (armor is ElvenGlasses) {
                            return new FoldedSteelGlasses();
                        }
                    break;
                    case "ZoatDruxDem":
                        if (armor is ElvenGlasses) {
                            return new PoisonedGlasses();
                        }
                    break;
                    case "DruxDemKazOrtDum":
                        if (armor is ElvenGlasses) {
                            return new TreasureTrinketGlasses();
                        }
                    break;
                    case "KazOrtDumZaLi":
                        if (armor is ElvenGlasses) {
                            return new MaceShieldGlasses();
                        }
                    break;
                    case "KazAmnVasLi":
                        if (armor is ElvenGlasses) {
                            return new ArtsGlasses();
                        }
                    break;
                    case "DruxHurEspOrt":
                        if (armor is ElvenGlasses) {
                            return new AnthropomorphistGlasses();
                        }
                    break;
                    case "AmnCurOrt":
                        if (armor is ChainLegs) {
                            return new FeyLeggings();
                        }
                    break;
                    case "OrtVasMox":
                        if (armor is WingedHelm) {
                            return new HelmOfSwiftness();
                        }
                    break;
                    case "NexZeDum":
                        if (armor is OrcHelm) {
                            return new OrcishVisage();
                        }
                    break;
                    case "LuxDumKresVexHur":
                        if (armor is FemalePlateChest) {
                            return new VioletCourage();
                        }
                    break;
                    case "KresEspCurAmnMar":
                        if (armor is PlateChest) {
                            return new HeartOfTheLion();
                        }
                    break;
                    case "OrtAmnZeKazMar":
                        if (armor is LeatherGloves) {
                            return new GlovesOfThePugilist();
                        }
                    break;
                    case "LiMirAmnOrt":
                        if (armor is PlateGloves) {
                            return new GuantletsOfAnger();
                        }
                    break;
                    case "MirZoMarLar":
                        if (armor is PlateGorget) {
                            return new GladiatorsCollar();
                        }
                    break;
                    case "VexKazZoAmn":
                        if (armor is OrcHelm) {
                            return new OrcChieftainHelm();
                        }
                    break;
                    case "EspMoxCurZo":
                        if (armor is BoneChest) {
                            return new ShroudOfDeciet();
                        }
                    break;
                    case "LiChaKaxDumKres":
                        if (armor is PlateBattleKabuto) {
                            return new SamuraiHelm();
                        }
                    break;
                    case "DemLiDumKazAmn":
                        if (armor is PlateLegs) {
                            return new LeggingsOfEmbers();
                        }
                    break;
                }
            }
            return null;
        }
        
        public static BaseWeapon GetRuneWordWeapon(BaseWeapon weapon) {
            List<Item> sockets = weapon.Sockets;
            string runeWords = GetRuneWords(sockets);
            if (!string.IsNullOrEmpty(runeWords)) {
                switch(runeWords) {
                    case "NexZaqVasHemPax":
                        if (weapon is MagicalShortbow) {
                            return new Windsong();
                        }
                    break;
                    case "VasCurHurDrux":
                        if (weapon is ElvenCompositeLongbow) {
                            return new WildfireBow();
                        }
                    break;
                    case "MeaMoxVoxVexAmn":
                        if (weapon is RadiantScimitar) {
                            return new SoulSeeker();
                        }
                    break;
                    case "EspCurLuxKresOrt":
                        if (weapon is OrnateAxe) {
                            return new TalonBite();
                        }
                    break;
                    case "AmnDruxDothOrtVax":
                        if (weapon is DoubleAxe) {
                            return new AxeOfTheHeavens();
                        }
                    break;
                    case "OrtVasHemZaqNex":
                        if (weapon is Katana) {
                            return new BladeOfInsanity();
                        }
                    break;
                    case "NexZaqAmnVasOrt":
                        if (weapon is Longsword) {
                            return new BladeOfTheRighteous();    
                        }
                    break;
                    case "UmDruxZaqOrtHem":
                        if (weapon is Longsword) {
                            return new BoneCrusher();
                        }
                    break;
                    case "DruxHemOrtVasZet":
                        if (weapon is BoneHarvester) {
                            return new BreathOfTheDead();
                        }
                    break;
                    case "VasDothZetOrtLeq":
                        if (weapon is Bow) {
                            return new Frostbringer();
                        }
                    break;
                    case "HemOrtMarZetAmn":
                        if (weapon is Bardiche) {
                            return new LegacyOfTheDreadLord();
                        }
                    break;
                    case "ZaqMarAmnNexDrux":
                        if (weapon is Kryss) {
                            return new SerpentsFang();
                        }
                    break;
                    case "VaxZetDothDruxVax":
                        if (weapon is BlackStaff) {
                            return new StaffOfTheMagi();
                        }
                    break;
                    case "DothZetPaxLeqDoth":
                        if (weapon is Maul) {
                            return new TheBeserkersMaul();
                        }                        
                    break;
                    case "ZetDruxNexOrtHem":
                        if (weapon is Lance) {
                            return new TheDragonSlayer();
                        }
                    break;
                    case "MarUmDruxZaqZet":
                        if (weapon is Bow) {
                            return new TheDryadBow();
                        }
                    break;
                    case "LeqAmnVasNexPax":
                        if (weapon is WarFork) {
                            return new TheTaskmaster();
                        }
                    break;
                    case "PaxLeqUmAmnLeq":
                        if (weapon is WarHammer) {
                            return new TitansHammer();
                        }
                    break;
                    case "VaxDothHemOrtVas":
                        if (weapon is ExecutionersAxe) {
                            return new ZyronicClaw();
                        }
                    break;
                     case "AlozHemLeqOrt":
                        if (weapon is Tessen) {
                            return new PilferedDancerFans();
                        }
                    break;
                    case "VaxZoatLuxZet":
                        if (weapon is Sai) {
                            return new DemonForks();
                        }
                    break;
                    case "KresMeaOrtDoth":
                        if (weapon is Nunchaku) {
                            return new DragonNunchaku();
                        }
                    break;
                    case "DothNexZoatUm":
                        if (weapon is Tetsubo) {
                            return new Exiler();
                        }
                    break;
                    case "AeoOrtZetDrux":
                        if (weapon is Yumi) {
                            return new HanzosBow();
                        }
                    break;
                    case "VasDruxAlozZoat":
                        if (weapon is Bokuto) {
                            return new PeasantsBokuto();
                        }
                    break;
                    case "NexPaxZoatAloz":
                        if (weapon is NoDachi) {
                            return new TheDestroyer();
                        }
                    break;
                    case "NexHurZoatHurAloz":
                        if (weapon is Daisho) {
                            return new SwordsOfProsperity();
                        }
                    break;
                    case "UmAlozHurAmnVas":
                        if (weapon is NoDachi) {
                            return new SwordOfTheStampede();
                        }
                    break;
                    case "KresAeoLuzDothVas":
                        if (weapon is Tessen) {
                            return new WindsEdge();
                        }
                    break;
                    case "VasOrtDruxLuxMea":
                        if (weapon is Kama) {
                            return new DarkenedSky();
                        }
                    break;
                    case "AeoVasKresHurOrt":
                        if (weapon is Yumi) {
                            return new TheHorselord();
                        }
                    break;
                    case "VoxVexAlozEsp":
                        if (weapon is RuneBlade) {
                            return new BladeDance();
                        }
                    break;
                    case "CurVexMeaMox":
                        if (weapon is DiamondMace) {
                            return new Bonesmasher();
                        }
                    break;
                    case "VexCurDruxHur":
                        if (weapon is WildStaff) {
                            return new Boomstick();
                        }
                    break;
                    case "VexVasOrtAmnEsp":
                        if (weapon is AssassinSpike) {
                            return new FleshRipper();
                        }
                    break;
                    case "AeoNexUmEspVex":
                        if (weapon is WarCleaver) {
                            return new RaedsGlory();
                        }
                    break;
                    case "KresVaxMoxMeaCur":
                        if (weapon is ElvenMachete) {
                            return new RighteousAnger();
                        }
                    break;
                    case "VexHurLuxNexZaq":
                        if (weapon is WarMace) {
                            return new ArcticDeathDealer();
                        }
                    break;
                    case "KazDemEspZeZa":
                        if (weapon is Halberd) {
                            return new BlazeOfDeath();
                        }
                    break;
                    case "DumOrtMoxZaHur":
                        if (weapon is Bow) {
                            return new BowOfTheJukaKing();
                        }
                    break;
                    case "MoxKazOrtDumZe":
                        if (weapon is Club) {
                            return new CavortingClub();
                        }
                    break;
                    case "KazAmnCurVexPax":
                        if (weapon is ShortSpear) {
                            return new EnchantedTitanLegBone();
                        }
                    break;
                    case "LuxMoxDruxVexVox":
                        if (weapon is Lance) {
                            return new LunaLance();
                        }
                    break;
                    case "VexVoxZetVasOrt":
                        if (weapon is Dagger) {
                            return new NightsKiss();
                        }
                    break;
                    case "AmnOrtLeqAlozZoat":
                        if (weapon is HeavyCrossbow) {
                            return new NoxRangersHeavyCrossbow();
                        }
                    break;
                    case "VexAlozLeqZe":
                        if (weapon is BlackStaff) {
                            return new StaffOfPower();
                        }
                    break;
                    case "VasZeKazAeoHem":
                        if (weapon is GnarledStaff) {
                            return new WrathOfTheDryad();
                        }
                    break;
                    case "AloxVoxMarZetDoth":
                        if (weapon is Scepter) {
                            return new PixieSwatter();
                        }
                    break;
                    case "ZeCurHemDruxDoth":
                        if (weapon is Cutlass) {
                            return new CaptainQuacklebushsCutlass();
                        }
                    break;
                    case "LuxOrtAmnDoth":
                        if (weapon is Cleaver) {
                            return new ColdBlood();
                        }
                    break;
                    case "LarChaZoAmnOrt":
                        if (weapon is Katana) {
                            return new BraveKnightOfTheBritannia();
                        }
                    break;
                    case "LarAmnChaZoPax":
                        if (weapon is Dagger) {
                            return new OblivionsNeedle();
                        }
                    break;
                    case "ChaZaAeoDruxZoat":
                        if (weapon is SkinningKnife) {
                            return new RoyalGuardSurvivalKnife();
                        }
                    break;
                    case "ZeMirZoAmnVas":
                        if (weapon is Halberd) {
                            return new Calm();
                        }
                    break;
                    case "ZoDruxAmnDoth":
                        if (weapon is Kryss) {
                            return new FangOfRactus();
                        }
                    break;
                    case "LeqMarZothZeZa":
                        if (weapon is Pike) {
                            return new Pacify();
                        }
                    break;
                    case "UmOrtChaLiAmn":
                        if (weapon is Bardiche) {
                            return new Quell();
                        }
                    break;
                    case "KazDumChaPaxVax":
                        if (weapon is Scythe) {
                            return new Subdue();
                        }
                    break;
                    case "MoxCurDothLiLar":
                        if (weapon is Longsword) {
                            return new HolySword();
                        }
                    break;
                    case "KazDemDumVexZa":
                        if (weapon is RepeatingCrossbow) {
                            return new ShaminoCrossbow();
                        }
                    break;
                }
            }
            return null;
        }

        public static List<AosElementAttribute> GetSocketItemElementAttributeBonus(Item item) {
            List<AosElementAttribute> types = new List<AosElementAttribute>();
            if (item is RuneWord rune) {
                 switch (rune.Name) {
                    case "Zo":
                        types.Add(AosElementAttribute.Chaos);
                    break;
                    case "Mir":
                        types.Add(AosElementAttribute.Cold);
                    break;
                    case "Li":
                        types.Add(AosElementAttribute.Energy);
                    break;
                    case "Cha":
                        types.Add(AosElementAttribute.Fire);
                    break;
                 }
            }
            return types;
        }

        public static List<AosWeaponAttribute> GetSocketItemWeaponAttributeBonus(Item item) {
            List<AosWeaponAttribute> types = new List<AosWeaponAttribute>();
            if (item is RuneWord rune) {
                 switch (rune.Name) {
                    case "Mox":
                        types.Add(AosWeaponAttribute.HitLeechHits);
                    break;
                    case "Kaz":
                        types.Add(AosWeaponAttribute.HitColdArea);
                    break;
                    case "Za":
                        types.Add(AosWeaponAttribute.HitEnergyArea);
                    break;
                    case "Dum":
                        types.Add(AosWeaponAttribute.HitFireArea);
                    break;
                    case "Dem":
                        types.Add(AosWeaponAttribute.HitLeechMana);
                    break;
                    case "Ze":
                        types.Add(AosWeaponAttribute.HitLightning);
                    break;
                    case "Lar":
                        types.Add(AosWeaponAttribute.HitLowerDefend);
                    break;
                 }
            }
            return types;
        }
        public static List<AosArmorAttribute> GetSocketArmorAttributeBonus(Item item) {
            List<AosArmorAttribute> types = new List<AosArmorAttribute>();
            if (item is RuneWord rune) {
                 switch (rune.Name) {
                    case "Mox":
                        types.Add(AosArmorAttribute.MageArmor);
                    break;
                    case "Kaz":
                        types.Add(AosArmorAttribute.SelfRepair);
                    break;
                    case "Za":
                        types.Add(AosArmorAttribute.LowerStatReq);
                    break;
                 }
            }
            return types;
        }
        public static List<AosAttribute> GetSocketItemAttributeBonus(Item item, Item parent) {
            List<AosAttribute> types = new List<AosAttribute>();
            if (item is RuneWord rune) {
                 switch (rune.Name) {
                    case "Lux":
                        types.Add(AosAttribute.AttackChance);
                    break;
                    case "Kres":
                        types.Add(AosAttribute.BonusDex);
                    break;
                    case "Hur":
                        types.Add(AosAttribute.RegenHits);
                    break;
                    case "Aeo":
                        types.Add(AosAttribute.BonusInt);
                    break;
                    case "Zoat":
                        types.Add(AosAttribute.RegenMana);
                    break;
                    case "Mea":
                        types.Add(AosAttribute.RegenStam);
                    break;
                    case "Aloz":
                        types.Add(AosAttribute.BonusStr);
                    break;
                    case "Vox":
                        types.Add(AosAttribute.CastRecovery);
                    break;
                    case "Vex":
                        types.Add(AosAttribute.CastSpeed);
                    break;
                    case "Cur":
                        types.Add(AosAttribute.DefendChance);
                    break;
                    case "Esp":
                        types.Add(AosAttribute.EnhancePotions);
                    break;
                }
                if (parent is BaseArmor) {
                     switch (rune.Name) {
                        case "Dum":
                        case "Dem":
                        case "Ze":
                        case "Lar":
                        case "Zo":
                        case "Mir":
                        case "Li":
                        case "Cha":
                            types.Add(Enchant.RandomAttribute());
                            types.Add(Enchant.RandomAttribute());
                        break;
                    }
                }   
            }
            return types;
        }

        public static bool IsGem(Item item)
        {
            return (
                item is Sapphire || item is Ruby || item is Diamond || item is StarSapphire || item is Emerald || item is Tourmaline || item is Citrine || item is Amber || item is Amethyst
                || item is BrilliantAmber || item is BlueDiamond || item is FireRuby || item is WhitePearl || item is EcruCitrine || item is Turquoise || item is DarkSapphire || item is PerfectEmerald
                );
        }

        public static List<ResistanceMod> GetSocketItemResistanceBonus(Item item) {
            List<ResistanceMod> types = new List<ResistanceMod>();
            if (IsGem(item)) {
                if (item is Diamond)
                {
                    types.Add(new ResistanceMod(ResistanceType.Physical, +6));
                }
                else if (item is BlueDiamond)
                {
                    types.Add(new ResistanceMod(ResistanceType.Physical, +9));
                }
                else if (item is WhitePearl)
                {
                    types.Add(new ResistanceMod(ResistanceType.Energy, +9));
                }
                else if (item is Amber)
                {
                    types.Add(new ResistanceMod(ResistanceType.Fire, +3));
                    types.Add(new ResistanceMod(ResistanceType.Energy, +3));
                }
                else if (item is BrilliantAmber)
                {
                    types.Add(new ResistanceMod(ResistanceType.Fire, +6));
                    types.Add(new ResistanceMod(ResistanceType.Energy, +6));
                }
                else if (item is Citrine)
                {
                    types.Add(new ResistanceMod(ResistanceType.Energy, +3));
                    types.Add(new ResistanceMod(ResistanceType.Poison, +3));
                }
                else if (item is EcruCitrine)
                {
                    types.Add(new ResistanceMod(ResistanceType.Energy, +6));
                    types.Add(new ResistanceMod(ResistanceType.Poison, +6));
                }
                else if (item is Tourmaline)
                {
                    types.Add(new ResistanceMod(ResistanceType.Cold, +3));
                    types.Add(new ResistanceMod(ResistanceType.Fire, +3));
                }
                else if (item is Turquoise)
                {
                    types.Add(new ResistanceMod(ResistanceType.Cold, +6));
                    types.Add(new ResistanceMod(ResistanceType.Fire, +6));
                }
                else if (item is StarSapphire)
                {
                    types.Add(new ResistanceMod(ResistanceType.Cold, +3));
                    types.Add(new ResistanceMod(ResistanceType.Energy, +3));
                }
                else if (item is DarkSapphire)
                {
                    types.Add(new ResistanceMod(ResistanceType.Cold, +6));
                    types.Add(new ResistanceMod(ResistanceType.Energy, +6));
                }
                else if (item is Ruby)
                {
                    types.Add(new ResistanceMod(ResistanceType.Fire, +6));
                }
                else if (item is FireRuby)
                {
                    types.Add(new ResistanceMod(ResistanceType.Fire, +9));
                }
                else if (item is Emerald)
                {
                    types.Add(new ResistanceMod(ResistanceType.Poison, +6));
                }
                else if (item is PerfectEmerald)
                {
                    types.Add(new ResistanceMod(ResistanceType.Poison, +9));
                }
                else if (item is Sapphire)
                {
                    types.Add(new ResistanceMod(ResistanceType.Cold, +6));
                } else if (item is Amethyst)
                {
                    types.Add(new ResistanceMod(ResistanceType.Energy, +6));
                }
            } else if (item is RuneWord rune) {
                switch (rune.Name) {
                    case "Amn":
                        types.Add(new ResistanceMod(ResistanceType.Physical, +14));
                    break;
                    case "Ort":
                        types.Add(new ResistanceMod(ResistanceType.Poison, +14));
                    break;
                    case "Nex":
                        types.Add(new ResistanceMod(ResistanceType.Cold, +14));
                    break;
                    case "Um":
                        types.Add(new ResistanceMod(ResistanceType.Energy, +14));
                    break;
                    case "Drux":
                        types.Add(new ResistanceMod(ResistanceType.Fire, +14));
                    break;
                    case "Vas":
                        types.Add(new ResistanceMod(ResistanceType.Fire, +8));
                        types.Add(new ResistanceMod(ResistanceType.Energy, +8));
                    break;
                    case "Hem":
                        types.Add(new ResistanceMod(ResistanceType.Fire, +8));
                        types.Add(new ResistanceMod(ResistanceType.Poison, +8));
                    break;
                    case "Zaq":
                        types.Add(new ResistanceMod(ResistanceType.Fire, +8));
                        types.Add(new ResistanceMod(ResistanceType.Cold, +8));
                    break;
                    case "Vax":
                        types.Add(new ResistanceMod(ResistanceType.Poison, +8));
                        types.Add(new ResistanceMod(ResistanceType.Energy, +8));
                    break;
                    case "Doth":
                        types.Add(new ResistanceMod(ResistanceType.Poison, +8));
                        types.Add(new ResistanceMod(ResistanceType.Cold, +8));
                    break;
                    case "Zet":
                        types.Add(new ResistanceMod(ResistanceType.Physical, +8));
                        types.Add(new ResistanceMod(ResistanceType.Poison, +8));
                    break;
                    case "Mar":
                        types.Add(new ResistanceMod(ResistanceType.Physical, +8));
                        types.Add(new ResistanceMod(ResistanceType.Fire, +8));
                    break;
                    case "Leq":
                        types.Add(new ResistanceMod(ResistanceType.Physical, +8));
                        types.Add(new ResistanceMod(ResistanceType.Energy, +8));
                    break;
                    case "Pax":
                        types.Add(new ResistanceMod(ResistanceType.Physical, +8));
                        types.Add(new ResistanceMod(ResistanceType.Cold, +8));
                    break;
                }
                
            } 
            return types;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Server.Items;
using Server.Talent;

namespace Server.Misc
{
    public class SocketBonus
    {
        public static string RandomWeaponRuneWord()
        {
            string[] words =
            {
                "KresHurKazZetLuxNex",
                "LiEspDemChaKaxVax",
                "ZoatDothPaxOrtEspMox",
                "LiDemKresKazAmnVas",
                "NexOrtDemKazDumPax",
                "VasOrtLuxPaxVaxNex",
                "NexZaqVasHemPax",
                "VasCurHurDrux",
                "MeaMoxVoxVexAmn",
                "EspCurLuxKresOrt",
                "AmnDruxDothOrtVax",
                "OrtVasHemZaqNex",
                "NexZaqAmnVasOrt",
                "UmDruxZaqOrtHem",
                "DruxHemOrtVasZet",
                "VasDothZetOrtLeq",
                "HemOrtMarZetAmn",
                "ZaqMarAmnNexDrux",
                "VaxZetDothDruxVax",
                "DothZetPaxLeqDoth",
                "ZetDruxNexOrtHem",
                "MarUmDruxZaqZet",
                "LeqAmnVasNexPax",
                "PaxLeqUmAmnLeq",
                "VaxDothHemOrtVas",
                "AlozHemLeqOrt",
                "VaxZoatLuxZet",
                "KresMeaOrtDoth",
                "DothNexZoatUm",
                "AeoOrtZetDrux",
                "VasDruxAlozZoat",
                "NexPaxZoatAloz",
                "NexHurZoatHurAloz",
                "UmAlozHurAmnVas",
                "KresAeoLuzDothVas",
                "VasOrtDruxLuxMea",
                "AeoVasKresHurOrt",
                "VoxVexAlozEsp",
                "CurVexMeaMox",
                "VexCurDruxHur",
                "VexVasOrtAmnEsp",
                "AeoNexUmEspVex",
                "KresVaxMoxMeaCur",
                "VexHurLuxNexZaq",
                "KazDemEspZeZa",
                "DumOrtMoxZaHur",
                "MoxKazOrtDumZe",
                "KazAmnCurVexPax",
                "LuxMoxDruxVexVox",
                "VexVoxZetVasOrt",
                "AmnOrtLeqAlozZoat",
                "VexAlozLeqZe",
                "VasZeKazAeoHem",
                "AloxVoxMarZetDoth",
                "ZeCurHemDruxDoth",
                "LuxOrtAmnDoth",
                "LarChaZoAmnOrt",
                "LarAmnChaZoPax",
                "ChaZaAeoDruxZoat",
                "ZeMirZoAmnVas",
                "ZoDruxAmnDoth",
                "LeqMarZothZeZa",
                "UmOrtChaLiAmn",
                "KazDumChaPaxVax",
                "MoxCurDothLiLar",
                "KazDemDumVexZa",
            };
            return words[Utility.Random(words.Length)];
        }
        public static string RandomShieldRuneWord()
        {
            string[] words =
            {
                "ZetAmnMarLeq",
                "PaxZaqMarOrtNex",
                "ZaqMarOrt",
                "ZeZaKazMoxVex"
            };
            return words[Utility.Random(words.Length)];
        }
        public static string RandomJewelleryRuneWord()
        {
            string[] words =
            {
                "AmnMarLeqDruxZaq",
                "NexAmnVasUmZet",
                "VasZetUmZetOrt",
                "OrtMarLeqAmnOrt",
                "OrtVasDruxCurMox",
                "EspCurAmnOrt"
            };
            return words[Utility.Random(words.Length)];
        }
        public static string RandomArmorRuneWord()
        {
            string[] words =  {
                "DruxOrtHemZetVax",
                "NexDruxVasHemZaq",
                "MarNexMarAmnVas",
                "DruxZaqUmOrtUm",
                "HemOrtMarVasNex",
                "DothZetAmnLeqZaq",
                "OrtZetMarDruxAmn",
                "MarAmnNexAmnMar",
                "ZetVaxLeqDruxOrt",
                "PaxZetUmLeqDoth",
                "DruxPaxNexOrtNex",
                "UmDruxZaqHemZet",
                "MeaZoatKres",
                "ZoatKresOrt",
                "HurLuxAloz",
                "MeaZoatKresAloz",
                "AmnDothHurAloz",
                "HurZaqMarAmn",
                "ZaqAmnOrtKresLux",
                "AmnZoatOrtDruxLux",
                "HurAlozOrtVasMea",
                "EspMoxCur",
                "ZoatOrtCur",
                "ZoatZaOrtKazDem",
                "VexAmnVaxKazDem",
                "KazVaxZaDem",
                "KazOrtDemDum",
                "DumVoxAlozVexPax",
                "MeaAeoKresHurDum",
                "EspDruxMirZaLux",
                "ZoatDruxDem",
                "DruxDemKazOrtDum",
                "KazOrtDumZaLi",
                "KazAmnVasLi",
                "DruxHurEspOrt",
                "AmnCurOrt",
                "OrtVasMox",
                "NexZeDum",
                "LuxDumKresVexHur",
                "KresEspCurAmnMar",
                "OrtAmnZeKazMar",
                "LiMirAmnOrt",
                "MirZoMarLar",
                "VexKazZoAmn",
                "EspMoxCurZo",
                "LiChaKaxDumKres",
                "DemLiDumKazAmn"
            };
            return words[Utility.Random(words.Length)];
        }
        public static string RandomClothingRuneWord()
        {
            string[] words = {
                "ZetAmnMar",
                "LuxAmnMarAoe",
                "CurOrtZetMar",
                "MarNexLeqMar",
                "NexMarUmDrux",
                "DruxMarPaxZaq",
                "PaxZaqOrtZet",
                "AlozAeoLux",
                "LuxZetOrt",
                "AeoMeaAoe",
                "MeaAlozOrtKres",
                "ZaOrtZeAloz",
                "OrtVexMar",
                "KresZeAlozOrt",
                "ChaZoAmnZoat",
                "ChaAmnOrtZet",
                "ChaOrtAlozMea"
            };
            return words[Utility.Random(words.Length)];
        }

        public static string GetRuneWordSymbolType(string runeword)
        {
            return runeword switch
            {
                "ZetAmnMar"            => "Half Apron",
                "LuxAmnMarAoe"         => "Robe",
                "CurOrtZetMar"         => "Robe",
                "MarNexLeqMar"         => "Bear Mask",
                "NexMarUmDrux"         => "Deer Mask",
                "DruxMarPaxZaq"        => "Wizards Hat",
                "PaxZaqOrtZet"         => "Horned Tribal Mask",
                "AlozAeoLux"           => "Kasa",
                "LuxZetOrt"            => "Jester Hat",
                "AeoMeaAoe"            => "Cloth Ninja Hood",
                "MeaAlozOrtKres"       => "Kasa",
                "ZaOrtZeAloz"          => "Bandana",
                "OrtVexMar"            => "Bear Mask",
                "KresZeAlozOrt"        => "Tricorne Hat",
                "ChaZoAmnZoat"         => "Tricorne Hat",
                "ChaAmnOrtZet"         => "Cloak",
                "ChaOrtAlozMea"        => "Bandana",
                "DruxOrtHemZetVax"     => "Studded Chest",
                "NexDruxVasHemZaq"     => "Ringmail Gloves",
                "MarNexMarAmnVas"      => "Plate Helm",
                "DruxZaqUmOrtUm"       => "Plate Chest",
                "HemOrtMarVasNex"      => "Plate Gloves",
                "DothZetAmnLeqZaq"     => "Plate Gorget",
                "OrtZetMarDruxAmn"     => "Chain Legs",
                "MarAmnNexAmnMar"      => "Bone Arms",
                "ZetVaxLeqDruxOrt"     => "Bone Helm",
                "PaxZetUmLeqDoth"      => "Leather Legs",
                "DruxPaxNexOrtNex"     => "Chain Chest",
                "UmDruxZaqHemZet"      => "Leather Gorget",
                "MeaZoatKres"          => "Plate Do",
                "ZoatKresOrt"          => "Leather Hiro Sode",
                "HurLuxAloz"           => "Plate Battle Kabuto",
                "MeaZoatKresAloz"      => "Plate Do",
                "AmnDothHurAloz"       => "Leather Ninja Mitts",
                "HurZaqMarAmn"         => "Plate Suneate",
                "ZaqAmnOrtKresLux"     => "Leather Mempo",
                "AmnZoatOrtDruxLux"    => "Plate Do",
                "HurAlozOrtVasMea"     => "Leather Ninja Mitts",
                "EspMoxCur"            => "Dragon Helm",
                "ZoatOrtCur"           => "Elven Glasses",
                "ZoatZaOrtKazDem"      => "Elven Glasses",
                "VexAmnVaxKazDem"      => "Elven Glasses",
                "KazVaxZaDem"          => "Elven Glasses",
                "KazOrtDemDum"         => "Elven Glasses",
                "DumVoxAlozVexPax"     => "Elven Glasses",
                "MeaAeoKresHurDum"     => "Elven Glasses",
                "EspDruxMirZaLux"      => "Elven Glasses",
                "ZoatDruxDem"          => "Elven Glasses",
                "DruxDemKazOrtDum"     => "Elven Glasses",
                "KazOrtDumZaLi"        => "Elven Glasses",
                "KazAmnVasLi"          => "Elven Glasses",
                "DruxHurEspOrt"        => "Elven Glasses",
                "AmnCurOrt"            => "Chain Legs",
                "OrtVasMox"            => "Winged Helm",
                "NexZeDum"             => "Orc Helm",
                "LuxDumKresVexHur"     => "Female Plate Chest",
                "KresEspCurAmnMar"     => "Plate Chest",
                "OrtAmnZeKazMar"       => "Leather Gloves",
                "LiMirAmnOrt"          => "Plate Gloves",
                "MirZoMarLar"          => "Plate Gorget",
                "VexKazZoAmn"          => "Orc Helm",
                "EspMoxCurZo"          => "Bone Chest",
                "LiChaKaxDumKres"      => "Plate Battle Kabuto",
                "DemLiDumKazAmn"       => "Plate Legs",
                "AmnMarLeqDruxZaq"     => "Gold Bracelet",
                "NexAmnVasUmZet"       => "Gold Bracelet",
                "VasZetUmZetOrt"       => "Gold Ring",
                "OrtMarLeqAmnOrt"      => "Gold Ring",
                "OrtVasDruxCurMox"     => "Gold Bracelet",
                "EspCurAmnOrt"         => "Silver Ring",
                "ZetAmnMarLeq"         => "Wooden Kite Shield",
                "PaxZaqMarOrtNex"      => "Heater Shield",
                "ZaqMarOrt"            => "Any",
                "ZeZaKazMoxVex"        => "Metal Shield",
                "KresHurKazZetLuxNex"  => "Frost bringer",
                "LiEspDemChaKaxVax"    => "Enchanted Titan Leg Bone",
                "ZoatDothPaxOrtEspMox" => "The Beserkers Maul",
                "LiDemKresKazAmnVas"   => "Staff Of Power",
                "NexOrtDemKazDumPax"   => "Blade Of The Righteous",
                "VasOrtLuxPaxVaxNex"   => "Calm",
                "NexZaqVasHemPax"      => "Magical Shortbow",
                "VasCurHurDrux"        => "Elven Composite Longbow",
                "MeaMoxVoxVexAmn"      => "Radiant Scimitar",
                "EspCurLuxKresOrt"     => "Ornate Axe",
                "AmnDruxDothOrtVax"    => "Double Axe",
                "OrtVasHemZaqNex"      => "Katana",
                "NexZaqAmnVasOrt"      => "Longsword",
                "UmDruxZaqOrtHem"      => "Longsword",
                "DruxHemOrtVasZet"     => "Bone Harvester",
                "VasDothZetOrtLeq"     => "Bow",
                "HemOrtMarZetAmn"      => "Bardiche",
                "ZaqMarAmnNexDrux"     => "Kryss",
                "VaxZetDothDruxVax"    => "Black Staff",
                "DothZetPaxLeqDoth"    => "Maul",
                "ZetDruxNexOrtHem"     => "Lance",
                "MarUmDruxZaqZet"      => "Bow",
                "LeqAmnVasNexPax"      => "War Fork",
                "PaxLeqUmAmnLeq"       => "War Hammer",
                "VaxDothHemOrtVas"     => "Executioners Axe",
                "AlozHemLeqOrt"        => "Tessen",
                "VaxZoatLuxZet"        => "Sai",
                "KresMeaOrtDoth"       => "Nunchaku",
                "DothNexZoatUm"        => "Tetsubo",
                "AeoOrtZetDrux"        => "Yumi",
                "VasDruxAlozZoat"      => "Bokuto",
                "NexPaxZoatAloz"       => "No Dachi",
                "NexHurZoatHurAloz"    => "Daisho",
                "UmAlozHurAmnVas"      => "No Dachi",
                "KresAeoLuzDothVas"    => "Tessen",
                "VasOrtDruxLuxMea"     => "Kama",
                "AeoVasKresHurOrt"     => "Yumi",
                "VoxVexAlozEsp"        => "Rune Blade",
                "CurVexMeaMox"         => "Diamond Mace",
                "VexCurDruxHur"        => "Wild Staff",
                "VexVasOrtAmnEsp"      => "Assassin Spike",
                "AeoNexUmEspVex"       => "War Cleaver",
                "KresVaxMoxMeaCur"     => "Elven Machete",
                "VexHurLuxNexZaq"      => "War Mace",
                "KazDemEspZeZa"        => "Halberd",
                "DumOrtMoxZaHur"       => "Bow",
                "MoxKazOrtDumZe"       => "Club",
                "KazAmnCurVexPax"      => "ShortSpear",
                "LuxMoxDruxVexVox"     => "Lance",
                "VexVoxZetVasOrt"      => "Dagger",
                "AmnOrtLeqAlozZoat"    => "Heavy Crossbow",
                "VexAlozLeqZe"         => "Black Staff",
                "VasZeKazAeoHem"       => "Gnarled Staff",
                "AloxVoxMarZetDoth"    => "Scepter",
                "ZeCurHemDruxDoth"     => "Cutlass",
                "LuxOrtAmnDoth"        => "Cleaver",
                "LarChaZoAmnOrt"       => "Katana",
                "LarAmnChaZoPax"       => "Dagger",
                "ChaZaAeoDruxZoat"     => "Skinning Knife",
                "ZeMirZoAmnVas"        => "Halberd",
                "ZoDruxAmnDoth"        => "Kryss",
                "LeqMarZothZeZa"       => "Pike",
                "UmOrtChaLiAmn"        => "Bardiche",
                "KazDumChaPaxVax"      => "Scythe",
                "MoxCurDothLiLar"      => "Longsword",
                "KazDemDumVexZa"       => "Repeating Crossbow"
            };
        }

        public static string GetItemName(Item item)
        {
            string itemName = item.Name;
            if (string.IsNullOrEmpty(itemName)) {
                itemName = item.ItemData.Name;
                itemName = Regex.Replace(itemName, @"%.%", "");
            }
            if (!string.IsNullOrEmpty(itemName))
            {
                itemName = string.Concat(itemName[0].ToString().ToUpper(), itemName.AsSpan(1));
            }
            return itemName;
        }
        public static void AddItem(Mobile from, Item item, Item socketItem) {
            string overheadMessage = "";
            Item runeWordItem = null;
            string itemName = GetItemName(socketItem);
            if (item is BaseWeapon { SocketAmount: > 0 } weapon && weapon.SocketArray.Length < weapon.SocketAmount) {
                weapon.Sockets = weapon.SocketArray.Length > 0 ? weapon.Sockets  + "," + itemName : itemName;
                if (socketItem is RuneWord) {
                    runeWordItem = GetRuneWordWeapon(weapon);
                }
                overheadMessage = $"* You add {itemName} to the weapon *";
            } else if (item is BaseArmor { SocketAmount: > 0 } armor && armor.SocketArray.Length < armor.SocketAmount)
            {
                armor.Sockets = armor.SocketArray.Length > 0 ? armor.Sockets  + "," + itemName : itemName;
                if (socketItem is RuneWord) {
                    runeWordItem = GetRuneWordArmor(armor);
                }
                overheadMessage = $"* You add {itemName} to the armor *";
            } else if (item is BaseJewel { SocketAmount: > 0 } jewellery && jewellery.SocketArray.Length < jewellery.SocketAmount) {
                jewellery.Sockets = jewellery.SocketArray.Length > 0 ? jewellery.Sockets  + "," + itemName : itemName;
                if (socketItem is RuneWord) {
                    runeWordItem = GetRuneWordJewellery(jewellery);
                }
                overheadMessage = $"* You add {itemName} to the armor *";
            } else if (item is BaseShield { SocketAmount: > 0 } shield && shield.SocketArray.Length < shield.SocketAmount)
            {
                shield.Sockets = shield.SocketArray.Length > 0 ? shield.Sockets + "," + itemName : itemName;
                if (socketItem is RuneWord) {
                    runeWordItem = GetRuneWordShield(shield);
                }
                overheadMessage = $"* You add {itemName} to the armor *";
            } else if (item is BaseClothing { PocketAmount: > 0 } clothing && clothing.PocketArray.Length < clothing.PocketAmount && clothing is BaseHat or BaseWaist or BaseOuterTorso) {
                clothing.Pockets = clothing.PocketArray.Length > 0 ? clothing.Pockets + "," + itemName : itemName;
                if (socketItem is RuneWord) {
                    runeWordItem = GetRuneWordClothing(clothing);
                }
                overheadMessage = $"* You add {itemName} to the clothing *";
            } else {
                from.SendMessage($"You cannot place {itemName} into this item");
            }
            if (runeWordItem != null) {
                item.Delete();
                from.Backpack.DropItem(runeWordItem);
                from.SendSound(0x1F7);
                overheadMessage = "* Your runewords create a powerful item *";
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

        public static void AddJewellerySocketProperties(BaseJewel jewel, List<ResistanceMod> resistanceMods)
        {
            AosElementAttributes jewelResistances = jewel.Resistances;
            foreach(ResistanceMod mod in resistanceMods) {
                if (mod.Type == ResistanceType.Cold)
                {
                    jewelResistances.Cold += mod.Offset;
                } else if (mod.Type == ResistanceType.Energy)
                {
                    jewelResistances.Energy += mod.Offset;
                } else if (mod.Type == ResistanceType.Fire)
                {
                    jewelResistances.Fire += mod.Offset;
                } else if (mod.Type == ResistanceType.Poison)
                {
                    jewelResistances.Poison += mod.Offset;
                } else if (mod.Type == ResistanceType.Physical)
                {
                    jewelResistances.Physical += mod.Offset;
                }
            }
        }

        public static void AddClothingPocketProperties(BaseClothing clothing, List<ResistanceMod> resistanceMods, Dictionary<AosArmorAttribute, int> clothingAttributeDictionary)
        {
            AosElementAttributes clothingAttributes = clothing.Resistances;
            foreach(ResistanceMod mod in resistanceMods) {
                if (mod.Type == ResistanceType.Cold)
                {
                    clothingAttributes.Cold += mod.Offset;
                } else if (mod.Type == ResistanceType.Energy)
                {
                    clothingAttributes.Energy += mod.Offset;
                } else if (mod.Type == ResistanceType.Fire)
                {
                    clothingAttributes.Fire += mod.Offset;
                } else if (mod.Type == ResistanceType.Poison)
                {
                    clothingAttributes.Poison += mod.Offset;
                } else if (mod.Type == ResistanceType.Physical)
                {
                    clothingAttributes.Physical += mod.Offset;
                }
            }

            AosArmorAttributes armorAttributes = clothing.ClothingAttributes;
            foreach (var (key, value) in clothingAttributeDictionary)
            {
                armorAttributes[key] += value;
            }
            clothing.ClothingAttributes  = armorAttributes;
        }

        public static void AddWeaponSocketProperties(BaseWeapon weapon, List<ResistanceMod> resistanceMods
            , Dictionary<AosWeaponAttribute, int> aosWeaponAttributeDictionary
            , Dictionary<AosElementAttribute, int> elementAttributeDictionary)
        {
            AosWeaponAttributes weaponAttributes = weapon.WeaponAttributes;
            AosElementAttributes aosElementDamages = weapon.AosElementDamages;
            foreach (ResistanceMod mod in resistanceMods)
            {
                if (mod.Type == ResistanceType.Cold)
                {
                    weaponAttributes.ResistColdBonus += mod.Offset;
                }
                else if (mod.Type == ResistanceType.Energy)
                {
                    weaponAttributes.ResistEnergyBonus += mod.Offset;
                }
                else if (mod.Type == ResistanceType.Fire)
                {
                    weaponAttributes.ResistFireBonus += mod.Offset;
                }
                else if (mod.Type == ResistanceType.Poison)
                {
                    weaponAttributes.ResistPoisonBonus += mod.Offset;
                }
                else if (mod.Type == ResistanceType.Physical)
                {
                    weaponAttributes.ResistPhysicalBonus += mod.Offset;
                }
            }
            foreach (var (key, value) in aosWeaponAttributeDictionary)
            {
                weaponAttributes[key] += value;
            }
            foreach (var (key, value) in elementAttributeDictionary)
            {
                aosElementDamages[key] += value;
            }

            weapon.AosElementDamages = aosElementDamages;
            weapon.WeaponAttributes = weaponAttributes;
        }

        public static void AddArmorSocketProperties(BaseArmor armor, List<ResistanceMod> resistanceMods, Dictionary<AosArmorAttribute, int> armorAttributeDictionary)
        {
            AosArmorAttributes armorAttributes = armor.ArmorAttributes;
            foreach(ResistanceMod mod in resistanceMods) {
                if (mod.Type == ResistanceType.Cold)
                {
                    armor.ColdBonus += mod.Offset;
                } else if (mod.Type == ResistanceType.Energy)
                {
                    armor.EnergyBonus += mod.Offset;
                } else if (mod.Type == ResistanceType.Fire)
                {
                    armor.FireBonus += mod.Offset;
                } else if (mod.Type == ResistanceType.Poison)
                {
                    armor.PoisonBonus += mod.Offset;
                } else if (mod.Type == ResistanceType.Physical)
                {
                    armor.PhysicalBonus += mod.Offset;
                }
            }
            foreach (var (key, value) in armorAttributeDictionary)
            {
                armorAttributes[key] += value;
            }

            armor.ArmorAttributes = armorAttributes;
        }

        public static void AddSocketProperties(Item socket, Item parent)
        {
            List<ResistanceMod> resistanceMods = GetSocketItemResistanceBonus(socket);
            AosAttributes aosAttributes = parent switch
            {
                BaseWeapon weapon     => weapon.Attributes,
                BaseArmor armor       => armor.Attributes,
                BaseJewel jewel       => jewel.Attributes,
                BaseClothing clothing => clothing.Attributes,
                _                     => null
            };

            Dictionary<AosAttribute, int> attributes = GetSocketItemAttributeBonus(socket, parent);
            foreach(var (key, value) in attributes) {
                if (aosAttributes != null)
                {
                    aosAttributes[key] += value;
                }
            }
            if (parent is BaseWeapon baseWeapon)
            {
                baseWeapon.Attributes = aosAttributes;
                Dictionary<AosWeaponAttribute, int> aosWeaponAttributeDictionary = GetSocketItemWeaponAttributeBonus(socket);
                Dictionary<AosElementAttribute, int> elementAttributeDictionary = GetSocketItemElementAttributeBonus(socket);
                AddWeaponSocketProperties(baseWeapon, resistanceMods, aosWeaponAttributeDictionary, elementAttributeDictionary);
            } else if (parent is BaseArmor baseArmor)
            {
                baseArmor.Attributes = aosAttributes;
                Dictionary<AosArmorAttribute, int> armorAttributeDictionary = GetSocketArmorAttributeBonus(socket);
                AddArmorSocketProperties(baseArmor, resistanceMods, armorAttributeDictionary);
            } else if (parent is BaseJewel baseJewel)
            {
                baseJewel.Attributes = aosAttributes;
                AddJewellerySocketProperties(baseJewel, resistanceMods);
            } else if (parent is BaseClothing baseClothing)
            {
                baseClothing.Attributes = aosAttributes;
                Dictionary<AosArmorAttribute, int> clothingAttributeDictionary = GetSocketArmorAttributeBonus(socket);
                AddClothingPocketProperties(baseClothing, resistanceMods, clothingAttributeDictionary);
            }
        }

        public static string GetRuneWords(List<string> sockets) {
            string runeWords = "";
            string[] runeList = RuneWord.WordList();
            foreach(string name in sockets) {
                if (runeList.Any(name.Contains))
                {
                    runeWords += name;
                }
            }
            return runeWords;
        }

        public static BaseClothing GetRuneWordClothing(BaseClothing clothing, string runeWordOverride = "") {
            List<string> pockets = clothing.PocketArray.ToList();
            string runeWords = GetRuneWords(pockets);
            if (!string.IsNullOrEmpty(runeWordOverride))
            {
                runeWords = runeWordOverride;
            }
            if (!string.IsNullOrEmpty(runeWords)) {
                switch(runeWords) {
                    case "ZetAmnMar":
                        if (clothing is HalfApron || runeWordOverride?.Length > 0) {
                            return new CrimsonCincture();
                        }
                        break;
                    case "LuxAmnMarAoe":
                        if (clothing is Robe || runeWordOverride?.Length > 0) {
                            return new RobeOfTheEclipse();
                        }
                        break;
                    case "CurOrtZetMar":
                        if (clothing is Robe || runeWordOverride?.Length > 0) {
                            return new RobeOfTheEquinox();
                        }
                        break;
                    case "MarNexLeqMar":
                        if (clothing is BearMask || runeWordOverride?.Length > 0) {
                            return new SpiritOfTheTotem();
                        }
                        break;
                    case "NexMarUmDrux":
                        if (clothing is DeerMask || runeWordOverride?.Length > 0) {
                            return new HuntersHeaddress();
                        }
                        break;
                    case "DruxMarPaxZaq":
                        if (clothing is WizardsHat || runeWordOverride?.Length > 0) {
                            return new HatOfTheMagi();
                        }
                        break;
                    case "PaxZaqOrtZet":
                        if (clothing is HornedTribalMask || runeWordOverride?.Length > 0) {
                            return new DivineCountenance();
                        }
                        break;
                    case "AlozAeoLux":
                        if (clothing is Kasa || runeWordOverride?.Length > 0) {
                            return new AncientFarmersKasa();
                        }
                        break;
                    case "LuxZetOrt":
                        if (clothing is JesterHat || runeWordOverride?.Length > 0) {
                            return new JesterHatofChuckles();
                        }
                        break;
                    case "AeoMeaAoe":
                        if (clothing is ClothNinjaHood || runeWordOverride?.Length > 0) {
                            return new BlackLotusHood();
                        }
                        break;
                    case "MeaAlozOrtKres":
                        if (clothing is Kasa || runeWordOverride?.Length > 0) {
                            return new KasaOfTheRajin();
                        }
                        break;
                    case "ZaOrtZeAloz":
                        if (clothing is Bandana || runeWordOverride?.Length > 0) {
                            return new BurglarsBandana();
                        }
                        break;
                    case "OrtVexMar":
                        if (clothing is BearMask || runeWordOverride?.Length > 0) {
                            return new PolarBearMask();
                        }
                        break;
                    case "KresZeAlozOrt":
                        if (clothing is TricorneHat || runeWordOverride?.Length > 0) {
                            return new DreadPirateHat();
                        }
                        break;
                    case "ChaZoAmnZoat":
                        if (clothing is TricorneHat || runeWordOverride?.Length > 0)
                        {
                            {
                                return new CaptainJohnsHat();
                            }
                        }
                        break;
                    case "ChaAmnOrtZet":
                        if (clothing is Cloak || runeWordOverride?.Length > 0)
                        {
                            {
                                return new EmbroideredOakLeafCloak();
                            }
                        }
                        break;
                    case "ChaOrtAlozMea":
                        if (clothing is Bandana || runeWordOverride?.Length > 0)
                        {
                            {
                                return new CrownOfTalKeesh();
                            }
                        }
                        break;
                }
            }
            return null;
        }

        public static BaseJewel GetRuneWordJewellery(BaseJewel jewellery, string runeWordOverride = "") {
            List<string> sockets = jewellery.SocketArray.ToList();
            string runeWords = GetRuneWords(sockets);
            if (!string.IsNullOrEmpty(runeWordOverride))
            {
                runeWords = runeWordOverride;
            }
            if (!string.IsNullOrEmpty(runeWords)) {
                switch(runeWords) {
                    case "AmnMarLeqDruxZaq":
                        if (jewellery is GoldBracelet || runeWordOverride?.Length > 0)
                        {
                            return new BraceletOfHealth();
                        }
                        break;
                    case "NexAmnVasUmZet":
                        if (jewellery is GoldBracelet || runeWordOverride?.Length > 0)
                        {
                            return new OrnamentOfTheMagician();
                        }
                        break;
                    case "VasZetUmZetOrt":
                        if (jewellery is GoldRing || runeWordOverride?.Length > 0)
                        {
                            return new RingOfTheElements();
                        }
                        break;
                    case "OrtMarLeqAmnOrt":
                        if (jewellery is GoldRing || runeWordOverride?.Length > 0)
                        {
                            return new RingOfTheVile();
                        }
                        break;
                    case "OrtVasDruxCurMox":
                        if (jewellery is GoldBracelet || runeWordOverride?.Length > 0)
                        {
                            return new AlchemistsBauble();
                        }
                        break;
                    case "EspCurAmnOrt":
                        if (jewellery is SilverRing || runeWordOverride?.Length > 0)
                        {
                            return new DjinnisRing();
                        }
                        break;
                }
            }
            return null;
        }
        public static BaseShield GetRuneWordShield(BaseShield shield, string runeWordOverride = "") {
            List<string> sockets = shield.SocketArray.ToList();
            string runeWords = GetRuneWords(sockets);
            if (!string.IsNullOrEmpty(runeWordOverride))
            {
                runeWords = runeWordOverride;
            }
            if (!string.IsNullOrEmpty(runeWords)) {
                switch(runeWords) {
                    case "ZetAmnMarLeq":
                        if (shield is WoodenKiteShield || runeWordOverride?.Length > 0)
                        {
                            return new ArcaneShield();
                        }

                        break;
                    case "PaxZaqMarOrtNex":
                        if (shield is HeaterShield || runeWordOverride?.Length > 0)
                        {
                            return new Aegis();
                        }
                        break;
                    case "ZaqMarOrt":
                        return new DupresShield();
                    case "ZeZaKazMoxVex":
                        if (shield is MetalShield || runeWordOverride?.Length > 0)
                        {
                            return new ShieldOfInvulnerability();
                        }
                        break;
                }
            }
            return null;
        }

        public static BaseArmor GetRuneWordArmor(BaseArmor armor, string runeWordOverride = "") {
            List<string> sockets = armor.SocketArray.ToList();
            string runeWords = GetRuneWords(sockets);
            if (!string.IsNullOrEmpty(runeWordOverride))
            {
                runeWords = runeWordOverride;
            }
            if (!string.IsNullOrEmpty(runeWords)) {
                switch(runeWords) {
                    case "DruxOrtHemZetVax":
                        if (armor is StuddedChest || runeWordOverride?.Length > 0)
                        {
                            return new ArmorOfFortune();
                        }

                        break;
                    case "NexDruxVasHemZaq":
                        if (armor is RingmailGloves || runeWordOverride?.Length > 0)
                        {
                            return new GauntletsOfNobility();
                        }

                        break;
                    case "MarNexMarAmnVas":
                        if (armor is PlateHelm || runeWordOverride?.Length > 0)
                        {
                            return new HelmOfInsight();
                        }

                        break;
                    case "DruxZaqUmOrtUm":
                        if (armor is PlateChest || runeWordOverride?.Length > 0)
                        {
                            return new HolyKnightsBreastplate();
                        }

                        break;
                    case "HemOrtMarVasNex":
                        if (armor is PlateGloves || runeWordOverride?.Length > 0)
                        {
                            return new InquisitorsResolution();
                        }

                        break;
                    case "DothZetAmnLeqZaq":
                        if (armor is PlateGorget || runeWordOverride?.Length > 0)
                        {
                            return new JackalsCollar();
                        }

                        break;
                    case "OrtZetMarDruxAmn":
                        if (armor is ChainLegs || runeWordOverride?.Length > 0)
                        {
                            return new LeggingsOfBane();
                        }

                        break;
                    case "MarAmnNexAmnMar":
                        if (armor is BoneArms || runeWordOverride?.Length > 0)
                        {
                            return new MidnightBracers();
                        }

                        break;
                    case "ZetVaxLeqDruxOrt":
                        if (armor is BoneHelm || runeWordOverride?.Length > 0)
                        {
                            return new OrnateCrownOfTheHarrower();
                        }

                        break;
                    case "PaxZetUmLeqDoth":
                        if (armor is LeatherLegs || runeWordOverride?.Length > 0)
                        {
                            return new ShadowDancerLeggings();
                        }

                        break;
                    case "DruxPaxNexOrtNex":
                        if (armor is ChainChest || runeWordOverride?.Length > 0)
                        {
                            return new TunicOfFire();
                        }

                        break;
                    case "UmDruxZaqHemZet":
                        if (armor is LeatherGorget || runeWordOverride?.Length > 0)
                        {
                            return new VoiceOfTheFallenKing();
                        }

                        break;
                    case "MeaZoatKres":
                        if (armor is PlateDo || runeWordOverride?.Length > 0)
                        {
                            return new AncientSamuraiDo();
                        }

                        break;
                    case "ZoatKresOrt":
                        if (armor is LeatherHiroSode || runeWordOverride?.Length > 0)
                        {
                            return new ArmsOfTacticalExcellence();
                        }

                        break;
                    case "HurLuxAloz":
                        if (armor is PlateBattleKabuto || runeWordOverride?.Length > 0)
                        {
                            return new DaimyosHelm();
                        }

                        break;
                    case "MeaZoatKresAloz":
                        if (armor is PlateDo || runeWordOverride?.Length > 0)
                        {
                            return new AncientSamuraiDo();
                        }

                        break;
                    case "AmnDothHurAloz":
                        if (armor is LeatherNinjaMitts || runeWordOverride?.Length > 0)
                        {
                            return new GlovesOfTheSun();
                        }

                        break;
                    case "HurZaqMarAmn":
                        if (armor is PlateSuneate || runeWordOverride?.Length > 0)
                        {
                            return new LegsOfStability();
                        }

                        break;
                    case "ZaqAmnOrtKresLux":
                        if (armor is LeatherMempo || runeWordOverride?.Length > 0)
                        {
                            return new LeurociansMempoOfFortune();
                        }

                        break;
                    case "AmnZoatOrtDruxLux":
                        if (armor is PlateDo || runeWordOverride?.Length > 0)
                        {
                            return new RuneBeetleCarapace();
                        }

                        break;
                    case "HurAlozOrtVasMea":
                        if (armor is LeatherNinjaMitts || runeWordOverride?.Length > 0)
                        {
                            return new Stormgrip();
                        }

                        break;
                    case "EspMoxCur":
                        if (armor is DragonHelm || runeWordOverride?.Length > 0)
                        {
                            return new AegisOfGrace();
                        }

                        break;
                    case "ZoatOrtCur":
                        if (armor is ElvenGlasses || runeWordOverride?.Length > 0)
                        {
                            return new BrightsightLenses();
                        }

                        break;
                    case "ZoatZaOrtKazDem":
                        if (armor is ElvenGlasses || runeWordOverride?.Length > 0)
                        {
                            return new MaritimeGlasses();
                        }

                        break;
                    case "VexAmnVaxKazDem":
                        if (armor is ElvenGlasses || runeWordOverride?.Length > 0)
                        {
                            return new WizardsGlasses();
                        }

                        break;
                    case "KazVaxZaDem":
                        if (armor is ElvenGlasses || runeWordOverride?.Length > 0)
                        {
                            return new TradeGlasses();
                        }

                        break;
                    case "KazOrtDemDum":
                        if (armor is ElvenGlasses || runeWordOverride?.Length > 0)
                        {
                            return new LyricalGlasses();
                        }

                        break;
                    case "DumVoxAlozVexPax":
                        if (armor is ElvenGlasses || runeWordOverride?.Length > 0)
                        {
                            return new NecromanticGlasses();
                        }

                        break;
                    case "MeaAeoKresHurDum":
                        if (armor is ElvenGlasses || runeWordOverride?.Length > 0)
                        {
                            return new LightOfWayGlasses();
                        }

                        break;
                    case "EspDruxMirZaLux":
                        if (armor is ElvenGlasses || runeWordOverride?.Length > 0)
                        {
                            return new FoldedSteelGlasses();
                        }

                        break;
                    case "ZoatDruxDem":
                        if (armor is ElvenGlasses || runeWordOverride?.Length > 0)
                        {
                            return new PoisonedGlasses();
                        }

                        break;
                    case "DruxDemKazOrtDum":
                        if (armor is ElvenGlasses || runeWordOverride?.Length > 0)
                        {
                            return new TreasureTrinketGlasses();
                        }

                        break;
                    case "KazOrtDumZaLi":
                        if (armor is ElvenGlasses || runeWordOverride?.Length > 0)
                        {
                            return new MaceShieldGlasses();
                        }

                        break;
                    case "KazAmnVasLi":
                        if (armor is ElvenGlasses || runeWordOverride?.Length > 0)
                        {
                            return new ArtsGlasses();
                        }

                        break;
                    case "DruxHurEspOrt":
                        if (armor is ElvenGlasses || runeWordOverride?.Length > 0)
                        {
                            return new AnthropomorphistGlasses();
                        }

                        break;
                    case "AmnCurOrt":
                        if (armor is ChainLegs || runeWordOverride?.Length > 0)
                        {
                            return new FeyLeggings();
                        }

                        break;
                    case "OrtVasMox":
                        if (armor is WingedHelm || runeWordOverride?.Length > 0)
                        {
                            return new HelmOfSwiftness();
                        }

                        break;
                    case "NexZeDum":
                        if (armor is OrcHelm || runeWordOverride?.Length > 0)
                        {
                            return new OrcishVisage();
                        }

                        break;
                    case "LuxDumKresVexHur":
                        if (armor is FemalePlateChest || runeWordOverride?.Length > 0)
                        {
                            return new VioletCourage();
                        }

                        break;
                    case "KresEspCurAmnMar":
                        if (armor is PlateChest || runeWordOverride?.Length > 0)
                        {
                            return new HeartOfTheLion();
                        }

                        break;
                    case "OrtAmnZeKazMar":
                        if (armor is LeatherGloves || runeWordOverride?.Length > 0)
                        {
                            return new GlovesOfThePugilist();
                        }

                        break;
                    case "LiMirAmnOrt":
                        if (armor is PlateGloves || runeWordOverride?.Length > 0)
                        {
                            return new GuantletsOfAnger();
                        }

                        break;
                    case "MirZoMarLar":
                        if (armor is PlateGorget || runeWordOverride?.Length > 0)
                        {
                            return new GladiatorsCollar();
                        }

                        break;
                    case "VexKazZoAmn":
                        if (armor is OrcHelm || runeWordOverride?.Length > 0)
                        {
                            return new OrcChieftainHelm();
                        }

                        break;
                    case "EspMoxCurZo":
                        if (armor is BoneChest || runeWordOverride?.Length > 0)
                        {
                            return new ShroudOfDeciet();
                        }

                        break;
                    case "LiChaKaxDumKres":
                        if (armor is PlateBattleKabuto || runeWordOverride?.Length > 0)
                        {
                            return new SamuraiHelm();
                        }

                        break;
                    case "DemLiDumKazAmn":
                        if (armor is PlateLegs || runeWordOverride?.Length > 0)
                        {
                            return new LeggingsOfEmbers();
                        }

                        break;
                }
            }
            return null;
        }

        public static BaseWeapon GetRuneWordWeapon(BaseWeapon weapon, string runeWordOverride = "") {
            List<string> sockets = weapon.SocketArray.ToList();
            string runeWords = GetRuneWords(sockets);
            if (!string.IsNullOrEmpty(runeWordOverride))
            {
                runeWords = runeWordOverride;
            }
            if (!string.IsNullOrEmpty(runeWords)) {
                switch(runeWords) {
                    case "KresHurKazZetLuxNex":
                        if (weapon is Frostbringer || runeWordOverride?.Length > 0)
                        {
                            return new Forbidden();
                        }
                        break;
                    case "LiEspDemChaKaxVax":
                        if (weapon is EnchantedTitanLegBone || runeWordOverride?.Length > 0)
                        {
                            return new Gruumsh();
                        }
                        break;
                    case "ZoatDothPaxOrtEspMox":
                        if (weapon is TheBeserkersMaul || runeWordOverride?.Length > 0)
                        {
                            return new Exile();
                        }
                        break;
                    case "LiDemKresKazAmnVas":
                        if (weapon is StaffOfPower || runeWordOverride?.Length > 0)
                        {
                            return new TwigOfCreation();
                        }
                        break;
                    case "NexOrtDemKazDumPax":
                        if (weapon is BladeOfTheRighteous || runeWordOverride?.Length > 0)
                        {
                            return new Valkyrie();
                        }
                        break;
                    case "VasOrtLuxPaxVaxNex":
                        if (weapon is Calm || runeWordOverride?.Length > 0)
                        {
                            return new DeathEater();
                        }
                        break;
                    case "NexZaqVasHemPax":
                        if (weapon is MagicalShortbow || runeWordOverride?.Length > 0)
                        {
                            return new Windsong();
                        }

                        break;
                    case "VasCurHurDrux":
                        if (weapon is ElvenCompositeLongbow || runeWordOverride?.Length > 0)
                        {
                            return new WildfireBow();
                        }

                        break;
                    case "MeaMoxVoxVexAmn":
                        if (weapon is RadiantScimitar || runeWordOverride?.Length > 0)
                        {
                            return new SoulSeeker();
                        }

                        break;
                    case "EspCurLuxKresOrt":
                        if (weapon is OrnateAxe || runeWordOverride?.Length > 0)
                        {
                            return new TalonBite();
                        }

                        break;
                    case "AmnDruxDothOrtVax":
                        if (weapon is DoubleAxe || runeWordOverride?.Length > 0)
                        {
                            return new AxeOfTheHeavens();
                        }

                        break;
                    case "OrtVasHemZaqNex":
                        if (weapon is Katana || runeWordOverride?.Length > 0)
                        {
                            return new BladeOfInsanity();
                        }

                        break;
                    case "NexZaqAmnVasOrt":
                        if (weapon is Longsword || runeWordOverride?.Length > 0)
                        {
                            return new BladeOfTheRighteous();
                        }

                        break;
                    case "UmDruxZaqOrtHem":
                        if (weapon is Longsword || runeWordOverride?.Length > 0)
                        {
                            return new BoneCrusher();
                        }

                        break;
                    case "DruxHemOrtVasZet":
                        if (weapon is BoneHarvester || runeWordOverride?.Length > 0)
                        {
                            return new BreathOfTheDead();
                        }

                        break;
                    case "VasDothZetOrtLeq":
                        if (weapon is Bow || runeWordOverride?.Length > 0)
                        {
                            return new Frostbringer();
                        }

                        break;
                    case "HemOrtMarZetAmn":
                        if (weapon is Bardiche || runeWordOverride?.Length > 0)
                        {
                            return new LegacyOfTheDreadLord();
                        }

                        break;
                    case "ZaqMarAmnNexDrux":
                        if (weapon is Kryss || runeWordOverride?.Length > 0)
                        {
                            return new SerpentsFang();
                        }

                        break;
                    case "VaxZetDothDruxVax":
                        if (weapon is BlackStaff || runeWordOverride?.Length > 0)
                        {
                            return new StaffOfTheMagi();
                        }

                        break;
                    case "DothZetPaxLeqDoth":
                        if (weapon is Maul || runeWordOverride?.Length > 0)
                        {
                            return new TheBeserkersMaul();
                        }

                        break;
                    case "ZetDruxNexOrtHem":
                        if (weapon is Lance || runeWordOverride?.Length > 0)
                        {
                            return new TheDragonSlayer();
                        }

                        break;
                    case "MarUmDruxZaqZet":
                        if (weapon is Bow || runeWordOverride?.Length > 0)
                        {
                            return new TheDryadBow();
                        }

                        break;
                    case "LeqAmnVasNexPax":
                        if (weapon is WarFork || runeWordOverride?.Length > 0)
                        {
                            return new TheTaskmaster();
                        }

                        break;
                    case "PaxLeqUmAmnLeq":
                        if (weapon is WarHammer || runeWordOverride?.Length > 0)
                        {
                            return new TitansHammer();
                        }

                        break;
                    case "VaxDothHemOrtVas":
                        if (weapon is ExecutionersAxe || runeWordOverride?.Length > 0)
                        {
                            return new ZyronicClaw();
                        }

                        break;
                    case "AlozHemLeqOrt":
                        if (weapon is Tessen || runeWordOverride?.Length > 0)
                        {
                            return new PilferedDancerFans();
                        }

                        break;
                    case "VaxZoatLuxZet":
                        if (weapon is Sai || runeWordOverride?.Length > 0)
                        {
                            return new DemonForks();
                        }

                        break;
                    case "KresMeaOrtDoth":
                        if (weapon is Nunchaku || runeWordOverride?.Length > 0)
                        {
                            return new DragonNunchaku();
                        }

                        break;
                    case "DothNexZoatUm":
                        if (weapon is Tetsubo || runeWordOverride?.Length > 0)
                        {
                            return new Exiler();
                        }

                        break;
                    case "AeoOrtZetDrux":
                        if (weapon is Yumi || runeWordOverride?.Length > 0)
                        {
                            return new HanzosBow();
                        }

                        break;
                    case "VasDruxAlozZoat":
                        if (weapon is Bokuto || runeWordOverride?.Length > 0)
                        {
                            return new PeasantsBokuto();
                        }

                        break;
                    case "NexPaxZoatAloz":
                        if (weapon is NoDachi || runeWordOverride?.Length > 0)
                        {
                            return new TheDestroyer();
                        }

                        break;
                    case "NexHurZoatHurAloz":
                        if (weapon is Daisho || runeWordOverride?.Length > 0)
                        {
                            return new SwordsOfProsperity();
                        }

                        break;
                    case "UmAlozHurAmnVas":
                        if (weapon is NoDachi || runeWordOverride?.Length > 0)
                        {
                            return new SwordOfTheStampede();
                        }

                        break;
                    case "KresAeoLuzDothVas":
                        if (weapon is Tessen || runeWordOverride?.Length > 0)
                        {
                            return new WindsEdge();
                        }

                        break;
                    case "VasOrtDruxLuxMea":
                        if (weapon is Kama || runeWordOverride?.Length > 0)
                        {
                            return new DarkenedSky();
                        }

                        break;
                    case "AeoVasKresHurOrt":
                        if (weapon is Yumi || runeWordOverride?.Length > 0)
                        {
                            return new TheHorselord();
                        }

                        break;
                    case "VoxVexAlozEsp":
                        if (weapon is RuneBlade || runeWordOverride?.Length > 0)
                        {
                            return new BladeDance();
                        }

                        break;
                    case "CurVexMeaMox":
                        if (weapon is DiamondMace || runeWordOverride?.Length > 0)
                        {
                            return new Bonesmasher();
                        }

                        break;
                    case "VexCurDruxHur":
                        if (weapon is WildStaff || runeWordOverride?.Length > 0)
                        {
                            return new Boomstick();
                        }

                        break;
                    case "VexVasOrtAmnEsp":
                        if (weapon is AssassinSpike || runeWordOverride?.Length > 0)
                        {
                            return new FleshRipper();
                        }

                        break;
                    case "AeoNexUmEspVex":
                        if (weapon is WarCleaver || runeWordOverride?.Length > 0)
                        {
                            return new RaedsGlory();
                        }

                        break;
                    case "KresVaxMoxMeaCur":
                        if (weapon is ElvenMachete || runeWordOverride?.Length > 0)
                        {
                            return new RighteousAnger();
                        }

                        break;
                    case "VexHurLuxNexZaq":
                        if (weapon is WarMace || runeWordOverride?.Length > 0)
                        {
                            return new ArcticDeathDealer();
                        }

                        break;
                    case "KazDemEspZeZa":
                        if (weapon is Halberd || runeWordOverride?.Length > 0)
                        {
                            return new BlazeOfDeath();
                        }

                        break;
                    case "DumOrtMoxZaHur":
                        if (weapon is Bow || runeWordOverride?.Length > 0)
                        {
                            return new BowOfTheJukaKing();
                        }

                        break;
                    case "MoxKazOrtDumZe":
                        if (weapon is Club || runeWordOverride?.Length > 0)
                        {
                            return new CavortingClub();
                        }

                        break;
                    case "KazAmnCurVexPax":
                        if (weapon is ShortSpear || runeWordOverride?.Length > 0)
                        {
                            return new EnchantedTitanLegBone();
                        }

                        break;
                    case "LuxMoxDruxVexVox":
                        if (weapon is Lance || runeWordOverride?.Length > 0)
                        {
                            return new LunaLance();
                        }

                        break;
                    case "VexVoxZetVasOrt":
                        if (weapon is Dagger || runeWordOverride?.Length > 0)
                        {
                            return new NightsKiss();
                        }

                        break;
                    case "AmnOrtLeqAlozZoat":
                        if (weapon is HeavyCrossbow || runeWordOverride?.Length > 0)
                        {
                            return new NoxRangersHeavyCrossbow();
                        }

                        break;
                    case "VexAlozLeqZe":
                        if (weapon is BlackStaff || runeWordOverride?.Length > 0)
                        {
                            return new StaffOfPower();
                        }

                        break;
                    case "VasZeKazAeoHem":
                        if (weapon is GnarledStaff || runeWordOverride?.Length > 0)
                        {
                            return new WrathOfTheDryad();
                        }

                        break;
                    case "AloxVoxMarZetDoth":
                        if (weapon is Scepter || runeWordOverride?.Length > 0)
                        {
                            return new PixieSwatter();
                        }

                        break;
                    case "ZeCurHemDruxDoth":
                        if (weapon is Cutlass || runeWordOverride?.Length > 0)
                        {
                            return new CaptainQuacklebushsCutlass();
                        }

                        break;
                    case "LuxOrtAmnDoth":
                        if (weapon is Cleaver || runeWordOverride?.Length > 0)
                        {
                            return new ColdBlood();
                        }

                        break;
                    case "LarChaZoAmnOrt":
                        if (weapon is Katana || runeWordOverride?.Length > 0)
                        {
                            return new BraveKnightOfTheBritannia();
                        }

                        break;
                    case "LarAmnChaZoPax":
                        if (weapon is Dagger || runeWordOverride?.Length > 0)
                        {
                            return new OblivionsNeedle();
                        }

                        break;
                    case "ChaZaAeoDruxZoat":
                        if (weapon is SkinningKnife || runeWordOverride?.Length > 0)
                        {
                            return new RoyalGuardSurvivalKnife();
                        }

                        break;
                    case "ZeMirZoAmnVas":
                        if (weapon is Halberd || runeWordOverride?.Length > 0)
                        {
                            return new Calm();
                        }

                        break;
                    case "ZoDruxAmnDoth":
                        if (weapon is Kryss || runeWordOverride?.Length > 0)
                        {
                            return new FangOfRactus();
                        }

                        break;
                    case "LeqMarZothZeZa":
                        if (weapon is Pike || runeWordOverride?.Length > 0)
                        {
                            return new Pacify();
                        }

                        break;
                    case "UmOrtChaLiAmn":
                        if (weapon is Bardiche || runeWordOverride?.Length > 0)
                        {
                            return new Quell();
                        }

                        break;
                    case "KazDumChaPaxVax":
                        if (weapon is Scythe || runeWordOverride?.Length > 0)
                        {
                            return new Subdue();
                        }

                        break;
                    case "MoxCurDothLiLar":
                        if (weapon is Longsword || runeWordOverride?.Length > 0)
                        {
                            return new HolySword();
                        }

                        break;
                    case "KazDemDumVexZa":
                        if (weapon is RepeatingCrossbow || runeWordOverride?.Length > 0)
                        {
                            return new ShaminoCrossbow();
                        }

                        break;
                }
            }
            return null;
        }

        public static Dictionary<AosElementAttribute, int> GetSocketItemElementAttributeBonus(Item item) {
            Dictionary<AosElementAttribute, int> types = new Dictionary<AosElementAttribute, int>();
            if (item is RuneWord rune) {
                switch (rune.Name) {
                    case "Zo":
                        types.Add(AosElementAttribute.Chaos, 15);
                        break;
                    case "Mir":
                        types.Add(AosElementAttribute.Cold, 15);
                        break;
                    case "Li":
                        types.Add(AosElementAttribute.Energy, 15);
                        break;
                    case "Cha":
                        types.Add(AosElementAttribute.Fire, 15);
                        break;
                }
            }
            return types;
        }

        public static Dictionary<AosWeaponAttribute, int> GetSocketItemWeaponAttributeBonus(Item item) {
            Dictionary<AosWeaponAttribute, int> types = new Dictionary<AosWeaponAttribute, int>();
            if (item is RuneWord rune) {
                switch (rune.Name) {
                    case "Mox":
                        types.Add(AosWeaponAttribute.HitLeechHits, 15);
                        break;
                    case "Kaz":
                        types.Add(AosWeaponAttribute.HitColdArea, 20);
                        break;
                    case "Za":
                        types.Add(AosWeaponAttribute.HitEnergyArea, 20);
                        break;
                    case "Dum":
                        types.Add(AosWeaponAttribute.HitFireArea, 20);
                        break;
                    case "Dem":
                        types.Add(AosWeaponAttribute.HitLeechMana, 15);
                        break;
                    case "Ze":
                        types.Add(AosWeaponAttribute.HitLightning, 20);
                        break;
                    case "Lar":
                        types.Add(AosWeaponAttribute.HitLowerDefend, 10);
                        break;
                }
            }
            return types;
        }
        public static Dictionary<AosArmorAttribute, int> GetSocketArmorAttributeBonus(Item item) {
            Dictionary<AosArmorAttribute, int> types = new Dictionary<AosArmorAttribute, int>();
            if (item is RuneWord rune) {
                switch (rune.Name) {
                    case "Mox":
                        types.Add(AosArmorAttribute.MageArmor, 1);
                        break;
                    case "Kaz":
                        types.Add(AosArmorAttribute.SelfRepair, 3);
                        break;
                    case "Za":
                        types.Add(AosArmorAttribute.LowerStatReq, 15);
                        break;
                }
            }
            return types;
        }
        public static Dictionary<AosAttribute, int> GetSocketItemAttributeBonus(Item item, Item parent) {
            Dictionary<AosAttribute, int> dictionary = new Dictionary<AosAttribute, int>();
            List<AosAttribute> types = new List<AosAttribute>();
            if (item is RuneWord rune) {
                switch (rune.Name) {
                    case "Lux":
                        dictionary.Add(AosAttribute.AttackChance, 7);
                        break;
                    case "Kres":
                        dictionary.Add(AosAttribute.BonusDex, 5);
                        break;
                    case "Hur":
                        dictionary.Add(AosAttribute.RegenHits, 10);
                        break;
                    case "Aeo":
                        dictionary.Add(AosAttribute.BonusInt, 5);
                        break;
                    case "Zoat":
                        dictionary.Add(AosAttribute.RegenMana, 10);
                        break;
                    case "Mea":
                        dictionary.Add(AosAttribute.RegenStam, 10);
                        break;
                    case "Aloz":
                        dictionary.Add(AosAttribute.BonusStr, 5);
                        break;
                    case "Vox":
                        dictionary.Add(AosAttribute.CastRecovery, 7);
                        break;
                    case "Vex":
                        dictionary.Add(AosAttribute.CastSpeed, 7);
                        break;
                    case "Cur":
                        dictionary.Add(AosAttribute.DefendChance, 7);
                        break;
                    case "Esp":
                        dictionary.Add(AosAttribute.EnhancePotions, 10);
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
                            dictionary.Add(Enchant.RandomAttribute(), 7);
                            dictionary.Add(Enchant.RandomAttribute(), 7);
                            break;
                    }
                }
            }
            return dictionary;
        }

        public static bool IsGem(Item item)
        {
            return (
                item is Sapphire or Ruby or Diamond or StarSapphire or Emerald or Tourmaline or Citrine or Amber or Amethyst or BrilliantAmber
                    or BlueDiamond or FireRuby or WhitePearl or EcruCitrine or Turquoise or DarkSapphire or PerfectEmerald
            );
        }

        public static List<ResistanceMod> GetSocketItemResistanceBonus(Item item) {
            List<ResistanceMod> types = new List<ResistanceMod>();
            if (IsGem(item)) {
                if (item is Diamond)
                {
                    types.Add(new ResistanceMod(ResistanceType.Physical, "Diamond", +6));
                }
                else if (item is BlueDiamond)
                {
                    types.Add(new ResistanceMod(ResistanceType.Physical,"BlueDiamond", +9));
                }
                else if (item is WhitePearl)
                {
                    types.Add(new ResistanceMod(ResistanceType.Energy, "WhitePearl", +9));
                }
                else if (item is Amber)
                {
                    types.Add(new ResistanceMod(ResistanceType.Fire, "AmberFire", +3));
                    types.Add(new ResistanceMod(ResistanceType.Energy, "AmberEnergy", +3));
                }
                else if (item is BrilliantAmber)
                {
                    types.Add(new ResistanceMod(ResistanceType.Fire, "BrilliantAmberFire", +6));
                    types.Add(new ResistanceMod(ResistanceType.Energy, "BrilliantAmberEnergy", +6));
                }
                else if (item is Citrine)
                {
                    types.Add(new ResistanceMod(ResistanceType.Energy, "CitrineEnergy", +3));
                    types.Add(new ResistanceMod(ResistanceType.Poison, "CitrinePoison", +3));
                }
                else if (item is EcruCitrine)
                {
                    types.Add(new ResistanceMod(ResistanceType.Energy, "EcruCitrineEnergy", +6));
                    types.Add(new ResistanceMod(ResistanceType.Poison, "EcruCitrinePoison", +6));
                }
                else if (item is Tourmaline)
                {
                    types.Add(new ResistanceMod(ResistanceType.Cold, "TourmalineCold", +3));
                    types.Add(new ResistanceMod(ResistanceType.Fire, "TourmalineFire", +3));
                }
                else if (item is Turquoise)
                {
                    types.Add(new ResistanceMod(ResistanceType.Cold, "TurquoiseCold", +6));
                    types.Add(new ResistanceMod(ResistanceType.Fire, "TurquoiseFire", +6));
                }
                else if (item is StarSapphire)
                {
                    types.Add(new ResistanceMod(ResistanceType.Cold, "StarSapphireCold", +3));
                    types.Add(new ResistanceMod(ResistanceType.Energy, "StarSapphireEnergy", +3));
                }
                else if (item is DarkSapphire)
                {
                    types.Add(new ResistanceMod(ResistanceType.Cold, "DarkSapphireCold", +6));
                    types.Add(new ResistanceMod(ResistanceType.Energy, "DarkSapphireEnergy", +6));
                }
                else if (item is Ruby)
                {
                    types.Add(new ResistanceMod(ResistanceType.Fire, "Ruby", +6));
                }
                else if (item is FireRuby)
                {
                    types.Add(new ResistanceMod(ResistanceType.Fire, "FireRuby", +9));
                }
                else if (item is Emerald)
                {
                    types.Add(new ResistanceMod(ResistanceType.Poison, "Emerald", +6));
                }
                else if (item is PerfectEmerald)
                {
                    types.Add(new ResistanceMod(ResistanceType.Poison, "PerfectEmerald", +9));
                }
                else if (item is Sapphire)
                {
                    types.Add(new ResistanceMod(ResistanceType.Cold, "SapphireCold", +6));
                } else if (item is Amethyst)
                {
                    types.Add(new ResistanceMod(ResistanceType.Energy, "AmethystEnergy", +6));
                }
            } else if (item is RuneWord rune) {
                switch (rune.Name) {
                    case "Amn":
                        types.Add(new ResistanceMod(ResistanceType.Physical, "Amn", +14));
                    break;
                    case "Ort":
                        types.Add(new ResistanceMod(ResistanceType.Poison, "Ort", +14));
                    break;
                    case "Nex":
                        types.Add(new ResistanceMod(ResistanceType.Cold, "NexCold", +14));
                    break;
                    case "Um":
                        types.Add(new ResistanceMod(ResistanceType.Energy, "Um", +14));
                    break;
                    case "Drux":
                        types.Add(new ResistanceMod(ResistanceType.Fire, "Drux", +14));
                    break;
                    case "Vas":
                        types.Add(new ResistanceMod(ResistanceType.Fire, "VasFire", +8));
                        types.Add(new ResistanceMod(ResistanceType.Energy, "VasEnergy", +8));
                    break;
                    case "Hem":
                        types.Add(new ResistanceMod(ResistanceType.Fire, "HemFire", +8));
                        types.Add(new ResistanceMod(ResistanceType.Poison, "HemPoison", +8));
                    break;
                    case "Zaq":
                        types.Add(new ResistanceMod(ResistanceType.Fire, "ZaqFire", +8));
                        types.Add(new ResistanceMod(ResistanceType.Cold, "ZaqCold", +8));
                    break;
                    case "Vax":
                        types.Add(new ResistanceMod(ResistanceType.Poison,"VaxPoison", +8));
                        types.Add(new ResistanceMod(ResistanceType.Energy, "VaxEnergy", +8));
                    break;
                    case "Doth":
                        types.Add(new ResistanceMod(ResistanceType.Poison, "DothPoison", +8));
                        types.Add(new ResistanceMod(ResistanceType.Cold, "DothCold", +8));
                    break;
                    case "Zet":
                        types.Add(new ResistanceMod(ResistanceType.Physical, "ZetPhysical", +8));
                        types.Add(new ResistanceMod(ResistanceType.Poison, "ZetPoison", +8));
                    break;
                    case "Mar":
                        types.Add(new ResistanceMod(ResistanceType.Physical,"MarPhysical", +8));
                        types.Add(new ResistanceMod(ResistanceType.Fire, "MarFire", +8));
                    break;
                    case "Leq":
                        types.Add(new ResistanceMod(ResistanceType.Physical, "LeqPhysical", +8));
                        types.Add(new ResistanceMod(ResistanceType.Energy, "LeqEnergy", +8));
                    break;
                    case "Pax":
                        types.Add(new ResistanceMod(ResistanceType.Physical, "PaxPhysical", +8));
                        types.Add(new ResistanceMod(ResistanceType.Cold, "PaxCold",+8));
                    break;
                }

            }
            return types;
        }
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ModernUO.Serialization;
using Org.BouncyCastle.Asn1.Cms;
using Server.Collections;
using Server.Factions;
using Server.Misc;
using Server.Mobiles;
using Server.Pantheon;
using Server.Regions;
using Server.Spells;
using Server.Talent;

namespace Server.Items
{
    public enum ShrineType
    {
        None,
        Combat,
        Protection,
        Poison,
        Fire,
        Cold,
        Energy,
        Blessed,
        Cursed,
        Lunar,
        Solar,
        Void,
        Nature,
        Death,
        Life,
        Neophyte,
        Throne,
        Sacrifice,
        Wasp,
        Leech,
        Lion,
        Fountain,
        Hammer,
        Acid,
        Guardian,
        Famine,
        Harvest,
        Sanguine,
        Mythic,
        Vengeance,
        Silence,
        Weakness,
        Adventure
    }

    [SerializationGenerator(0, false)]
    public partial class Shrine : StoneAnkh
    {
        public const string ShrineEffectModName = "ShrineEffect";
        public const string ShrineEmpty = "This shrine appears devoid of any pantheon magic.";

        [SerializableField(0)]
        private static string _typeString;

        [SerializableField(1, getter: "private")]
        private static bool _activated;

        [SerializableField(2, getter: "private")]
        private Item _item;

        [SerializableField(3)]
        private int _timeLeft;

        public TimerExecutionToken _shrineEffectOverTimeToken;
        public override TimeSpan DecayTime => Timeout.InfiniteTimeSpan;

        public PlayerMobile Player { get; set; }

        public ResistanceMod ResMod { get; set; }

        public List<DefaultSkillMod> SkillMods { get; set; }

        public StatMod StatsMod { get; set; }

        public int InvastionTicks { get; set; }

        public List<Moongate> Gates { get; set; }

        public List<Mobile> Invaders { get; set; }
        public Region Region => Map == null ? Map.Internal.DefaultRegion : Map.DefaultRegion;

        [Constructible]
        public Shrine() : base(Utility.RandomBool())
        {
            Movable = false;
            _activated = false;
            var shrineTypes = Enum.GetValues(typeof(ShrineType));
            var randomShrineType = ShrineType.None;
            while (randomShrineType is ShrineType.None)
            {
                randomShrineType = (ShrineType)shrineTypes.GetValue(Utility.Random(shrineTypes.Length))!;
            }
            _typeString = randomShrineType.ToString();
        }

        public override int LabelNumber => 1116797; // Pantheon Shrine

        public void CheckTimeLeft()
        {
            TimeLeft--;
            Player.ShrineTimeLeft = TimeLeft;
            if (TimeLeft <= 0)
            {
                RemoveShrineEffect();
            }
            else
            {
                Timer.StartTimer(TimeSpan.FromMinutes(1), CheckTimeLeft);
            }
        }

        public override void OnComponentUsed(AddonComponent c, Mobile from)
        {
            if (!_activated && InRange(from.Location, 2))
            {
                TimeLeft = TimeLeft > 0 ? TimeLeft : 15;
                Player = (PlayerMobile)from;
                if (Player.mShrineType is ShrineType.None || Player.ShrineTimeLeft == 0)
                {
                    if (GetShrineType() is ShrineType.Mythic)
                    {
                        TimeLeft = 30;
                    }
                    Activate();
                }
                else
                {
                    Player.SendMessage("You have an active shrine effect already in place.");
                }
            }
        }

        public static bool ValidShrineAnchor(Region region, Type anchorType) => !Array.Exists(OppositionGroup.SeaCreatures, type => type == anchorType) && !region.IsPartOf<GuardedRegion>();

        public static int GetShrineRange(Region region) => region.IsPartOf<DungeonRegion>() ? 75 : 300;

        public static void CreateShrine(Point3D location, Region region, Type anchorType, Map map, bool ignoreValidationCheck = false)
        {
            var valid = ignoreValidationCheck || ValidShrineAnchor(region, anchorType);
            if (valid)
            {
                int range = GetShrineRange(region);
                var items = map.GetItemsInRange(location, range);
                var nearbyShrine = false;
                foreach (var item in items)
                {
                    if (item is Shrine { Activated: false })
                    {
                        nearbyShrine = true;
                        break;
                    }
                }

                if (!nearbyShrine)
                {
                    Shrine shrine = new Shrine();
                    Point3D shrineLocation = map.GetRandomNearbyLocation(location, 3, 2, 4, 1);
                    shrine.MoveToWorld(shrineLocation, map);
                }
            }
        }

        private void RemoveShrineEffect()
        {
            if (Player is not null)
            {
                Player.ShrineTimeLeft = 0;
                Player.mShrineType = ShrineType.None;
                if (ResMod is not null)
                {
                    Player.RemoveResistanceMod(ResMod);
                }

                if (SkillMods is not null)
                {
                    foreach (var skillMod in SkillMods)
                    {
                        Player.RemoveSkillMod(skillMod);
                    }
                }
                if (Player.MergedTalents is not null)
                {
                    Player.MergedTalents = null;
                }
            }
            if (Gates is not null)
            {
                foreach (var gate in Gates)
                {
                    gate.Delete();
                }
            }
            if (Invaders is not null)
            {
                foreach (var invader in Invaders)
                {
                    invader.Delete();
                }
            }
            RespawnShrine();
            _shrineEffectOverTimeToken.Cancel();
            _item?.Delete();
            Delete();
        }

        public void RespawnShrine()
        {
            using var list = PooledRefList<Mobile>.Create();
            foreach(var mobile in Map.GetMobilesInRange(Location, GetShrineRange(Region)))
            {
                list.Add(mobile);
            }
            if (list.Count > 0)
            {
                var mobile = list[Utility.Random(list.Count)];
                int attempts = 0;
                bool validAnchor = ValidShrineAnchor(mobile.Region, mobile.GetType());
                bool attemptCreation = validAnchor;
                while (!validAnchor)
                {
                    mobile = list[Utility.Random(list.Count)];
                    attemptCreation = ValidShrineAnchor(mobile.Region, mobile.GetType());
                    validAnchor = attemptCreation;
                    attempts++;
                    if (attempts >= 100)
                    {
                        validAnchor = true;
                    }
                }
                if (attemptCreation)
                {
                    CreateShrine(mobile.Location, mobile.Region, mobile.GetType(), mobile.Map, true);
                }
            }
        }

        private void CheckSetMessage(ref string message, string alternativeMessage)
        {
            if (message.Length == 0)
            {
                message = alternativeMessage;
            }
        }

        public void AlterMana(bool drain, int sound)
        {
            int percentage = AOS.Scale(Player.ManaMax, 30);
            int alteration = (drain) ? percentage * -1 : percentage;
            if (Player.Mana - alteration < 0 && drain)
            {
                Player.Mana = 1;
            }
            else if (Player.Mana + alteration > Player.ManaMax && !drain)
            {
                Player.Mana = Player.ManaMax;
            }
            else
            {
                // if its negative number it will decrease
                Player.Mana += alteration;
            }
            Player.FixedParticles(0x3779, 10, 15, 5004, EffectLayer.Head);
            Player.SendSound(sound);
            Timer.StartTimer(TimeSpan.FromSeconds(7), () => AlterMana(drain, sound), out _shrineEffectOverTimeToken);
        }
        public static ShrineType ShrineTypeFromString(string typeString) => (ShrineType)Enum.Parse(typeof(ShrineType), typeString, true);

        public void AlterStamina(bool drain, int sound)
        {
            int percentage = AOS.Scale(Player.StamMax, 30);
            int alteration = (drain) ? percentage * -1 : percentage;
            if (Player.Stam - alteration < 0 && drain)
            {
                Player.Stam = 1;
            }
            else if (Player.Stam + alteration > Player.StamMax && !drain)
            {
                Player.Stam = Player.StamMax;
            }
            else
            {
                // if its negative number it will decrease
                Player.Stam += alteration;
            }
            Player.FixedParticles(0x3779, 10, 15, 5002, EffectLayer.Head);
            Player.SendSound(sound);
            Timer.StartTimer(TimeSpan.FromSeconds(7), () => AlterStamina(drain, sound), out _shrineEffectOverTimeToken);
        }

        public void AlterWearables(bool repair)
        {
            if (Player.HeadArmor is BaseArmor headArmor)
            {
                headArmor.HitPoints = repair ? headArmor.MaxHitPoints : 0;
            }
            if (Player.ChestArmor is BaseArmor chestArmor)
            {
                chestArmor.HitPoints = repair ? chestArmor.MaxHitPoints : 0;
            }
            if (Player.ArmsArmor is BaseArmor armsArmor)
            {
                armsArmor.HitPoints = repair ? armsArmor.MaxHitPoints : 0;
            }
            if (Player.HandArmor is BaseArmor handArmor)
            {
                handArmor.HitPoints = repair ? handArmor.MaxHitPoints : 0;
            }
            if (Player.LegsArmor is BaseArmor legsArmor)
            {
                legsArmor.HitPoints = repair ? legsArmor.MaxHitPoints : 0;
            }
            if (Player.NeckArmor is BaseArmor neckArmor)
            {
                neckArmor.HitPoints = repair ? neckArmor.MaxHitPoints : 0;
            }
            if (Player.FindItemOnLayer(Layer.TwoHanded) is BaseShield baseShield)
            {
                baseShield.HitPoints = repair ? baseShield.MaxHitPoints : 0;
            }
            if (Player.FindItemOnLayer(Layer.TwoHanded) is BaseWeapon baseWeapon)
            {
                baseWeapon.HitPoints = repair ? baseWeapon.MaxHitPoints : 0;
            } else if (Player.FindItemOnLayer(Layer.OneHanded) is BaseWeapon oneHandedWeapon)
            {
                oneHandedWeapon.HitPoints = repair ? oneHandedWeapon.MaxHitPoints : 0;
            }
        }

        public void PlaySlimeSound()
        {
            switch (Utility.RandomMinMax(1, 4))
            {
                case 1:
                    Player.SendSound(0x1C8);
                    break;
                case 2:
                    Player.SendSound(0x1C9);
                    break;
                case 3:
                    Player.SendSound(0x1CA);
                    break;
                case 4:
                    Player.SendSound(0x1CB);
                    break;
                default:
                    Player.SendSound(0x1CC);
                    break;
            }
        }

        public void AlterHealth()
        {
            int percentage = AOS.Scale(Player.Hits, 5);
            if (Utility.Random(100) < 65)
            {
                Player.Damage(percentage);
            }
            else
            {
                Player.Heal(percentage);
            }
            var blood = new Blood { ItemID = Utility.Random(0x122A, 5) };
            blood.MoveToWorld(Player.Location, Player.Map);
            Player.FixedParticles(0x374A, 10, 15, 5054, EffectLayer.Head);
            Player.SendSound(0x1F9);
            Timer.StartTimer(TimeSpan.FromSeconds(10), AlterHealth, out _shrineEffectOverTimeToken);
        }

        public void SpawnGate(Moongate gate, Point3D potentialPoint)
        {
            gate.MoveToWorld(potentialPoint, Player.Map);
            Effects.PlaySound(potentialPoint, Player.Map, 0x20E);
        }

        public void SpawnInvaders()
        {
            if (Invaders?.Count == 0 || Invaders is null)
            {
                Invaders = Deity.FindGuardians(Player, 3, 1, true);
            }
            if (Invaders.Count > 0)
            {
                foreach (var gate in Gates)
                {
                    if (Invaders[Utility.Random(Invaders.Count)] is BaseCreature invader)
                    {
                        invader.SetLevel();
                        if (!MonsterBuff.CannotBeAltered(invader))
                        {
                            MonsterBuff.BalanceCreatureAgainstMobile(invader, Player);
                        }
                        if (Utility.Random(100) < 10)
                        {
                            if (InvastionTicks > 4)
                            {
                                invader.IsHeroic = true;
                            }
                            else
                            {
                                invader.IsVeteran = true;
                            }
                        }
                        Point3D invasionPoint = new Point3D(
                            gate.Location.X + Utility.RandomMinMax(-1, 1), gate.Y + Utility.RandomMinMax(-1, 1), gate.Z
                        );
                        invader.MoveToWorld(invasionPoint, gate.Map);
                        Effects.PlaySound(invasionPoint, gate.Map, 0x20E);
                    }
                }

                if (InvastionTicks >= 10 && Invaders[Utility.Random(Invaders.Count)] is BaseCreature boss)
                {
                    int buffs = Player.Level / 10;
                    if (buffs <= 0)
                    {
                        buffs = 1;
                    } else if (buffs > 4)
                    {
                        buffs = 4;
                    }
                    MonsterBuff.RandomMonsterBuffs(boss, buffs);
                    Moongate bossGate = Gates[Utility.Random(Gates.Count)];
                    if (bossGate is not null)
                    {
                        boss.MoveToWorld(bossGate.Location, bossGate.Map);
                    }
                    else
                    {
                        boss.Delete();
                    }
                }
                InvastionTicks++;
            }
            Timer.StartTimer(TimeSpan.FromSeconds(Utility.RandomMinMax(30, 50)), SpawnInvaders, out _shrineEffectOverTimeToken);
        }

        public void InvasionEffect()
        {
            if (!Deleted)
            {
                Point3D randomLocation = Map.GetRandomNearbyLocation(Location, 8);
                using var list = PooledRefList<Mobile>.Create();
                foreach(var mobile in Map.GetMobilesInRange(randomLocation, 1))
                {
                    list.Add(mobile);
                }
                if (list.Count > 0)
                {
                    Mobile entity = list[Utility.Random(list.Count)];
                    Effects.SendBoltEffect(entity);
                    entity.Damage(Utility.RandomMinMax(2, 10));
                }
                else
                {
                    Entity entity = new Entity(
                        Serial.Zero,
                        new Point3D(randomLocation.X, randomLocation.Y, randomLocation.Z),
                        Map
                    );
                    Effects.SendBoltEffect(new Entity(Serial.Zero, new Point3D(randomLocation.X, randomLocation.Y, randomLocation.Z), Map));
                    entity.Delete();
                }
                Timer.StartTimer(TimeSpan.FromSeconds(Utility.RandomMinMax(5, 20)), InvasionEffect);
            }
        }

        public void StartInvasionEvent()
        {
            Gates = new List<Moongate>();
            for (int i = 0; i < 4; i++)
            {
                Moongate gate = new Moongate(false);
                Point3D potentialPoint = Map.GetRandomNearbyLocation(Location, 6, 3);
                Timer.StartTimer(TimeSpan.FromSeconds(Utility.RandomMinMax(3, 10)), () => SpawnGate(gate, potentialPoint));
                Gates.Add(gate);
            }
            InvastionTicks = 0;
            Timer.StartTimer(TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10)), SpawnInvaders, out _shrineEffectOverTimeToken);
            Timer.StartTimer(TimeSpan.FromSeconds(Utility.RandomMinMax(5, 20)), InvasionEffect);
        }

        public ShrineType GetShrineType() => (ShrineType)Enum.Parse(typeof(ShrineType), _typeString, true);

        public void Activate()
        {
            _activated = true;
            string message = "";
            // int minutes = ShrineEffectTimeInMinutes;
            if (Utility.Random(50) < 1)
            {
                CheckSetMessage(ref message, ShrineEmpty);
            }
            else
            {
                Player.mShrineType = GetShrineType();
                Player.ShrineTimeLeft = TimeLeft;
                bool opposingAlignmentCheck = ShrineAlignmentEnemyCheck(Player, GetShrineType());
                if (opposingAlignmentCheck)
                {
                    message = "You are overcome with harmful energies from your enemy pantheon.";
                    Player.Damage(AOS.Scale(Player.HitsMax, 20));
                }

                switch (GetShrineType())
                {
                    case ShrineType.Adventure:
                        CheckSetMessage(ref message, "Adventure awaits.");
                        Deity.Effect(Player, Deity.Alignment.Light);
                        break;
                    case ShrineType.Weakness:
                        CheckSetMessage(ref message, "A weakness overcomes your body.");
                        StatsMod = new StatMod(
                            StatType.Str,
                            $"{ShrineEffectModName}",
                            AOS.Scale(Player.Str, 40) * -1,
                            TimeSpan.FromMinutes(TimeLeft)
                        );
                        Deity.Effect(Player, Deity.Alignment.Darkness);
                        break;
                    case ShrineType.Silence:
                        CheckSetMessage(ref message, "A dominating silence bites your tongue.");
                        Deity.Effect(Player, Deity.Alignment.Darkness);
                        break;
                    case ShrineType.Vengeance:
                        CheckSetMessage(ref message, "An item of pure vengeance lay before you.");
                        switch (Utility.RandomMinMax(1, 4))
                        {
                            case 1:
                                string clothingRuneWord = SocketBonus.RandomClothingRuneWord();
                                BaseClothing baseClothing = SocketBonus.GetRuneWordClothing(new Robe(), clothingRuneWord);
                                // baseClothing.Temporary = true;
                                _item = baseClothing;
                                break;
                            case 2:
                                string jewelleryRuneWord = SocketBonus.RandomJewelleryRuneWord();
                                BaseJewel baseJewellery = SocketBonus.GetRuneWordJewellery(new GoldRing(), jewelleryRuneWord);
                                _item = baseJewellery;
                                break;
                            case 3:
                                string armorRuneWord = SocketBonus.RandomArmorRuneWord();
                                BaseArmor baseArmor = SocketBonus.GetRuneWordArmor(new LeatherArms(), armorRuneWord);
                                _item = baseArmor;
                                break;
                            case 4:
                                string shieldRuneWord = SocketBonus.RandomShieldRuneWord();
                                BaseShield baseShield = SocketBonus.GetRuneWordShield(new WoodenShield(), shieldRuneWord);
                                _item = baseShield;
                                break;
                            default:
                                string weaponRuneWord = SocketBonus.RandomWeaponRuneWord();
                                BaseWeapon baseWeapon = SocketBonus.GetRuneWordWeapon(new Longsword(), weaponRuneWord);
                                _item = baseWeapon;
                                break;
                        }

                        if (_item is not null)
                        {
                            Point3D itemLocation = Map.GetRandomNearbyLocation(Location, 1);
                            _item.MoveToWorld(itemLocation);
                            Effects.SendBoltEffect(_item);
                        }
                        break;
                    case ShrineType.Mythic:
                        CheckSetMessage(ref message, "The air around you grows energised and ominous.");
                        StartInvasionEvent();
                        break;
                    case ShrineType.Sanguine:
                        CheckSetMessage(ref message, "You have a sudden vampiric haunt around you.");
                        Timer.StartTimer(TimeSpan.FromSeconds(10), AlterHealth, out _shrineEffectOverTimeToken);
                        break;
                    case ShrineType.Harvest:
                        CheckSetMessage(ref message, "As spring rains fall, so does cheer and merriment.");
                        var food = Food.RandomFood(Utility.RandomMinMax(1, 10));
                        if (food is not null)
                        {
                            Player.Backpack?.DropItem(food);
                        }
                        var beverage = BaseBeverage.RandomBeverage(Utility.RandomMinMax(1, 10));
                        if (beverage is not null)
                        {
                            Player.Backpack?.DropItem(beverage);
                        }
                        Player.FixedParticles(0, 10, 5, 2003, EffectLayer.RightHand);
                        Player.SendSound(0x1E2);
                        break;
                    case ShrineType.Famine:
                        CheckSetMessage(ref message, "Famine and pestilence clouds your judgement.");
                        if (Player.Backpack is not null)
                        {
                            Container.FindItemsByTypeEnumerator<Item> consumables = Player.Backpack.FindItemsByType(typeof(Food));
                            Container.FindItemsByTypeEnumerator<Item> drinks = Player.Backpack.FindItemsByType(typeof(BaseBeverage));
                            foreach (var consumable in consumables)
                            {
                                if (consumable.Amount > 1)
                                {
                                    consumable.Amount /= 2;
                                }
                            }
                            int amount = Utility.RandomMinMax(3,8);
                            int count = 0;
                            foreach (var drink in drinks)
                            {
                                drink.Delete();
                                count++;
                                if (count >= amount)
                                {
                                    break;
                                }
                            }
                        }
                        Player.Hunger = Utility.RandomMinMax(-4, -2);
                        Player.Thirst = Utility.RandomMinMax(-4, -2);
                        Player.SendSound(Player.Female ? 0x32D : 0x43F);
                        break;
                    case ShrineType.Guardian:
                        List<Mobile> guardians = Deity.FindGuardians(Player, 1, 1, true, false);
                        if (guardians.Count > 0)
                        {
                            BaseCreature guardian = guardians[Utility.Random(guardians.Count)] as BaseCreature;
                            if (guardian is not null)
                            {
                                CheckSetMessage(ref message, "A champion from the pantheon has come to serve you for a brief time.");
                                SpellHelper.Summon(guardian, Player, 0x215, TimeSpan.FromMinutes(TimeLeft), false, false);
                                guardian.FixedParticles(0x376A, 9, 32, 0x13AF, EffectLayer.Waist);
                            }
                        }
                        else
                        {
                            CheckSetMessage(ref message, ShrineEmpty);
                        }
                        break;
                    case ShrineType.Acid:
                        CheckSetMessage(ref message, "The wheel of time corrodes your adornments.");
                        AlterWearables(false);
                        PlaySlimeSound();
                        break;
                    case ShrineType.Hammer:
                        CheckSetMessage(ref message, "The smith of the pantheon lends you his hand.");
                        AlterWearables(true);
                        Player.SendSound(0x2A);
                        break;
                    case ShrineType.Lion:
                        CheckSetMessage(ref message, "A roaring vigor has filled your body and soul.");
                        int sound = Utility.RandomMinMax(1, 4) switch
                        {
                            1 => 0x0B2,
                            2 => 0x0B3,
                            3 => 0x0B4,
                            4 => 0x0B5,
                            _ => 0x0B6
                        };
                        Timer.StartTimer(TimeSpan.FromSeconds(7), () => AlterStamina(false, sound), out _shrineEffectOverTimeToken);
                        break;
                    case ShrineType.Fountain:
                        CheckSetMessage(ref message, "A flowing refreshment fills your entire being.");
                        Timer.StartTimer(TimeSpan.FromSeconds(7), () => AlterMana(false, 0x011), out _shrineEffectOverTimeToken);
                        Effects.SendMovingParticles(
                            this,
                            Player,
                            0x2255,
                            1,
                            0,
                            false,
                            false,
                            13,
                            3,
                            9501,
                            1,
                            0,
                            EffectLayer.Head,
                            0x100
                        );
                        break;
                    case ShrineType.Leech:
                        CheckSetMessage(ref message, "A feast upon the endurance of your body and soul.");
                        Timer.StartTimer(TimeSpan.FromSeconds(7), () => AlterStamina(true, 0x1FB), out _shrineEffectOverTimeToken);
                        break;
                    case ShrineType.Wasp:
                        CheckSetMessage(ref message, "A sting upon the focus of your mind.");
                        Timer.StartTimer(TimeSpan.FromSeconds(7), () => AlterMana(true, 0x1E4), out _shrineEffectOverTimeToken);
                        break;
                    case ShrineType.Sacrifice:
                        CheckSetMessage(ref message, "Victory comes with sacrifice.");
                        Deity.DestroyItem(Player);
                        Player.FixedParticles(0x3779, 10, 15, 5009, EffectLayer.Waist);
                        Player.SendSound(0x040);
                        break;
                    case ShrineType.Throne:
                        CheckSetMessage(ref message, "The gods have granted you a temporary power.");
                        BaseTalent randomTalent = BaseTalent.RandomTalent();
                        randomTalent.Level = Utility.RandomMinMax(1, 2);
                        randomTalent.IgnoreRequirements = true;
                        ConcurrentDictionary<Type, BaseTalent> mergedTalents = new ConcurrentDictionary<Type, BaseTalent>(Player.Talents);
                        if (mergedTalents.TryGetValue(randomTalent.GetType(), out var existingTalent))
                        {
                            existingTalent.Level++;
                            mergedTalents.AddOrUpdate(randomTalent.GetType(), existingTalent, (t, bt) => existingTalent);
                        }
                        else
                        {
                            mergedTalents.TryAdd(randomTalent.GetType(), randomTalent);
                        }
                        Player.MergedTalents = mergedTalents;
                        Player.SendSound(0x5C1);
                        break;
                    case ShrineType.Neophyte:
                        if (Player.NonCraftExperience + Player.CraftExperience + Player.RangerExperience > 1000)
                        {
                            CheckSetMessage(ref message, "The experienced made during your path deceives you. A silver coin of remembrance is all you have.");
                            Player.CraftExperience = AOS.Scale(Player.CraftExperience, 90);
                            Player.NonCraftExperience = AOS.Scale(Player.NonCraftExperience, 90);
                            Player.RangerExperience = AOS.Scale(Player.RangerExperience, 90);
                            Player.Backpack?.DropItem(new Silver());
                            Deity.Effect(Player, Deity.Alignment.Darkness);
                        }
                        else
                        {
                            CheckSetMessage(ref message, ShrineEmpty);
                        }
                        break;
                    case ShrineType.Blessed:
                        CheckSetMessage(ref message, "Your path is looked upon with favour from your pantheon.");
                        Deity.Effect(Player, Deity.Alignment.Charity);
                        break;
                    case ShrineType.Combat:
                        CheckSetMessage(ref message, "Your path echoes a brave war cry cutting fear into your enemies.");
                        Deity.Effect(Player, Deity.Alignment.Light);
                        break;
                    case ShrineType.Protection:
                        CheckSetMessage(ref message,"Your path stands firm with a mountainous resolve.");
                        Deity.Effect(Player, Deity.Alignment.Charity);
                        break;
                    case ShrineType.Poison:
                        ResMod = new ResistanceMod(ResistanceType.Poison, ShrineEffectModName, 100);
                        CheckSetMessage(ref message,"Your path bubbles with a toxic putrefaction.");
                        Deity.Effect(Player, Deity.Alignment.Greed);
                        break;
                    case ShrineType.Fire:
                        ResMod = new ResistanceMod(ResistanceType.Fire, ShrineEffectModName, 100);
                        CheckSetMessage(ref message,"Your path flickers brightly with blazing vengeance.");
                        Deity.Effect(Player, Deity.Alignment.Chaos);
                        break;
                    case ShrineType.Cold:
                        ResMod = new ResistanceMod(ResistanceType.Cold, ShrineEffectModName, 100);
                        CheckSetMessage(ref message,"Your path chills with a hardened resolve.");
                        Deity.Effect(Player, Deity.Alignment.Light);
                        break;
                    case ShrineType.Energy:
                        ResMod = new ResistanceMod(ResistanceType.Energy, ShrineEffectModName, 100);
                        CheckSetMessage(ref message,"Your path sparks with a lighting like tenacity.");
                        Deity.Effect(Player, Deity.Alignment.Order);
                        break;
                    case ShrineType.Cursed:
                        List<Skill> skills = new List<Skill>();
                        BaseTalent.GetTopSkills(Player, ref skills, Utility.RandomMinMax(1, 3));
                        SkillMods = BaseTalent.GetTopDefaultSkillMods(skills, -10.0, ShrineEffectModName);
                        foreach (var skillMod in SkillMods)
                        {
                            Player.AddSkillMod(skillMod);
                        }
                        StatsMod = new StatMod(
                            StatType.All,
                            ShrineEffectModName,
                            Utility.RandomMinMax(1, 5),
                            TimeSpan.FromMinutes(TimeLeft)
                        );
                        Player.AddStatMod(StatsMod);
                        CheckSetMessage(ref message,"Your path is looked upon with disdain from your pantheon.");
                        Deity.Effect(Player, Deity.Alignment.Greed);
                        break;
                    case ShrineType.Lunar:
                        CheckSetMessage(ref message,"Your path sees a doomed and demonic red moon.");
                        Deity.Effect(Player, Deity.Alignment.Chaos);
                        break;
                    case ShrineType.Solar:
                        CheckSetMessage(ref message,"Your path is illuminated by the power of the sun.");
                        Deity.Effect(Player, Deity.Alignment.Light);
                        break;
                    case ShrineType.Void:
                        Deity.BeginChallenge(Player, 1, 1);
                        CheckSetMessage(ref message,"Your path is met with a nearby challenger seeking your demise.");
                        Deity.Effect(Player, Deity.Alignment.Darkness);
                        break;
                    case ShrineType.Nature:
                        CheckSetMessage(ref message,"Your path blossoms with the peace of the wilderness.");
                        Deity.Effect(Player, Deity.Alignment.Charity);
                        break;
                    case ShrineType.Death:
                        CheckSetMessage(ref message,"Your path is urging you to destroy that which life holds dear.");
                        Deity.Effect(Player, Deity.Alignment.Darkness);
                        break;
                    case ShrineType.Life:
                        CheckSetMessage(ref message,"Your path is urging you to protect and serve.");
                        Deity.Effect(Player, Deity.Alignment.Light);
                        break;
                    case ShrineType.None:
                        CheckSetMessage(ref message, "Your soul feels lonely");
                        break;
                }
                if (ResMod is not null && !opposingAlignmentCheck)
                {
                    Player.AddResistanceMod(ResMod);
                }
            }
            Player.Say(message);
            Timer.StartTimer(TimeSpan.FromMinutes(1), CheckTimeLeft);
        }

        public static bool ShrineAlignmentEnemyCheck(Mobile from, ShrineType shrineType) =>
            shrineType is ShrineType.Death && Deity.AlignmentCheck(from, Deity.Alignment.Light, true)
            ||
            shrineType is ShrineType.Life && Deity.AlignmentCheck(from, Deity.Alignment.Darkness, true)
            ||
            shrineType is ShrineType.Solar && Deity.AlignmentCheck(from, Deity.Alignment.Chaos, true)
            ||
            shrineType is ShrineType.Lunar && Deity.AlignmentCheck(from, Deity.Alignment.Order, true);


        [AfterDeserialization]
        private void AfterDeserialization()
        {

            if (_activated)
            {
                RemoveShrineEffect();
            }
        }
    }
}

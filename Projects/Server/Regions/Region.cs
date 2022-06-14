using System;
using System.Collections.Generic;
using System.Text.Json;
using Server.Json;
using Server.Network;
using Server.Targeting;

namespace Server
{
    public enum MusicName
    {
        Invalid = -1,
        OldUlt01 = 0,
        Create1,
        DragFlit,
        OldUlt02,
        OldUlt03,
        OldUlt04,
        OldUlt05,
        OldUlt06,
        Stones2,
        Britain1,
        Britain2,
        Bucsden,
        Jhelom,
        LBCastle,
        Linelle,
        Magincia,
        Minoc,
        Ocllo,
        Samlethe,
        Serpents,
        Skarabra,
        Trinsic,
        Vesper,
        Wind,
        Yew,
        Cave01,
        Dungeon9,
        Forest_a,
        InTown01,
        Jungle_a,
        Mountn_a,
        Plains_a,
        Sailing,
        Swamp_a,
        Tavern01,
        Tavern02,
        Tavern03,
        Tavern04,
        Combat1,
        Combat2,
        Combat3,
        Approach,
        Death,
        Victory,
        BTCastle,
        Nujelm,
        Dungeon2,
        Cove,
        Moonglow,
        Zento,
        TokunoDungeon,
        Taiko,
        DreadHornArea,
        ElfCity,
        GrizzleDungeon,
        MelisandesLair,
        ParoxysmusLair,
        GwennoConversation,
        GoodEndGame,
        GoodVsEvil,
        GreatEarthSerpents,
        Humanoids_U9,
        MinocNegative,
        Paws,
        SelimsBar,
        SerpentIsleCombat_U7,
        ValoriaShips,
        NoMusic = 0x1FFF
    }

    public class Region : IComparable<Region>
    {
        public static readonly int DefaultPriority = 50;

        public static readonly int MinZ = sbyte.MinValue;
        public static readonly int MaxZ = sbyte.MaxValue + 1;

        public Region(string name, Map map, int priority, params Rectangle2D[] area) : this(
            name,
            map,
            priority,
            ConvertTo3D(area)
        )
        {
        }

        public Region(string name, Map map, int priority, params Rectangle3D[] area) : this(name, map, null, area) =>
            Priority = priority;

        public Region(string name, Map map, Region parent, params Rectangle2D[] area) : this(
            name,
            map,
            parent,
            ConvertTo3D(area)
        )
        {
        }

        public Region(string name, Map map, Region parent, params Rectangle3D[] area)
        {
            Name = name;
            Map = map;
            Parent = parent;
            Area = area;
            Dynamic = true;
            Music = DefaultMusic;

            if (Parent == null)
            {
                ChildLevel = 0;
                Priority = DefaultPriority;
            }
            else
            {
                ChildLevel = Parent.ChildLevel + 1;
                Priority = Parent.Priority;
            }
        }

        public Region(DynamicJson json, JsonSerializerOptions options)
        {
            Map = json.GetProperty("map", options, out Map map) ? map : null;
            Parent = json.GetProperty("parent", options, out string parent) ? Find(parent, Map) : null;
            Name = json.GetProperty("name", options, out string name) ? name : null;

            Dynamic = false;

            if (Parent == null)
            {
                ChildLevel = 0;
                Priority = DefaultPriority;
            }
            else
            {
                ChildLevel = Parent.ChildLevel + 1;
                Priority = Parent.Priority;
            }

            Priority = json.GetProperty("priority", options, out int priority) ? priority : Priority;

            Area = json.GetProperty("rects", options, out List<Rectangle3D> rects)
                ? rects.ToArray()
                : Array.Empty<Rectangle3D>();

            if (Area.Length == 0)
            {
                Console.WriteLine("Empty area for region '{0}'", this);
            }

            if (json.GetProperty("go", options, out Point3D go))
            {
                GoLocation = go;
            }
            else if (Area.Length > 0)
            {
                var start = Area[0].Start;
                var end = Area[0].End;

                var x = start.X + (end.X - start.X) / 2;
                var y = start.Y + (end.Y - start.Y) / 2;

                GoLocation = new Point3D(x, y, Map?.GetAverageZ(x, y) ?? start.Z + (end.Z - start.Z) / 2);
            }

            Music = json.GetEnumProperty("music", options, out MusicName music) ? music : DefaultMusic;
        }

        public static List<Region> Regions { get; } = new();

        public static TimeSpan StaffLogoutDelay { get; set; } = TimeSpan.Zero;

        public static TimeSpan DefaultLogoutDelay { get; set; } = TimeSpan.FromMinutes(5.0);

        public string Name { get; }

        public Map Map { get; }

        public Region Parent { get; }

        public List<Region> Children { get; } = new();

        public Rectangle3D[] Area { get; }

        public Sector[] Sectors { get; private set; }

        public bool Dynamic { get; }

        public int Priority { get; }

        public int ChildLevel { get; }

        public bool Registered { get; private set; }

        public Point3D GoLocation { get; set; }

        public MusicName Music { get; set; }

        public bool IsDefault => Map.DefaultRegion == this;
        public virtual MusicName DefaultMusic => Parent?.Music ?? MusicName.Invalid;

        public int CompareTo(Region reg)
        {
            if (reg == null)
            {
                return 1;
            }

            // Dynamic regions go first
            if (Dynamic)
            {
                if (!reg.Dynamic)
                {
                    return -1;
                }
            }
            else if (reg.Dynamic)
            {
                return 1;
            }

            var regPriority = reg.Priority;
            return Priority != regPriority ? reg.Priority - Priority : reg.ChildLevel - ChildLevel;
        }

        // This is not optimized. Use sparingly
        public static Region Find(string name, Map map, bool insensitive = false)
        {
            if (insensitive)
            {
                name = name.ToLower();
            }

            for (var i = 0; i < Regions.Count; i++)
            {
                var region = Regions[i];
                if (region.Map != map)
                {
                    continue;
                }

                var rName = region.Name;
                if (insensitive)
                {
                    rName = rName.ToLower();
                }

                if (rName == name)
                {
                    return region;
                }
            }

            return null;
        }

        public static Region Find(Point3D p, Map map)
        {
            if (map == null)
            {
                return Map.Internal.DefaultRegion;
            }

            var sector = map.GetSector(p);
            var list = sector.RegionRects;

            for (var i = 0; i < list.Count; ++i)
            {
                var regRect = list[i];

                if (regRect.Contains(p))
                {
                    return regRect.Region;
                }
            }

            return map.DefaultRegion;
        }

        public static Rectangle3D ConvertTo3D(Rectangle2D rect) =>
            new(new Point3D(rect.Start, MinZ), new Point3D(rect.End, MaxZ));

        public static Rectangle3D[] ConvertTo3D(Rectangle2D[] rects)
        {
            var ret = new Rectangle3D[rects.Length];

            for (var i = 0; i < ret.Length; i++)
            {
                ret[i] = ConvertTo3D(rects[i]);
            }

            return ret;
        }

        public void Register()
        {
            if (Registered)
            {
                return;
            }

            OnRegister();

            Registered = true;

            if (Parent != null)
            {
                Parent.Children.Add(this);
                Parent.OnChildAdded(this);
            }

            Regions.Add(this);

            Map.RegisterRegion(this);

            var sectors = new List<Sector>();

            for (var i = 0; i < Area.Length; i++)
            {
                var rect = Area[i];

                var start = Map.Bound(new Point2D(rect.Start.X, rect.Start.Y));
                var end = Map.Bound(new Point2D(rect.End.X, rect.End.Y));

                var startSector = Map.GetSector(start);
                var endSector = Map.GetSector(end);

                for (var x = startSector.X; x <= endSector.X; x++)
                {
                    for (var y = startSector.Y; y <= endSector.Y; y++)
                    {
                        var sector = Map.GetRealSector(x, y);

                        sector.OnEnter(this, rect);

                        if (!sectors.Contains(sector))
                        {
                            sectors.Add(sector);
                        }
                    }
                }
            }

            Sectors = sectors.ToArray();
        }

        public void Unregister()
        {
            if (!Registered)
            {
                return;
            }

            OnUnregister();

            Registered = false;

            if (Children.Count > 0)
            {
                Console.WriteLine("Warning: Unregistering region '{0}' with children", this);
            }

            if (Parent != null)
            {
                Parent.Children.Remove(this);
                Parent.OnChildRemoved(this);
            }

            Regions.Remove(this);

            Map.UnregisterRegion(this);

            if (Sectors != null)
            {
                for (var i = 0; i < Sectors.Length; i++)
                {
                    Sectors[i].OnLeave(this);
                }
            }

            Sectors = null;
        }

        public bool Contains(Point3D p)
        {
            for (var i = 0; i < Area.Length; i++)
            {
                var rect = Area[i];

                if (rect.Contains(p))
                {
                    return true;
                }
            }

            return false;
        }

        // TODO: Memoize this
        public bool IsChildOf(Region region)
        {
            if (region == null)
            {
                return false;
            }

            var p = Parent;

            while (p != null)
            {
                if (p == region)
                {
                    return true;
                }

                p = p.Parent;
            }

            return false;
        }

        // TODO: Memoize this
        public T GetRegion<T>() where T : Region
        {
            var r = this;

            do
            {
                if (r is T tr)
                {
                    return tr;
                }

                r = r.Parent;
            } while (r != null);

            return null;
        }

        public Region GetRegion(Type regionType)
        {
            if (regionType == null)
            {
                return null;
            }

            var r = this;

            do
            {
                if (regionType.IsInstanceOfType(r))
                {
                    return r;
                }

                r = r.Parent;
            } while (r != null);

            return null;
        }

        public Region GetRegion(string regionName)
        {
            if (regionName == null)
            {
                return null;
            }

            var r = this;

            do
            {
                if (r.Name == regionName)
                {
                    return r;
                }

                r = r.Parent;
            } while (r != null);

            return null;
        }

        public bool IsPartOf<T>() where T : Region => GetRegion<T>() != null;

        public bool IsPartOf(Region region) => this == region || IsChildOf(region);

        public bool IsPartOf(string regionName) => GetRegion(regionName) != null;

        public virtual bool AcceptsSpawnsFrom(Region region) =>
            AllowSpawn() && (region == this || Parent?.AcceptsSpawnsFrom(region) == true);

        public List<Mobile> GetPlayers()
        {
            var list = new List<Mobile>();

            for (var i = 0; i < Sectors?.Length; i++)
            {
                var sector = Sectors[i];

                foreach (var ns in sector.Clients)
                {
                    var player = ns.Mobile;
                    if (player?.Deleted == false && player.Region.IsPartOf(this))
                    {
                        list.Add(ns.Mobile);
                    }
                }
            }

            return list;
        }

        public int GetPlayerCount()
        {
            var count = 0;

            for (var i = 0; i < Sectors?.Length; i++)
            {
                var sector = Sectors[i];

                foreach (var ns in sector.Clients)
                {
                    var player = ns.Mobile;
                    if (player?.Deleted == false && player.Region.IsPartOf(this))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public List<Mobile> GetMobiles()
        {
            var list = new List<Mobile>();

            for (var i = 0; i < Sectors?.Length; i++)
            {
                var sector = Sectors[i];

                foreach (var mobile in sector.Mobiles)
                {
                    if (mobile.Region.IsPartOf(this))
                    {
                        list.Add(mobile);
                    }
                }
            }

            return list;
        }

        public int GetMobileCount()
        {
            var count = 0;

            for (var i = 0; i < Sectors?.Length; i++)
            {
                var sector = Sectors[i];

                foreach (var mobile in sector.Mobiles)
                {
                    if (mobile.Region.IsPartOf(this))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public List<Item> GetItems()
        {
            var list = new List<Item>();

            for (var i = 0; i < Sectors?.Length; i++)
            {
                var sector = Sectors[i];

                foreach (var item in sector.Items)
                {
                    if (Find(item.Location, item.Map).IsPartOf(this))
                    {
                        list.Add(item);
                    }
                }
            }

            return list;
        }

        public int GetItemCount()
        {
            var count = 0;

            for (var i = 0; i < Sectors?.Length; i++)
            {
                var sector = Sectors[i];

                foreach (var item in sector.Items)
                {
                    if (Find(item.Location, item.Map).IsPartOf(this))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public override string ToString() => Name ?? GetType().Name;

        public virtual void OnRegister()
        {
        }

        public virtual void OnUnregister()
        {
        }

        public virtual void OnChildAdded(Region child)
        {
        }

        public virtual void OnChildRemoved(Region child)
        {
        }

        public virtual bool OnMoveInto(Mobile m, Direction d, Point3D newLocation, Point3D oldLocation) =>
            m.WalkRegion == null || AcceptsSpawnsFrom(m.WalkRegion);

        public virtual void OnEnter(Mobile m)
        {
        }

        public virtual void OnExit(Mobile m)
        {
        }

        public virtual void MakeGuard(Mobile focus)
        {
            Parent?.MakeGuard(focus);
        }

        public virtual Type GetResource(Type type) => Parent?.GetResource(type) ?? type;

        public virtual bool CanUseStuckMenu(Mobile m) => Parent?.CanUseStuckMenu(m) != false;

        public virtual void OnAggressed(Mobile aggressor, Mobile aggressed, bool criminal)
        {
            Parent?.OnAggressed(aggressor, aggressed, criminal);
        }

        public virtual void OnDidHarmful(Mobile harmer, Mobile harmed)
        {
            Parent?.OnDidHarmful(harmer, harmed);
        }

        public virtual void OnGotHarmful(Mobile harmer, Mobile harmed)
        {
            Parent?.OnGotHarmful(harmer, harmed);
        }

        public virtual void OnLocationChanged(Mobile m, Point3D oldLocation)
        {
            Parent?.OnLocationChanged(m, oldLocation);
        }

        public virtual bool OnTarget(Mobile m, Target t, object o) => Parent?.OnTarget(m, t, o) != false;

        public virtual bool OnCombatantChange(Mobile m, Mobile old, Mobile newMobile) =>
            Parent?.OnCombatantChange(m, old, newMobile) != false;

        public virtual bool AllowHousing(Mobile from, Point3D p) => Parent?.AllowHousing(from, p) != false;

        public virtual bool SendInaccessibleMessage(Item item, Mobile from) =>
            Parent?.SendInaccessibleMessage(item, from) == true;

        public virtual bool CheckAccessibility(Item item, Mobile from) => Parent?.CheckAccessibility(item, from) != false;

        public virtual bool OnDecay(Item item) => Parent?.OnDecay(item) != false;

        public virtual bool AllowHarmful(Mobile from, Mobile target) =>
            Parent?.AllowHarmful(from, target) ?? Mobile.AllowHarmfulHandler?.Invoke(from, target) ?? true;

        public virtual void OnCriminalAction(Mobile m, bool message)
        {
            if (Parent != null)
            {
                Parent.OnCriminalAction(m, message);
            }
            else if (message)
            {
                m.SendLocalizedMessage(1005040); // You've committed a criminal act!!
            }
        }

        public virtual bool AllowBeneficial(Mobile from, Mobile target) =>
            Parent?.AllowBeneficial(from, target) ??
            Mobile.AllowBeneficialHandler?.Invoke(from, target) ?? true;

        public virtual void OnBeneficialAction(Mobile helper, Mobile target)
        {
            Parent?.OnBeneficialAction(helper, target);
        }

        public virtual void OnGotBeneficialAction(Mobile helper, Mobile target)
        {
            Parent?.OnGotBeneficialAction(helper, target);
        }

        public virtual void SpellDamageScalar(Mobile caster, Mobile target, ref double damage)
        {
            Parent?.SpellDamageScalar(caster, target, ref damage);
        }

        public virtual void OnSpeech(SpeechEventArgs args)
        {
            Parent?.OnSpeech(args);
        }

        public virtual bool OnSkillUse(Mobile m, int skill) => Parent?.OnSkillUse(m, skill) != false;

        public virtual bool OnBeginSpellCast(Mobile m, ISpell s) => Parent?.OnBeginSpellCast(m, s) != false;

        public virtual void OnSpellCast(Mobile m, ISpell s)
        {
            Parent?.OnSpellCast(m, s);
        }

        public virtual bool OnResurrect(Mobile m) => Parent?.OnResurrect(m) != false;

        public virtual bool OnBeforeDeath(Mobile m) => Parent?.OnBeforeDeath(m) != false;

        public virtual void OnDeath(Mobile m)
        {
            Parent?.OnDeath(m);
        }

        public virtual bool OnDamage(Mobile m, ref int damage) => Parent?.OnDamage(m, ref damage) != false;

        public virtual bool OnHeal(Mobile m, ref int heal) => Parent?.OnHeal(m, ref heal) != false;

        public virtual bool OnDoubleClick(Mobile m, object o) => Parent?.OnDoubleClick(m, o) != false;

        public virtual bool OnSingleClick(Mobile m, object o) => Parent?.OnSingleClick(m, o) != false;

        public virtual bool AllowSpawn() => Parent?.AllowSpawn() != false;

        public virtual void AlterLightLevel(Mobile m, ref int global, ref int personal)
        {
            Parent?.AlterLightLevel(m, ref global, ref personal);
        }

        public virtual TimeSpan GetLogoutDelay(Mobile m)
        {
            if (Parent != null)
            {
                return Parent.GetLogoutDelay(m);
            }

            return m.AccessLevel > AccessLevel.Player ? StaffLogoutDelay : DefaultLogoutDelay;
        }

        internal static bool CanMove(Mobile m, Direction d, Point3D newLocation, Point3D oldLocation, Map map)
        {
            var oldRegion = m.Region;
            var newRegion = Find(newLocation, map);

            while (oldRegion != newRegion)
            {
                if (!newRegion.OnMoveInto(m, d, newLocation, oldLocation))
                {
                    return false;
                }

                if (newRegion.Parent == null)
                {
                    return true;
                }

                newRegion = newRegion.Parent;
            }

            return true;
        }

        internal static void OnRegionChange(Mobile m, Region oldRegion, Region newRegion)
        {
            if (newRegion != null && m.NetState != null)
            {
                m.CheckLightLevels(false);

                if (oldRegion == null || oldRegion.Music != newRegion.Music)
                {
                    m.NetState.SendPlayMusic(newRegion.Music);
                }
            }

            var oldR = oldRegion;
            var newR = newRegion;

            while (oldR != newR)
            {
                var oldRChild = oldR?.ChildLevel ?? -1;
                var newRChild = newR?.ChildLevel ?? -1;

                if (oldRChild >= newRChild)
                {
                    oldR?.OnExit(m);
                    oldR = oldR?.Parent;
                }

                if (newRChild >= oldRChild)
                {
                    newR?.OnEnter(m);
                    newR = newR?.Parent;
                }
            }
        }
    }
}

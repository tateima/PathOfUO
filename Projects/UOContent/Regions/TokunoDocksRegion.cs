﻿using System;

namespace Server.Regions;

public class TokunoDocksRegion : NoHousingGuardedRegion
{
    public TokunoDocksRegion(string name, Map map, int priority, params Rectangle3D[] area) : base(name, map, priority, area)
    {
    }

    public TokunoDocksRegion(string name, Map map, int priority, params Rectangle2D[] area) : base(name, map, priority, area)
    {
    }

    public TokunoDocksRegion(string name, Map map, Region parent, params Rectangle3D[] area) : base(name, map, parent, area)
    {
    }

    public TokunoDocksRegion(string name, Map map, Region parent, int priority, params Rectangle3D[] area) : base(name, map, parent, priority, area)
    {
    }

    public TokunoDocksRegion(string name, Map map, Region parent, int priority, Type guardType, params Rectangle3D[] area) : base(name, map, parent, priority, guardType, area)
    {
    }
}

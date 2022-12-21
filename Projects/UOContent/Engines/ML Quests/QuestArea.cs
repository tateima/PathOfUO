using System;

namespace Server.Engines.MLQuests
{
    public class QuestArea
    {
        public QuestArea(TextDefinition name, string region, Map forceMap = null)
        {
            Name = name;
            RegionName = region;
            ForceMap = forceMap;

            if (MLQuestSystem.Debug)
            {
                ValidationQueue<QuestArea>.Add(this);
            }
        }

        public QuestArea(TextDefinition name, Rectangle2D boundary, Map[] forceMaps)
        {
            Name = name;
            Boundary = boundary;
            ForceMaps = forceMaps;

            if (MLQuestSystem.Debug)
            {
                ValidationQueue<QuestArea>.Add(this);
            }
        }

        public TextDefinition Name { get; set; }

        public string RegionName { get; set; }

        public Rectangle2D Boundary { get; set; }

        public Map ForceMap { get; set; }

        public Map[] ForceMaps { get; set; }

        public bool Contains(Mobile mob) => Contains(mob.Region);
        public bool ContainsPoint(Mobile mob) => Contains(mob.Location, mob.Map);

        public bool Contains(Region reg)
        {
            if (reg == null || ForceMap != null && reg.Map != ForceMap)
            {
                return false;
            }

            return reg.IsPartOf(RegionName);
        }

        public bool Contains(Point3D point, Map map)
        {
            if (ForceMaps is null || map is null)
            {
                return false;
            }
            return Array.Exists(ForceMaps, forceMap => forceMap == map) && Boundary.Contains(point);
        }

        // Debug method
        public void Validate()
        {
            var found = false;

            foreach (var r in Region.Regions)
            {
                if (r.Name == RegionName && (ForceMap == null || r.Map == ForceMap))
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Console.WriteLine(
                    "Warning: QuestArea region '{0}' does not exist (ForceMap = {1})",
                    RegionName,
                    ForceMap?.ToString() ?? "-null-"
                );
            }
        }
    }
}

using System;
using System.Collections.Generic;

namespace Server.Commands.Generic
{
    public class RegionCommandImplementor : BaseCommandImplementor
    {
        public RegionCommandImplementor()
        {
            Accessors = new[] { "Region" };
            SupportRequirement = CommandSupport.Region;
            SupportsConditionals = true;
            AccessLevel = AccessLevel.GameMaster;
            Usage = "Region <command> [condition]";
            Description =
                "Invokes the command on all appropriate mobiles in your current region. Optional condition arguments can further restrict the set of objects.";
        }

        public override void Compile(Mobile from, BaseCommand command, ref string[] args, ref object obj)
        {
            try
            {
                var ext = Extensions.Parse(from, ref args);

                if (!CheckObjectTypes(from, command, ext, out var items, out var mobiles))
                {
                    return;
                }

                var reg = from.Region;

                var list = new List<object>();

                if (mobiles)
                {
                    foreach (var mob in reg.GetMobiles())
                    {
                        if (BaseCommand.IsAccessible(from, mob) && ext.IsValid(mob))
                        {
                            list.Add(mob);
                        }
                    }
                }

                if (items)
                {
                    foreach (var item in reg.GetItems())
                    {
                        if (BaseCommand.IsAccessible(from, item) && ext.IsValid(item))
                        {
                            list.Add(item);
                        }
                    }
                }

                ext.Filter(list);

                obj = list;
            }
            catch (Exception ex)
            {
                from.SendMessage(ex.Message);
            }
        }
    }
}

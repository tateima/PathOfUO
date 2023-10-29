using System;
using System.Collections.Generic;
using Server.Gumps;
using Server.Items;
using Server.Utilities;

namespace Server.Engines.MLQuests.Objectives
{
    public class KillObjective : BaseObjective
    {
        public KillObjective(
            int amount = 0, Type[] types = null, TextDefinition name = null, QuestArea area = null,
            bool dropQuest = false, List<Type> dropTypes = null, int dropChance = 0,
            int dropQuantity = 0, int dropQuantityMax = 0
        )
        {
            DesiredAmount = amount;
            AcceptedTypes = types;
            Name = name;
            Area = area;
            DropTypes = dropTypes;
            DropQuest = dropQuest;
            DropChance = dropChance;
            DropQuantity = dropQuantity;
            DropQuantityMax = dropQuantityMax;
        }

        public int DesiredAmount { get; set; }

        public Type[] AcceptedTypes { get; set; }

        public TextDefinition Name { get; set; }

        public QuestArea Area { get; set; }

        public int DropChance { get; set; }

        public int DropQuantityMax { get; set; }

        public int DropQuantity { get; set; }

        public List<Type> DropTypes { get; set; }

        public bool DropQuest { get; set; }

        public bool CheckQuestDrop(Container c)
        {
            if (DropQuest && Utility.Random(100) < DropChance && DropQuantity < DropQuantityMax)
            {
                DropQuantity++;
                foreach (var dropType in DropTypes)
                {
                    try
                    {
                        var dropped = dropType.CreateInstance<object>();
                        if (dropped is Item droppedItem)
                        {
                            droppedItem.Amount = 1;
                            c.DropItem(droppedItem);
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }

                return true;
            }

            return false;
        }

        public override void WriteToGump(Gump g, ref int y)
        {
            var amount = DesiredAmount.ToString();

            g.AddHtmlLocalized(98, y, 312, 16, 1072204, 0x15F90); // Slay
            g.AddLabel(133, y, 0x481, amount);

            if (Name.Number > 0)
            {
                g.AddHtmlLocalized(133 + amount.Length * 15, y, 190, 18, Name.Number, 0x77BF);
            }
            else if (Name.String != null)
            {
                g.AddLabel(133 + amount.Length * 15, y, 0x481, Name.String);
            }

            y += 16;

            if (Area != null)
            {
                g.AddHtmlLocalized(103, y, 312, 20, 1018327, 0x15F90); // Location

                if (Area.Name.Number > 0)
                {
                    g.AddHtmlLocalized(223, y, 312, 20, Area.Name.Number, 0xFFFFFF);
                }
                else if (Area.Name.String != null)
                {
                    g.AddLabel(223, y, 0x481, Area.Name.String);
                }

                y += 16;
            }
        }

        public override BaseObjectiveInstance CreateInstance(MLQuestInstance instance) =>
            new KillObjectiveInstance(this, instance);
    }

    public class TimedKillObjective : KillObjective
    {
        public TimedKillObjective(TimeSpan duration, int amount, Type[] types, TextDefinition name, QuestArea area = null)
            : base(amount, types, name, area) =>
            Duration = duration;

        public override bool IsTimed => true;
        public override TimeSpan Duration { get; }
    }

    public class KillObjectiveInstance : BaseObjectiveInstance
    {
        public KillObjectiveInstance(KillObjective objective, MLQuestInstance instance)
            : base(instance, objective)
        {
            Objective = objective;
            Slain = 0;
        }

        public KillObjective Objective { get; set; }

        public int Slain { get; set; }

        public override DataType ExtraDataType => DataType.KillObjective;

        public bool AddKill(Mobile mob, Type type, Container c)
        {
            var desired = Objective.DesiredAmount;

            foreach (var acceptedType in Objective.AcceptedTypes)
            {
                if (acceptedType.IsAssignableFrom(type))
                {
                    if (Objective.Area?.Contains(mob) == false && Objective.Area?.ContainsPoint(mob) == false && Objective.Area?.Contains(mob.Location, mob.Map) == false)
                    {
                        return false;
                    }
                    if (Objective.Area?.Contains(mob) == true || Objective.Area?.ContainsPoint(mob) == true || Objective.Area?.Contains(mob.Location, mob.Map) == true)
                    {
                        var pm = Instance.Player;
                        if (Objective.DropQuest && !Objective.CheckQuestDrop(c))
                        {
                            Slain--;
                        }
                        if (++Slain >= desired)
                        {
                            pm.SendLocalizedMessage(1075050); // You have killed all the required quest creatures of this type.
                        }
                        else
                        {
                            // You have killed a quest creature. ~1_val~ more left.
                            pm.SendLocalizedMessage(1075051, (desired - Slain).ToString());
                        }
                        return true;
                    }

                    return false;
                }
            }

            return false;
        }

        public override bool IsCompleted() => Slain >= Objective.DesiredAmount;

        public override void WriteToGump(Gump g, ref int y)
        {
            Objective.WriteToGump(g, ref y);

            base.WriteToGump(g, ref y);

            g.AddHtmlLocalized(103, y, 120, 16, 3000087, 0x15F90); // Total
            g.AddLabel(223, y, 0x481, Slain.ToString());
            y += 16;

            g.AddHtmlLocalized(103, y, 120, 16, 1074782, 0x15F90); // Return to
            g.AddLabel(223, y, 0x481, QuesterNameAttribute.GetQuesterNameFor(Instance.QuesterType));
            y += 16;
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(Slain);
        }
    }
}

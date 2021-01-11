using System;
using Server.Utilities;
using System.Collections.Generic;

namespace Server.Talent
{
    public class TalentSerializer
    {
        public static object Construct(Type type)
        {
            try
            {
                return type.CreateInstance<object>();
            }
            catch
            {
                return null;
            }
        }
        public static void Serialize(HashSet<BaseTalent> t, IGenericWriter writer)
        {
            if (t == null)
            {
                writer.WriteEncodedInt(0);
            }
            else
            {
                writer.WriteEncodedInt(t.Count);
                foreach (BaseTalent talent in t)
                {
                    Write(t.GetType(), BaseTalent.TalentTypes, writer);
                    writer.WriteEncodedInt(talent.Level);
                }
            }
        }
        public static void Write(Type type, Type[] referenceTable, IGenericWriter writer)
        {
            if (type == null)
            {
                writer.WriteEncodedInt(0x00);
            }
            else
            {
                for (var i = 0; i < referenceTable.Length; ++i)
                {
                    if (referenceTable[i] == type)
                    {
                        writer.WriteEncodedInt(0x01);
                        writer.WriteEncodedInt(i);
                        return;
                    }
                }
            }
        }
        public static Type ReadType(Type[] referenceTable, IGenericReader reader)
        {
            var encoding = reader.ReadEncodedInt();

            switch (encoding)
            {
                default:
                    {
                        return null;
                    }
                case 0x01: // indexed
                    {
                        var index = reader.ReadEncodedInt();

                        if (index >= 0 && index < referenceTable.Length)
                        {
                            return referenceTable[index];
                        }

                        return null;
                    }
            }
        }
    }
}

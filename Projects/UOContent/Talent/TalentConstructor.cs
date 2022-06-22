using System;
using Server.Utilities;

namespace Server.Talent
{
    public static class TalentConstructor
    {
        public static BaseTalent ConstructFromIndex(int index)
        {
            try
            {
                Type type = BaseTalent.TalentTypes[index];
                return Construct(type) as BaseTalent;
            }
            catch
            {
                return null;
            }
        }
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
    }
}

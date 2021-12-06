using System;
using Server.Utilities;

namespace Server.Talent
{
    public static class TalentConstructor
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
    }
}

using System;

namespace Server.Mobiles
{
    public static class MonsterName
    {
        public static string Generate()
        {
            return NameList.RandomName("empowered monster I") + " " + NameList.RandomName("empowered monster II") + " " + NameList.RandomName("empowered monster III");
        }
    }
}


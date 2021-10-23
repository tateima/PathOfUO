using System;
using Server.Items;
using Server.Utilities;

namespace Server.Mobiles
{
    public static class Veteran
    {
        // Buffs
        public static double GoldBuff = 1.12;
        public static double HitsBuff = 1.28;
        public static double StrBuff = 1.015;
        public static double IntBuff = 1.06;
        public static double DexBuff = 1.06;
        public static double SkillsBuff = 1.06;
        public static double SpeedBuff = 1.06;
        public static double FameBuff = 1.11;
        public static double KarmaBuff = 1.11;
        public static int DamageBuff = 3;
        public static void Convert(BaseCreature bc)
        {
            if (bc.IsVeteran)
            {
                return;
            }
            MonsterBuff.Convert(bc, GoldBuff, HitsBuff, StrBuff, IntBuff, DexBuff, SkillsBuff, SpeedBuff, FameBuff, KarmaBuff, DamageBuff);
            MonsterBuff.AddLoot(bc, 3);
        }
        public static void UnConvert(BaseCreature bc)
        {
            if (!bc.IsVeteran)
            {
                return;
            }
            MonsterBuff.UnConvert(bc, GoldBuff, HitsBuff, StrBuff, IntBuff, DexBuff, SkillsBuff, SpeedBuff, FameBuff, KarmaBuff, DamageBuff);
        }
    }
}

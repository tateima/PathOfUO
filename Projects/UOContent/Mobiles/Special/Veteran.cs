using System;
using Server.Items;
using Server.Utilities;

namespace Server.Mobiles
{
    public static class Veteran
    {
        // Buffs
        public static double GoldBuff = 1.20;
        public static double HitsBuff = 4.00;
        public static double StrBuff = 1.020;
        public static double IntBuff = 1.10;
        public static double DexBuff = 1.10;
        public static double SkillsBuff = 1.10;
        public static double SpeedBuff = 1.10;
        public static double FameBuff = 1.11;
        public static double KarmaBuff = 1.11;
        public static int DamageBuff = 4;
        public static void Convert(BaseCreature bc)
        {
            if (bc.IsVeteran)
            {
                return;
            }
            MonsterBuff.Convert(bc, GoldBuff, HitsBuff, StrBuff, IntBuff, DexBuff, SkillsBuff, SpeedBuff, FameBuff, KarmaBuff, DamageBuff);
            MonsterBuff.AddLoot(bc);
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

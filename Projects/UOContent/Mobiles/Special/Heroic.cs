using System;
using Server.Items;
using Server.Utilities;

namespace Server.Mobiles
{
    public static class Heroic
    {
        // Buffs
        public static double GoldBuff = 1.30;
        public static double HitsBuff = 2.5;
        public static double StrBuff = 1.025;
        public static double IntBuff = 1.20;
        public static double DexBuff = 1.20;
        public static double SkillsBuff = 1.20;
        public static double SpeedBuff = 1.20;
        public static double FameBuff = 1.20;
        public static double KarmaBuff = 1.20;
        public static int DamageBuff = 4;
        public static void Convert(BaseCreature bc)
        {
            if (bc.IsHeroic)
            {
                return;
            }
            MonsterBuff.Convert(bc, GoldBuff, HitsBuff, StrBuff, IntBuff, DexBuff, SkillsBuff, SpeedBuff, FameBuff, KarmaBuff, DamageBuff);
            MonsterBuff.AddLoot(bc, Utility.Random(3,4));
        }
        public static void UnConvert(BaseCreature bc)
        {
            if (!bc.IsHeroic)
            {
                return;
            }
            MonsterBuff.UnConvert(bc, GoldBuff, HitsBuff, StrBuff, IntBuff, DexBuff, SkillsBuff, SpeedBuff, FameBuff, KarmaBuff, DamageBuff);
        }
    }
}

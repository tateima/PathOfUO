using System;
using Server.Items;
using Server.Utilities;

namespace Server.Mobiles
{
    public static class Heroic
    {
        // Buffs
        public static double GoldBuff = 1.30;
        public static double HitsBuff = 3;
        public static double StrBuff = 2.5;
        public static double IntBuff = 1.75;
        public static double DexBuff = 1.75;
        public static double SkillsBuff = 1.20;
        public static double SpeedBuff = 1.20;
        public static double FameBuff = 1.20;
        public static double KarmaBuff = 1.20;
        public static int DamageBuff = 6;
        public static void Convert(BaseCreature bc)
        {
            if (bc.IsHeroic)
            {
                return;
            }
            bc.SetResistance(ResistanceType.Cold, bc.BaseColdResistance + 10);
            bc.SetResistance(ResistanceType.Poison, bc.BasePoisonResistance + 10);
            bc.SetResistance(ResistanceType.Energy, bc.BaseEnergyResistance + 10);
            bc.SetResistance(ResistanceType.Fire, bc.BaseFireResistance + 10);
            bc.SetResistance(ResistanceType.Physical, bc.BasePhysicalResistance + 10);
            MonsterBuff.Convert(bc, GoldBuff, HitsBuff, StrBuff, IntBuff, DexBuff, SkillsBuff, SpeedBuff, FameBuff, KarmaBuff, DamageBuff);
            MonsterBuff.AddLoot(bc);
        }
        public static void UnConvert(BaseCreature bc)
        {
            if (!bc.IsHeroic)
            {
                return;
            }
            bc.SetResistance(ResistanceType.Cold, bc.BaseColdResistance - 10);
            bc.SetResistance(ResistanceType.Poison, bc.BasePoisonResistance - 10);
            bc.SetResistance(ResistanceType.Energy, bc.BaseEnergyResistance - 10);
            bc.SetResistance(ResistanceType.Fire, bc.BaseFireResistance - 10);
            bc.SetResistance(ResistanceType.Physical, bc.BasePhysicalResistance - 10);
            MonsterBuff.UnConvert(bc, GoldBuff, HitsBuff, StrBuff, IntBuff, DexBuff, SkillsBuff, SpeedBuff, FameBuff, KarmaBuff, DamageBuff);
        }
    }
}

using System;
using Server.Items;
using Server.Utilities;

namespace Server.Mobiles
{
    public static class Veteran
    {
        // Buffs
        public static double GoldBuff = 1.20;
        public static double HitsBuff = 2.5;
        public static double StrBuff = 1.020;
        public static double IntBuff = 1.10;
        public static double DexBuff = 1.10;
        public static double SkillsBuff = 1.10;
        public static double SpeedBuff = 1.10;
        public static double FameBuff = 1.11;
        public static double KarmaBuff = 1.11;
        public static int DamageBuff = 5;
        public static void Convert(BaseCreature bc)
        {
            if (bc.IsVeteran)
            {
                return;
            }
            bc.SetResistance(ResistanceType.Cold, bc.BaseColdResistance + 5);
            bc.SetResistance(ResistanceType.Poison, bc.BasePoisonResistance + 5);
            bc.SetResistance(ResistanceType.Energy, bc.BaseEnergyResistance + 5);
            bc.SetResistance(ResistanceType.Fire, bc.BaseFireResistance + 5);
            bc.SetResistance(ResistanceType.Physical, bc.BasePhysicalResistance + 5);
            MonsterBuff.Convert(bc, GoldBuff, HitsBuff, StrBuff, IntBuff, DexBuff, SkillsBuff, SpeedBuff, FameBuff, KarmaBuff, DamageBuff);
            MonsterBuff.AddLoot(bc);
        }
        public static void UnConvert(BaseCreature bc)
        {
            if (!bc.IsVeteran)
            {
                return;
            }
            bc.SetResistance(ResistanceType.Cold, bc.BaseColdResistance - 5);
            bc.SetResistance(ResistanceType.Poison, bc.BasePoisonResistance - 5);
            bc.SetResistance(ResistanceType.Energy, bc.BaseEnergyResistance - 5);
            bc.SetResistance(ResistanceType.Fire, bc.BaseFireResistance - 5);
            bc.SetResistance(ResistanceType.Physical, bc.BasePhysicalResistance - 5);
            MonsterBuff.UnConvert(bc, GoldBuff, HitsBuff, StrBuff, IntBuff, DexBuff, SkillsBuff, SpeedBuff, FameBuff, KarmaBuff, DamageBuff);
        }
    }
}

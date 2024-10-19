namespace Server.Mobiles
{
    public static class Heroic
    {
        // Buffs
        public static double GoldBuff = 2.30;
        public static double HitsBuff = 2;
        public static double StrBuff = 1.30;
        public static double IntBuff = 1.20;
        public static double DexBuff = 1.20;
        public static double SkillsBuff = 1.4;
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

            if (bc.Tamable)
            {
                if (bc.MinTameSkill < 40)
                {
                    bc.MinTameSkill = 40;
                }
                bc.MinTameSkill += 17;
            }
            bc.ControlSlots++;
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
            if (bc.Tamable)
            {
                bc.MinTameSkill -= 17;
            }
            bc.ControlSlots--;
            bc.SetResistance(ResistanceType.Cold, bc.BaseColdResistance - 10);
            bc.SetResistance(ResistanceType.Poison, bc.BasePoisonResistance - 10);
            bc.SetResistance(ResistanceType.Energy, bc.BaseEnergyResistance - 10);
            bc.SetResistance(ResistanceType.Fire, bc.BaseFireResistance - 10);
            bc.SetResistance(ResistanceType.Physical, bc.BasePhysicalResistance - 10);
            MonsterBuff.UnConvert(bc, GoldBuff, HitsBuff, StrBuff, IntBuff, DexBuff, SkillsBuff, SpeedBuff, FameBuff, KarmaBuff, DamageBuff);
        }
    }
}

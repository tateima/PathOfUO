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
        public static double IntBuff = 1.10;
        public static double DexBuff = 1.10;
        public static double SkillsBuff = 1.10;
        public static double SpeedBuff = 1.10;
        public static double FameBuff = 1.20;
        public static double KarmaBuff = 1.20;
        public static int DamageBuff = 3;
        public static void Convert(BaseCreature bc)
        {
            if (bc.IsHeroic)
            {
                return;
            }
            if (bc.Backpack != null)
            {
                Item[] goldItems = bc.Backpack.FindItemsByType(typeof(Gold));
                foreach(Item gold in goldItems)
                {
                    ((Gold)gold).Amount = (int)(((Gold)gold).Amount * GoldBuff);
                }
                if (bc.AI != AIType.AI_Animal)
                {
                    if (Utility.Random(100) <= 1)
                    {
                        switch (Utility.Random(1, 3))
                        {
                            case 1:
                                bc.AddLoot(LootPack.Average);
                                break;
                            case 2:
                                bc.AddLoot(LootPack.Rich);
                                break;
                            case 3:
                                bc.AddLoot(LootPack.Meager);
                                break;
                        }
                    }
                }
            }
            

            if (bc.HitsMaxSeed >= 0)
            {
                bc.HitsMaxSeed = (int)(bc.HitsMaxSeed * HitsBuff);
            }

            bc.RawStr = (int)(bc.RawStr * StrBuff);
            bc.RawInt = (int)(bc.RawInt * IntBuff);
            bc.RawDex = (int)(bc.RawDex * DexBuff);

            bc.Hits = bc.HitsMax;
            bc.Mana = bc.ManaMax;
            bc.Stam = bc.StamMax;

            for (var i = 0; i < bc.Skills.Length; i++)
            {
                var skill = bc.Skills[i];

                if (skill.Base > 0.0)
                {
                    skill.Base *= SkillsBuff;
                }
            }

            bc.PassiveSpeed /= SpeedBuff;
            bc.ActiveSpeed /= SpeedBuff;
            bc.CurrentSpeed = bc.PassiveSpeed;

            bc.DamageMin += DamageBuff;
            bc.DamageMax += DamageBuff;

            if (bc.Fame > 0)
            {
                bc.Fame = (int)(bc.Fame * FameBuff);
            }

            if (bc.Fame > 32000)
            {
                bc.Fame = 32000;
            }

            // TODO: Mana regeneration rate = Sqrt( buffedFame ) / 4

            if (bc.Karma != 0)
            {
                bc.Karma = (int)(bc.Karma * KarmaBuff);

                if (bc.Karma.Abs() > 32000)
                {
                    bc.Karma = 32000 * Math.Sign(bc.Karma);
                }
            }
        }
        public static void UnConvert(BaseCreature bc)
        {
            if (!bc.IsHeroic)
            {
                return;
            }

            bc.Hue = 0;

            if (bc.HitsMaxSeed >= 0)
            {
                bc.HitsMaxSeed = (int)(bc.HitsMaxSeed / HitsBuff);
            }

            bc.RawStr = (int)(bc.RawStr / StrBuff);
            bc.RawInt = (int)(bc.RawInt / IntBuff);
            bc.RawDex = (int)(bc.RawDex / DexBuff);

            bc.Hits = bc.HitsMax;
            bc.Mana = bc.ManaMax;
            bc.Stam = bc.StamMax;

            for (var i = 0; i < bc.Skills.Length; i++)
            {
                var skill = bc.Skills[i];

                if (skill.Base > 0.0)
                {
                    skill.Base /= SkillsBuff;
                }
            }

            bc.PassiveSpeed *= SpeedBuff;
            bc.ActiveSpeed *= SpeedBuff;
            bc.CurrentSpeed = bc.PassiveSpeed;

            bc.DamageMin -= DamageBuff;
            bc.DamageMax -= DamageBuff;

            if (bc.Fame > 0)
            {
                bc.Fame = (int)(bc.Fame / FameBuff);
            }

            if (bc.Karma != 0)
            {
                bc.Karma = (int)(bc.Karma / KarmaBuff);
            }
        }
    }
}

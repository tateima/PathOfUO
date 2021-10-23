using System;
using System.Collections.Generic;
using Server.Items;
using Server.Utilities;

namespace Server.Mobiles
{
    public static class MonsterBuff
    {
        private static readonly TimeSpan FastRegenRate = TimeSpan.FromSeconds(1);
        private static readonly TimeSpan CPUSaverRate = TimeSpan.FromSeconds(3);

        public static double BossGoldBuff = 1.11;
        public static double BossHitsBuff = 1.11;
        public static double BossStrBuff = 1.04;
        public static double BossIntBuff = 1.04;
        public static double BossDexBuff = 1.04;
        public static double BossSkillsBuff = 1.04;
        public static double BossSpeedBuff = 1.04;
        public static double BossFameBuff = 1.11;
        public static double BossKarmaBuff = 1.12;
        public static int BossDamageBuff = 2;

        public static double MinionGoldBuff = 1.02;
        public static double MinionHitsBuff = 1.05;
        public static double MinionStrBuff = 1.02;
        public static double MinionIntBuff = 1.02;
        public static double MinionDexBuff = 1.02;
        public static double MinionSkillsBuff = 1.02;
        public static double MinionSpeedBuff = 1.02;
        public static double MinionFameBuff = 1.04;
        public static double MinionKarmaBuff = 1.04;
        public static int MinionDamageBuff = 1;

        public static double NoBuff = 1.0;

        public static int BossHue = 0x21;
        public static int RegenerativeHue = 0x3C;
        public static int ReflectiveHue = 0x2B;
        public static int MagicResistantHue = 0x5D;
        public static int IllusionistHue = 0x143;

        public static void CheckHues(BaseCreature bc)
        {
            bc.Hue = 0;
            if (bc.IsBoss)
            {
                bc.Hue = BossHue;
            }
            else if (bc.IsReflective)
            {
                bc.Hue = ReflectiveHue;
            }
            else if (bc.IsIllusionist)
            {
                bc.Hue = IllusionistHue;
            }
            else if (bc.IsRegenerative)
            {
                bc.Hue = RegenerativeHue;
            }
            else if (bc.IsMagicResistant)
            {
                bc.Hue = MagicResistantHue;
            } 
        }

        public static void AddIllusion(BaseCreature bc, Mobile from)
        {
            BaseCreature illusion = Activator.CreateInstance(bc.GetType()) as BaseCreature;
            Point3D point = new Point3D(bc.Location.X + Utility.Random(1, 2), bc.Location.Y + Utility.Random(1, 2), bc.Y);
            illusion.MoveToWorld(point, bc.Map);
            illusion.Combatant = from;
            illusion.SetHits(Utility.Random(5, 15));
            illusion.ExperienceValue = 0;
            illusion.Hue = IllusionistHue;
            if (illusion.Backpack != null)
            {
                foreach (Item item in illusion.Backpack.Items)
                {
                    item.Delete();
                }
            }
        }

        public static void AddIllusionist(BaseCreature bc)
        {
            CheckHues(bc);
            Convert(bc, BossGoldBuff, 1.50, NoBuff, NoBuff, NoBuff, BossSkillsBuff, NoBuff, BossFameBuff, BossKarmaBuff, 0);
        }
        public static void RemoveIllusionist(BaseCreature bc)
        {
            CheckHues(bc);
        }
        public static void AddRegenerative(BaseCreature bc)
        {
            CheckHues(bc);
            new RegenerativeTimer(bc).Start();
        }
        public static void RemoveRegenerative(BaseCreature bc)
        {
            CheckHues(bc);
        }
        public static void AddReflective(BaseCreature bc)
        {
            CheckHues(bc);
        }
        public static void RemoveReflective(BaseCreature bc)
        {
            CheckHues(bc);
        }
        public static void AddMagicResistant(BaseCreature bc) {
            CheckHues(bc);
            bc.SetResistance(ResistanceType.Fire, 100);
            bc.SetResistance(ResistanceType.Cold, 100);
            bc.SetResistance(ResistanceType.Poison, 100);
            bc.SetResistance(ResistanceType.Energy, 100);
            bc.Skills.MagicResist.Base = 120.0;
        }
        public static void RemoveMagicResistant(BaseCreature bc)
        {
            CheckHues(bc);
            bc.SetResistance(ResistanceType.Fire, 0);
            bc.SetResistance(ResistanceType.Cold, 0);
            bc.SetResistance(ResistanceType.Poison, 0);
            bc.SetResistance(ResistanceType.Energy, 0);
            bc.Skills.MagicResist.Base = 0;
        }
        public static void Bossify(BaseCreature bc)
        {
            CheckHues(bc);
            Type monsterType = bc.GetType();
            int numberOfMinions = Utility.Random(3, 5);
            List<Mobile> minions = new List<Mobile>();
            for (int i = 0; i < numberOfMinions; i = i + 1)
            {
                BaseCreature minion = Activator.CreateInstance(monsterType) as BaseCreature;
                Point3D point = new Point3D(bc.Location.X + Utility.Random(2, 3), bc.Location.Y + Utility.Random(2, 3), bc.Y);
                minion.MoveToWorld(point, bc.Map);
                minion.IsMinion = true;
                minion.ControlTarget = bc;
                minion.ControlOrder = OrderType.Guard;
                Convert(minion, MinionGoldBuff, MinionHitsBuff, MinionStrBuff, MinionIntBuff, MinionDexBuff, MinionSkillsBuff, MinionSpeedBuff, MinionFameBuff, MinionKarmaBuff, MinionDamageBuff);
                AddLoot(minion, 1);
                minions.Add(minion);
            }
            bc.Minions = minions;
            bc.SetResistance(ResistanceType.Physical, 100);
            Convert(bc, BossGoldBuff, BossHitsBuff, BossStrBuff, BossIntBuff, BossDexBuff, BossSkillsBuff, BossSpeedBuff, BossFameBuff, BossKarmaBuff, BossDamageBuff);
            AddLoot(bc, Utility.Random(2, 4));
        }
        public static void AddLoot(BaseCreature bc, int lootLevel)
        {
            switch (lootLevel)
            {
                case 1:
                    bc.AddLoot(LootPack.Poor);
                    break;
                case 2:
                    bc.AddLoot(LootPack.Meager);
                    break;
                case 3:
                    bc.AddLoot(LootPack.Average);
                    break;
                case 4:
                    bc.AddLoot(LootPack.Rich);
                    break;
                case 5:
                    bc.AddLoot(LootPack.FilthyRich);
                    break;
                case 6:
                    bc.AddLoot(LootPack.UltraRich);
                    break;
                case 7:
                    bc.AddLoot(LootPack.SuperBoss);
                    break;
            }
        }
        public static void UnBossify(BaseCreature bc)
        {
            CheckHues(bc);
            if (bc.Minions != null)
            {
                foreach(Mobile minion in bc.Minions)
                {
                    minion.Delete();
                }
                bc.Minions = new List<Mobile>();
            }
            bc.SetResistance(ResistanceType.Physical, 0);
            UnConvert(bc, BossGoldBuff, BossHitsBuff, BossStrBuff, BossIntBuff, BossDexBuff, BossSkillsBuff, BossSpeedBuff, BossFameBuff, BossKarmaBuff, BossDamageBuff);
        }
        public static void Convert(BaseCreature bc, double goldBuff, double hitsBuff, double strBuff, double intBuff, double dexBuff, double skillsBuff, double speedBuff, double fameBuff, double karmaBuff, int damageBuff)
        {
            if (bc.Backpack != null)
            {
                Item[] goldItems = bc.Backpack.FindItemsByType(typeof(Gold));
                foreach(Item gold in goldItems)
                {
                    ((Gold)gold).Amount = (int)(((Gold)gold).Amount * goldBuff);
                }
            }            

            if (bc.HitsMaxSeed >= 0)
            {
                bc.HitsMaxSeed = (int)(bc.HitsMaxSeed * hitsBuff);
            }

            bc.RawStr = (int)(bc.RawStr * strBuff);
            bc.RawInt = (int)(bc.RawInt * intBuff);
            bc.RawDex = (int)(bc.RawDex * dexBuff);

            bc.Hits = bc.HitsMax;
            bc.Mana = bc.ManaMax;
            bc.Stam = bc.StamMax;

            for (var i = 0; i < bc.Skills.Length; i++)
            {
                var skill = bc.Skills[i];

                if (skill.Base > 0.0)
                {
                    skill.Base *= skillsBuff;
                }
            }

            bc.PassiveSpeed /= speedBuff;
            bc.ActiveSpeed /= speedBuff;
            bc.CurrentSpeed = bc.PassiveSpeed;

            bc.DamageMin += damageBuff;
            bc.DamageMax += damageBuff;

            if (bc.Fame > 0)
            {
                bc.Fame = (int)(bc.Fame * fameBuff);
            }

            if (bc.Fame > 32000)
            {
                bc.Fame = 32000;
            }

            // TODO: Mana regeneration rate = Sqrt( buffedFame ) / 4

            if (bc.Karma != 0)
            {
                bc.Karma = (int)(bc.Karma * karmaBuff);

                if (bc.Karma.Abs() > 32000)
                {
                    bc.Karma = 32000 * Math.Sign(bc.Karma);
                }
            }
        }
        public static void UnConvert(BaseCreature bc, double goldBuff, double hitsBuff, double strBuff, double intBuff, double dexBuff, double skillsBuff, double speedBuff, double fameBuff, double karmaBuff, int damageBuff)
        {
            bc.Hue = 0;

            if (bc.HitsMaxSeed >= 0)
            {
                bc.HitsMaxSeed = (int)(bc.HitsMaxSeed / hitsBuff);
            }

            bc.RawStr = (int)(bc.RawStr / strBuff);
            bc.RawInt = (int)(bc.RawInt / intBuff);
            bc.RawDex = (int)(bc.RawDex / dexBuff);

            bc.Hits = bc.HitsMax;
            bc.Mana = bc.ManaMax;
            bc.Stam = bc.StamMax;

            for (var i = 0; i < bc.Skills.Length; i++)
            {
                var skill = bc.Skills[i];

                if (skill.Base > 0.0)
                {
                    skill.Base /= skillsBuff;
                }
            }

            bc.PassiveSpeed *= speedBuff;
            bc.ActiveSpeed *= speedBuff;
            bc.CurrentSpeed = bc.PassiveSpeed;

            bc.DamageMin -= damageBuff;
            bc.DamageMax -= damageBuff;

            if (bc.Fame > 0)
            {
                bc.Fame = (int)(bc.Fame / fameBuff);
            }

            if (bc.Karma != 0)
            {
                bc.Karma = (int)(bc.Karma / karmaBuff);
            }
        }
        private class RegenerativeTimer : Timer
        {
            private readonly BaseCreature m_Owner;

            public RegenerativeTimer(Mobile m)
                : base(FastRegenRate, FastRegenRate)
            {

                m_Owner = m as BaseCreature;
            }

            protected override void OnTick()
            {
                if (!m_Owner.Deleted && m_Owner.IsRegenerative && m_Owner.Map != Map.Internal)
                {
                    m_Owner.Hits += Utility.Random(1, 3);
                    Delay = Interval = m_Owner.HitsMax < m_Owner.HitsMax * .75 ? FastRegenRate : CPUSaverRate;
                }
                else
                {
                    Stop();
                }
            }
        }
    }
}

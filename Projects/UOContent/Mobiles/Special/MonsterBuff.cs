using System;
using System.Collections.Generic;
using System.Reflection;
using Server.Engines.Spawners;
using Server.Items;
using Server.Talent;
using Server.Network;

namespace Server.Mobiles
{
    public static class MonsterBuff
    {
        private static readonly TimeSpan FastRegenRate = TimeSpan.FromSeconds(1);
        private static readonly TimeSpan CPUSaverRate = TimeSpan.FromSeconds(3);
        private static readonly TimeSpan CorruptionRate = TimeSpan.FromSeconds(7);
        private static readonly TimeSpan ElementalDamageRate = TimeSpan.FromSeconds(8);

        public static readonly int CorruptionRange = 12;

        public static double BossGoldBuff = 1.15;
        public static double BossHitsBuff = 2.5;
        public static double BossStrBuff = 1.25;
        public static double BossIntBuff = 1.20;
        public static double BossDexBuff = 1.20;
        public static double BossSkillsBuff = 1.50;
        public static double BossSpeedBuff = 1.3;
        public static double BossFameBuff = 1.05;
        public static double BossKarmaBuff = 1.17;
        public static int BossDamageBuff = 4;
        public static double BossTameableModifier = 30.0;

        public static double MinionGoldBuff = 1.07;
        public static double MinionHitsBuff = 1.2;
        public static double MinionStrBuff = 1.07;
        public static double MinionIntBuff = 1.07;
        public static double MinionDexBuff = 1.07;
        public static double MinionSkillsBuff = 1.15;
        public static double MinionSpeedBuff = 1.02;
        public static double MinionFameBuff = 1.09;
        public static double MinionKarmaBuff = 1.09;
        public static int MinionDamageBuff = 2;

        public static double NoBuff = 1.0;

        public static int BossHue = 0x21;
        public static int CorruptorHue = 0x549;
        public static int CorruptedHue = 0x543;
        public static int RegenerativeHue = 0x3C;
        public static int ReflectiveHue = 0x2B;
        public static int MagicResistantHue = 0x5D;
        public static int IllusionistHue = 0x143;
        public static int EtherealHue = 0x4001;
        public static int FrozenHue = 0x480;
        public static int BurningHue = 0x4EC;
        public static int ElectrifiedHue = 0x4FC;
        public static int ToxicHue = 0x4F8;

        public static void CheckHues(BaseCreature bc)
        {
            bc.Hue = 0;
            if (bc.IsCorruptor || bc.IsSoulFeeder)
            {
                bc.Hue = CorruptorHue;
            } else if (bc.IsBoss || bc.IsWarlord)
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
            else if (bc.IsMagicResistant || bc.IsMagical)
            {
                bc.Hue = MagicResistantHue;
            } else if (bc.IsEthereal)
            {
                bc.Hue = EtherealHue;
            }
            else if (bc.IsCorrupted)
            {
                bc.Hue = CorruptedHue;
            }
            if (bc.IsFrozen)
            {
                bc.Hue = FrozenHue;
            }
            else if (bc.IsBurning)
            {
                bc.Hue = BurningHue;
            }
            else if (bc.IsElectrified)
            {
                bc.Hue = ElectrifiedHue;
            }
            else if (bc.IsElectrified)
            {
                bc.Hue = ElectrifiedHue;
            }
            else if (bc.IsToxic)
            {
                bc.Hue = ToxicHue;
            }
        }

        public static void AddFeederStats(BaseCreature bc)
        {
            if (bc.SoulFeeds < 10)
            {
                bc.SoulFeeds++;
                Convert(
                    bc,
                    MinionGoldBuff,
                    MinionHitsBuff,
                    MinionStrBuff,
                    MinionIntBuff,
                    MinionDexBuff,
                    MinionSkillsBuff,
                    MinionSpeedBuff,
                    MinionFameBuff,
                    MinionKarmaBuff,
                    MinionDamageBuff,
                    50,
                    3
                );
                bc.PublicOverheadMessage(MessageType.Regular, 0x0481, false, "* This creature grows in strength *");
            }
            else
            {
                AddLoot(bc);
            }
        }

        public static void AddElementalProperties(BaseCreature bc)
        {
            bc.Name = MonsterName.Generate();
            CheckHues(bc);
            Convert(bc, NoBuff, BossHitsBuff, NoBuff, NoBuff, NoBuff, BossSkillsBuff, NoBuff, NoBuff, NoBuff, BossDamageBuff, 250, 10.0);

            if (bc.IsElectrified)
            {
                bc.SetResistance(ResistanceType.Energy, 100);
            }
            else if (bc.IsBurning)
            {
                bc.SetResistance(ResistanceType.Fire, 100);
            }
            else if (bc.IsToxic)
            {
                bc.SetResistance(ResistanceType.Poison, 100);
                bc.HitPoisonOverride = Utility.RandomMinMax(1, 3) switch
                {
                    1 => Poison.Deadly,
                    2 => Poison.Lethal,
                    _ => Poison.Greater
                };
                bc.Skills.Poisoning.Base = Utility.RandomMinMax(70, 100);
            }
            else if (bc.IsFrozen)
            {
                bc.SetResistance(ResistanceType.Cold, 100);
                if (bc.Backpack != null) {
                    bc.Backpack.AddItem(new IcyHeart());
                }
            }
            AddLoot(bc);
            new ElementalDamageTimer(bc).Start();
        }
        public static void RemoveElementalProperties(BaseCreature bc)
        {
            CheckHues(bc);
            UnConvert(bc, NoBuff, BossHitsBuff, NoBuff, NoBuff, NoBuff, BossSkillsBuff, NoBuff, NoBuff, NoBuff, BossDamageBuff, 250, 10.0);
            if (bc.IsElectrified)
            {
                bc.SetResistance(ResistanceType.Energy, 0);
            }
            else if (bc.IsBurning)
            {
                bc.SetResistance(ResistanceType.Fire, 0);
            }
            else if (bc.IsToxic)
            {
                bc.SetResistance(ResistanceType.Poison, 0);
                bc.Skills.Poisoning.Base = 0;
            }
            else if (bc.IsFrozen)
            {
                bc.SetResistance(ResistanceType.Cold, 0);
                List<Item> hearts = bc.Backpack?.FindItemsByType(typeof(IcyHeart));
                if (hearts is not null)
                {
                    foreach (var heart in hearts)
                    {
                        heart.Delete();
                    }
                }
            }
        }

        public static void AddEthereal(BaseCreature bc)
        {
            bc.Name = MonsterName.Generate();
            CheckHues(bc);
            Convert(bc, NoBuff, BossHitsBuff, NoBuff, NoBuff, NoBuff, BossSkillsBuff, NoBuff, NoBuff, NoBuff, BossDamageBuff, 250, 10.0);
            AddLoot(bc);
        }
        public static void RemoveEthereal(BaseCreature bc)
        {
            CheckHues(bc);
            UnConvert(bc, NoBuff, BossHitsBuff, NoBuff, NoBuff, NoBuff, BossSkillsBuff, NoBuff, NoBuff, NoBuff, BossDamageBuff, 250, 10.0);
        }
        public static void AddCorrupted(BaseCreature bc)
        {
            CheckHues(bc);
            Convert(bc, NoBuff, MinionHitsBuff, NoBuff, NoBuff, NoBuff, MinionSkillsBuff, NoBuff, NoBuff, NoBuff, 1, 100, 5.0);

        }
        public static void RemoveCorrupted(BaseCreature bc)
        {
            CheckHues(bc);
            UnConvert(bc, NoBuff, BossHitsBuff, NoBuff, NoBuff, NoBuff, BossSkillsBuff, NoBuff, NoBuff, NoBuff, 1, 100, 5.0);
        }
        public static void AddCorruptor(BaseCreature bc)
        {
            bc.Name = MonsterName.Generate();
            CheckHues(bc);
            Convert(bc, BossGoldBuff, BossHitsBuff, BossStrBuff, BossIntBuff, BossDexBuff, BossSkillsBuff, BossSpeedBuff, BossFameBuff, BossKarmaBuff, BossDamageBuff, 400, BossTameableModifier);
            AddLoot(bc);
            new CorruptorTimer(bc).Start();
        }
        public static void RemoveCorruptor(BaseCreature bc)
        {
            CheckHues(bc);
            UnConvert(bc, BossGoldBuff, BossHitsBuff, BossStrBuff, BossIntBuff, BossDexBuff, BossSkillsBuff, BossSpeedBuff, BossFameBuff, BossKarmaBuff, BossDamageBuff, 400, BossTameableModifier);
        }

        public static void AddIllusion(BaseCreature bc, Mobile from)
        {
            BaseCreature illusion = Activator.CreateInstance(bc.GetType()) as BaseCreature;
            if (illusion != null)
            {
                illusion.MoveToWorld(bc.Location, bc.Map);
                illusion.Attack(from);
                illusion.SetHits(Utility.RandomMinMax(25, 45));
                illusion.ExperienceValue = 0;
                illusion.Hue = IllusionistHue;
                if (illusion.Backpack != null)
                {
                    foreach (Item item in illusion.Backpack.Items.ToArray())
                    {
                        item.Delete();
                    }

                    illusion.Name = bc.Name;
                }
                illusion.SetLevel();
                if (illusion.Level < bc.Level)
                {
                    illusion.AlterLevels(bc.Level - 2, false, bc.Level - 2, bc.Level - 2);
                }
                illusion.Combatant = from;
            }

            bc.PublicOverheadMessage(
                MessageType.Regular,
                0x3B2,
                false,
                "* The creature creates an illusion *"
            );
        }

        public static void AddIllusionist(BaseCreature bc)
        {
            bc.Name = MonsterName.Generate();
            CheckHues(bc);
            Convert(bc, BossGoldBuff, BossHitsBuff, NoBuff, NoBuff, NoBuff, BossSkillsBuff, NoBuff, BossFameBuff, BossKarmaBuff, 0, 200, 10.0);
            AddLoot(bc);
        }
        public static void RemoveIllusionist(BaseCreature bc)
        {
            CheckHues(bc);
            UnConvert(bc, BossGoldBuff, BossHitsBuff, NoBuff, NoBuff, NoBuff, BossSkillsBuff, NoBuff, BossFameBuff, BossKarmaBuff, 0, 200, 10.0);
        }
        public static void AddRegenerative(BaseCreature bc)
        {
            bc.Name = MonsterName.Generate();
            CheckHues(bc);
            Convert(bc, BossGoldBuff, BossHitsBuff, NoBuff, NoBuff, NoBuff, BossSkillsBuff, NoBuff, BossFameBuff, BossKarmaBuff, 0, 200, 10.0);
            AddLoot(bc);
            new RegenerativeTimer(bc).Start();
        }
        public static void RemoveRegenerative(BaseCreature bc)
        {
            CheckHues(bc);
            UnConvert(bc, BossGoldBuff, BossHitsBuff, NoBuff, NoBuff, NoBuff, BossSkillsBuff, NoBuff, BossFameBuff, BossKarmaBuff, 0, 200, 10.0);
        }
        public static void AddReflective(BaseCreature bc)
        {
            bc.Name = MonsterName.Generate();
            AddLoot(bc);
            Convert(bc, BossGoldBuff, BossHitsBuff, NoBuff, NoBuff, NoBuff, BossSkillsBuff, NoBuff, BossFameBuff, BossKarmaBuff, 0, 200, 10.0);
            CheckHues(bc);
        }
        public static void RemoveReflective(BaseCreature bc)
        {
            UnConvert(bc, BossGoldBuff, BossHitsBuff, NoBuff, NoBuff, NoBuff, BossSkillsBuff, NoBuff, BossFameBuff, BossKarmaBuff, 0, 200, 10.0);
            CheckHues(bc);
        }
        public static void AddWarlord(BaseCreature bc) {
            bc.Name = MonsterName.Generate();
            CheckHues(bc);
            bc.SetSkill(SkillName.Tactics, 85.1, 100.0);
            bc.SetSkill(SkillName.Wrestling, 85.1, 100.0);
            bc.SetSkill(SkillName.Swords, 85.1, 100.0);
            bc.SetSkill(SkillName.Macing, 85.1, 100.0);
            bc.SetSkill(SkillName.Fencing, 85.1, 100.0);
            bc.SetSkill(SkillName.Lumberjacking, 85.1, 100.0);
            bc.SetSkill(SkillName.Anatomy, 85.1, 100.0);
            if (bc.RawStr < 350)
            {
                bc.SetStr(350, 450);; // half strength as ogre lords
            }
            Convert(bc, BossGoldBuff, BossHitsBuff, NoBuff, NoBuff, NoBuff, BossSkillsBuff, NoBuff, BossFameBuff, BossKarmaBuff, 0, 200, 10.0);
            AddLoot(bc);
        }
        public static void RemoveWarlord(BaseCreature bc)
        {
            CheckHues(bc);
            UnConvert(bc, BossGoldBuff, BossHitsBuff, NoBuff, NoBuff, NoBuff, BossSkillsBuff, NoBuff, BossFameBuff, BossKarmaBuff, 0, 200, 10.0);
            bc.Skills.Tactics.Base = 0;
            bc.Skills.Wrestling.Base = 0;
            bc.Skills.Swords.Base = 0;
            bc.Skills.Macing.Base = 0;
            bc.Skills.Fencing.Base = 0;
            bc.Skills.Lumberjacking.Base = 0;
            bc.Skills.Anatomy.Base = 0;
            bc.SetStr(66, 105);
        }
        public static void AddMagical(BaseCreature bc) {
            bc.Name = MonsterName.Generate();
            CheckHues(bc);
            bc.SetSkill(SkillName.Magery, 85.1, 100.0);
            if (bc.RawInt < 265)
            {
                bc.SetInt(265, 305); // similar to lich
            }
            Convert(bc, BossGoldBuff, BossHitsBuff, NoBuff, NoBuff, NoBuff, BossSkillsBuff, NoBuff, BossFameBuff, BossKarmaBuff, 0, 200, 10.0);
            AddLoot(bc);
        }
        public static void RemoveMagical(BaseCreature bc)
        {
            CheckHues(bc);
            UnConvert(bc, BossGoldBuff, BossHitsBuff, NoBuff, NoBuff, NoBuff, BossSkillsBuff, NoBuff, BossFameBuff, BossKarmaBuff, 0, 200, 10.0);
            bc.Skills.Magery.Base = 0;
            bc.SetInt(31, 55);; // similar to ettin
        }
        public static void AddMagicResistant(BaseCreature bc) {
            bc.Name = MonsterName.Generate();
            CheckHues(bc);
            bc.SetResistance(ResistanceType.Fire, 100);
            bc.SetResistance(ResistanceType.Cold, 100);
            bc.SetResistance(ResistanceType.Poison, 100);
            bc.SetResistance(ResistanceType.Energy, 100);
            bc.SetSkill(SkillName.MagicResist, 85.1, 100.0);
            Convert(bc, BossGoldBuff, BossHitsBuff, NoBuff, NoBuff, NoBuff, BossSkillsBuff, NoBuff, BossFameBuff, BossKarmaBuff, 0, 200, 10.0);
            AddLoot(bc);
        }
        public static void RemoveMagicResistant(BaseCreature bc)
        {
            CheckHues(bc);
            bc.SetResistance(ResistanceType.Fire, 0);
            bc.SetResistance(ResistanceType.Cold, 0);
            bc.SetResistance(ResistanceType.Poison, 0);
            bc.SetResistance(ResistanceType.Energy, 0);
            UnConvert(bc, BossGoldBuff, BossHitsBuff, NoBuff, NoBuff, NoBuff, BossSkillsBuff, NoBuff, BossFameBuff, BossKarmaBuff, 0, 200, 10.0);
            bc.Skills.MagicResist.Base = 0;
        }

        public static void SpawnMinions(BaseCreature bc)
        {
            bc.PublicOverheadMessage(
                MessageType.Regular,
                0x3B2,
                false,
                "* Creature reveals its minions *"
            );
            Type monsterType = bc.GetType();
            List<Mobile> minions = new List<Mobile>();
            for (int i = 0; i < bc.NumberOfMinions; ++i)
            {
                BaseCreature minion = Activator.CreateInstance(monsterType) as BaseCreature;
                if (minion != null)
                {
                    Point3D point = bc.Map.GetRandomNearbyLocation(bc.Location, 2, 1);
                    minion.SetLevel();
                    minion.MoveToWorld(point, bc.Map);
                    minion.FixedParticles(0x376A, 9, 32, 0x13AF, EffectLayer.Waist);
                    minion.PlaySound(0x1FE);
                    minion.IsMinion = true;
                    Convert(
                        minion,
                        MinionGoldBuff,
                        MinionHitsBuff,
                        MinionStrBuff,
                        MinionIntBuff,
                        MinionDexBuff,
                        MinionSkillsBuff,
                        MinionSpeedBuff,
                        MinionFameBuff,
                        MinionKarmaBuff,
                        MinionDamageBuff
                    );
                    AddLoot(minion);
                    minions.Add(minion);
                }
            }
            bc.Minions = minions;
        }
        public static void Bossify(BaseCreature bc)
        {
            bc.Name = MonsterName.Generate();
            CheckHues(bc);
            bc.NumberOfMinions = Utility.RandomMinMax(3, 5);
            bc.SetResistance(ResistanceType.Cold, bc.BaseColdResistance + 15);
            bc.SetResistance(ResistanceType.Poison, bc.BasePoisonResistance + 15);
            bc.SetResistance(ResistanceType.Energy, bc.BaseEnergyResistance + 15);
            bc.SetResistance(ResistanceType.Fire, bc.BaseFireResistance + 15);
            bc.SetResistance(ResistanceType.Physical, bc.BasePhysicalResistance + 25);
            Convert(bc, BossGoldBuff, BossHitsBuff, BossStrBuff, BossIntBuff, BossDexBuff, BossSkillsBuff, BossSpeedBuff, BossFameBuff, BossKarmaBuff, BossDamageBuff, 500, BossTameableModifier);
            AddLoot(bc);
        }
        public static void AddLoot(BaseCreature bc)
        {
            int dynamicXp = (int)bc.DynamicExperienceValue();
            int lootRating = dynamicXp / 355;
            if (lootRating > 7)
            {
                lootRating = 7;
            }
            switch (lootRating)
            {
                default:
                    bc.ForceRandomLoot(LootPack.Average, 1);
                    break;
                case 2:
                    bc.ForceRandomLoot(LootPack.Average, Utility.RandomMinMax(1, 2));
                    break;
                case 3:
                    bc.ForceRandomLoot(LootPack.Rich, Utility.RandomMinMax(2, 4));
                    break;
                case 4:
                    bc.ForceRandomLoot(LootPack.Rich, Utility.RandomMinMax(1, 2));
                    break;
                case 5:
                    bc.ForceRandomLoot(LootPack.FilthyRich, Utility.RandomMinMax(1, 2));
                    break;
                case 6:
                    bc.ForceRandomLoot(LootPack.UltraRich, Utility.RandomMinMax(1, 2));
                    break;
                case 7:
                    bc.ForceRandomLoot(LootPack.UltraRich, Utility.RandomMinMax(2, 4));
                    break;
            }
        }

        public static void DespawnMinions(BaseCreature bc)
        {
            if (bc.Minions != null)
            {
                foreach (Mobile minion in bc.Minions)
                {
                    minion.Delete();
                }
                bc.Minions = new List<Mobile>();
            }
        }
        public static void UnBossify(BaseCreature bc)
        {
            CheckHues(bc);
            DespawnMinions(bc);
            bc.SetResistance(ResistanceType.Cold, bc.BaseColdResistance - 15);
            bc.SetResistance(ResistanceType.Poison, bc.BasePoisonResistance - 15);
            bc.SetResistance(ResistanceType.Energy, bc.BaseEnergyResistance - 15);
            bc.SetResistance(ResistanceType.Fire, bc.BaseFireResistance - 15);
            bc.SetResistance(ResistanceType.Physical, bc.BasePhysicalResistance - 25);
            UnConvert(bc, BossGoldBuff, BossHitsBuff, BossStrBuff, BossIntBuff, BossDexBuff, BossSkillsBuff, BossSpeedBuff, BossFameBuff, BossKarmaBuff, BossDamageBuff, 500, BossTameableModifier);
        }

        public static void Convert(BaseCreature bc, double goldBuff, double hitsBuff, double strBuff, double intBuff, double dexBuff, double skillsBuff, double speedBuff, double fameBuff, double karmaBuff, int damageBuff, int baseXpModiefier = 0, double tameableModifier = 0.0)
        {
            bc.ExperienceValue += baseXpModiefier;
            if (baseXpModiefier > 0)
            {
                bc.Backpack?.DropItem(new Gold(baseXpModiefier));
            }
            if (bc.Backpack != null)
            {
                List<Item> goldItems = bc.Backpack.FindItemsByType(typeof(Gold));
                foreach(Item gold in goldItems)
                {
                    ((Gold)gold).Amount = (int)(((Gold)gold).Amount * goldBuff);
                }
            }

            if (bc.HitsMaxSeed >= 0)
            {
                bc.HitsMaxSeed = (int)(bc.HitsMaxSeed * hitsBuff);
            }
            int soulFeedControlSlots = bc.SoulFeeds / 5;
            if (soulFeedControlSlots > 0)
            {
                bc.ControlSlots += soulFeedControlSlots;
            }
            if (tameableModifier >= 15.0)
            {
                bc.ControlSlots++;
            }

            if (bc.Tamable)
            {
                if (bc.MinTameSkill < 50 && tameableModifier > 4.0)
                {
                    bc.MinTameSkill = 50;
                }
                bc.MinTameSkill += tameableModifier;
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
                    if (skill.Base >= 120.0)
                    {
                        skill.Base = 120.0;
                    }
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
        public static void UnConvert(BaseCreature bc, double goldBuff, double hitsBuff, double strBuff, double intBuff, double dexBuff, double skillsBuff, double speedBuff, double fameBuff, double karmaBuff, int damageBuff, int baseXpModifier = 0, double tameableModifier = 0.0)
        {
            bc.ExperienceValue -= baseXpModifier;
            bc.Hue = 0;

            if (bc.HitsMaxSeed >= 0)
            {
                bc.HitsMaxSeed = (int)(bc.HitsMaxSeed / hitsBuff);
            }

            int soulFeedControlSlots = bc.SoulFeeds / 5;
            if (soulFeedControlSlots > 0)
            {
                bc.ControlSlots -= soulFeedControlSlots;
            }

            if (tameableModifier >= 15.0)
            {
                bc.ControlSlots--;
            }

            bc.RawStr = (int)(bc.RawStr / strBuff);
            bc.RawInt = (int)(bc.RawInt / intBuff);
            bc.RawDex = (int)(bc.RawDex / dexBuff);

            bc.Hits = bc.HitsMax;
            bc.Mana = bc.ManaMax;
            bc.Stam = bc.StamMax;

            if (bc.Tamable)
            {
                bc.MinTameSkill -= tameableModifier;
            }

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

        public static void RandomMonsterBuffs(BaseCreature creature, int maxBuffs)
        {
            var challengerBuffs = 0;
            int cycles = 0;
            while (challengerBuffs < maxBuffs)
            {
                int chance = 3 + cycles;
                if (Utility.Random(100) < chance && !creature.IsIllusionist)
                {
                    creature.IsIllusionist = true;
                    challengerBuffs++;
                }
                if (Utility.Random(100) < chance && !creature.IsCorruptor)
                {
                    creature.IsCorruptor = true;
                    challengerBuffs++;
                }
                if (Utility.Random(100) < chance && !creature.IsEthereal)
                {
                    creature.IsEthereal = true;
                    challengerBuffs++;
                }
                if (Utility.Random(100) < chance && !creature.IsIllusionist)
                {
                    creature.IsIllusionist = true;
                    challengerBuffs++;
                }
                if (Utility.Random(100) < chance && !creature.IsMagicResistant)
                {
                    creature.IsMagicResistant = true;
                    challengerBuffs++;
                }
                if (Utility.Random(100) < chance && !creature.IsFrozen)
                {
                    creature.IsFrozen = true;
                    challengerBuffs++;
                }
                if (Utility.Random(100) < chance && !creature.IsBurning)
                {
                    creature.IsBurning = true;
                    challengerBuffs++;
                }
                if (Utility.Random(100) < chance && !creature.IsElectrified)
                {
                    creature.IsElectrified = true;
                    challengerBuffs++;
                }
                if (Utility.Random(100) < chance && !creature.IsToxic)
                {
                    creature.IsToxic = true;
                    challengerBuffs++;
                }
                if (Utility.Random(100) < chance && !creature.IsReflective)
                {
                    creature.IsReflective = true;
                    challengerBuffs++;
                }
                if (Utility.Random(100) < chance && !creature.IsRegenerative)
                {
                    creature.IsRegenerative = true;
                    challengerBuffs++;
                }
                if (Utility.Random(100) < chance && !creature.IsSoulFeeder)
                {
                    creature.IsSoulFeeder = true;
                    challengerBuffs++;
                }
                if (Utility.Random(100) < chance && !creature.IsMagical)
                {
                    creature.IsMagical = true;
                    challengerBuffs++;
                }
                if (Utility.Random(100) < chance && !creature.IsWarlord)
                {
                    creature.IsWarlord = true;
                    challengerBuffs++;
                }
                if (Utility.Random(100) < chance - 2 && !creature.IsBoss)
                {
                    creature.IsBoss = true;
                    challengerBuffs++;
                }
                cycles += 3;
            }
        }

        public static bool CheckElementalEffect(int chance = 10)
        {
            return Utility.Random(100) < chance;
        }

        public static void CheckElementalAttack(BaseCreature owner, Mobile mobile)
        {
            int damage = 0;
            int sound = 0;
            int fireDam = 0;
            int coldDam = 0;
            int poisonDam = 0;
            int energyDam = 0;
            if (owner.IsElectrified)
            {
                damage = Utility.RandomMinMax(3, 5);
                energyDam = 100;
                sound = 0x5C3;
                Effects.SendLocationParticles(
                    EffectItem.Create(mobile.Location, mobile.Map, EffectItem.DefaultDuration),
                    0x37CC,
                    1,
                    40,
                    97,
                    3,
                    9917,
                    0
                );
                if (CheckElementalEffect())
                {
                    Blindness.BlindTarget(mobile, Utility.RandomMinMax(5, 7), "* Blinded by electrical shock *");
                }
            }
            else if (owner.IsBurning)
            {
                damage = Utility.RandomMinMax(5, 12);
                fireDam = 100;
                Effects.SendLocationEffect(mobile, 0x398F, 16, 10, 0);
                sound = 0x5CF;
            }
            else if (owner.IsToxic)
            {
                damage = mobile.Poisoned ? Utility.RandomMinMax(5, 7) : Utility.RandomMinMax(3, 5);
                poisonDam = 100;
                Effects.SendLocationEffect(mobile, 0x3924, 16, 10, 0);
                sound = 0x205;
            }
            else if (owner.IsFrozen)
            {
                damage = Utility.RandomMinMax(3, 5);
                PlayerMobile player = null;
                int frozenAvoidanceChance = 0;
                if (mobile is PlayerMobile) {
                    player = (PlayerMobile)mobile;
                    Warmth warmth = player.GetTalent(typeof(Warmth)) as Warmth;
                    if (warmth != null) {
                        frozenAvoidanceChance = warmth.ModifySpellMultiplier();
                    }
                    if (player.Heroism())
                    {
                        frozenAvoidanceChance = 100;
                    }
                }

                if (CheckElementalEffect() && !(Utility.Random(100) < frozenAvoidanceChance) && player?.Frozen == false)
                {
                    mobile.Freeze(TimeSpan.FromSeconds(Utility.RandomMinMax(5, 7)));
                    mobile.PublicOverheadMessage(
                        MessageType.Regular,
                        0x3B2,
                        false,
                        "* Frozen by intense cold *"
                    );
                } else if (player != null && frozenAvoidanceChance < 100 && !player.Slowed)
                {
                    player.Slow(6);
                    mobile.PublicOverheadMessage(
                        MessageType.Regular,
                        0x3B2,
                        false,
                        "* Slowed by intense cold *"
                    );
                }
                coldDam = 100;
                Effects.SendLocationEffect(mobile, 0x3924, 16, 10, FrozenHue);
                sound = 0x5C7;
            }

            if (Core.AOS)
            {
                AOS.Damage(mobile, damage, 0, fireDam, coldDam, poisonDam, energyDam);
            }
            else
            {
                mobile.Damage(damage, owner);
            }
            if (sound != 0)
            {
                mobile.PlaySound(sound);
            }
        }
        public static bool CannotBeAltered(BaseCreature baseCreature) => baseCreature.Tamable;
        public static bool DoubleElementalDamage(BaseCreature bc, BaseWeapon weapon)
        {
            return (bc.IsFrozen && weapon.Burning) || (bc.IsBurning && weapon.Frozen);
        }
        public static bool CanDropShards(BaseCreature bc)
        {
            return (bc.IsFrozen || bc.IsBurning || bc.IsToxic || bc.IsElectrified);
        }
        public static void CheckElementalAoe(BaseCreature owner)
        {
            foreach (Mobile mobile in owner.GetMobilesInRange(8))
            {
                if (mobile != owner && owner.ControlMaster != mobile && (mobile is PlayerMobile || owner.IsEnemy(mobile)) && owner.InLOS(mobile))
                {
                    CheckElementalAttack(owner, mobile);
                    mobile.RevealingAction();
                }
            }
        }

        public static void LevelUp(BaseCreature bc)
        {
            if (bc.Level < 70)
            {
                bc.Level++;
                if (!bc.Controlled)
                {
                    if (!bc.IsVeteran)
                    {
                        bc.IsVeteran = true;
                    } else if (bc.IsVeteran && !bc.IsHeroic)
                    {
                        bc.IsVeteran = false;
                        bc.IsHeroic = true;
                    } else if (bc.IsHeroic)
                    {
                        int buffs = 0;
                        while (buffs < 3)
                        {
                            switch (Utility.RandomMinMax(1,9))
                            {
                                case 1:
                                    if (!bc.IsBurning)
                                    {
                                        bc.IsBurning = true;
                                    }
                                    break;
                                case 2:
                                    if (!bc.IsFrozen)
                                    {
                                        bc.IsFrozen = true;
                                    }
                                    break;
                                case 3:
                                    if (!bc.IsToxic)
                                    {
                                        bc.IsToxic = true;
                                    }
                                    break;
                                case 4:
                                    if (!bc.IsElectrified)
                                    {
                                        bc.IsElectrified = true;
                                    }
                                    break;
                                case 5:
                                    if (!bc.IsEthereal)
                                    {
                                        bc.IsEthereal = true;
                                    }
                                    break;
                                case 6:
                                    if (!bc.IsIllusionist)
                                    {
                                        bc.IsIllusionist = true;
                                    }
                                    break;
                                case 7:
                                    if (!bc.IsReflective)
                                    {
                                        bc.IsReflective = true;
                                    }
                                    break;
                                case 8:
                                    if (!bc.IsRegenerative)
                                    {
                                        bc.IsRegenerative = true;
                                    }
                                    break;
                                default:
                                    if (!bc.IsMagicResistant)
                                    {
                                        bc.IsMagicResistant = true;
                                    }
                                    break;
                            }
                            buffs++;
                        }
                    }
                    bc.Hits = bc.HitsMax;
                }
            }
        }

        public static void BalanceLevels(BaseCreature baseCreature, int comparerLevel)
        {
            // if they're over 10 levels, dont intervene
            if (!(baseCreature.Level > comparerLevel + 10))
            {
                // add levels to be closer to comparer give or take 3
                if (baseCreature.Level < comparerLevel - 3)
                {
                    baseCreature.AlterLevels(comparerLevel - baseCreature.Level + Utility.RandomMinMax(-3, 3), false, comparerLevel - baseCreature.Level - 3, comparerLevel - baseCreature.Level + 3);
                } else if (baseCreature.Level > comparerLevel + 5)
                {
                    baseCreature.AlterLevels(baseCreature.Level - comparerLevel + Utility.RandomMinMax(-3, 3), false, baseCreature.Level - comparerLevel - 3, baseCreature.Level - comparerLevel + 3);
                }
            }
        }

        public static void BalanceCreatureAgainstMobile(BaseCreature target, Mobile mobile)
        {
            if (mobile is PlayerMobile playerMobile)
            {
                BalanceLevels(target, playerMobile.Level);
            } else if (mobile is BaseCreature baseCreature)
            {
                BalanceLevels(target, baseCreature.Level);
            }

        }

        public static void CheckTimers(BaseCreature bc)
        {
            if (bc.IsFrozen || bc.IsBurning || bc.IsToxic || bc.IsElectrified) {
                new ElementalDamageTimer(bc).Start();
            }
            if (bc.IsCorruptor)
            {
                new CorruptorTimer(bc).Start();
            }
            if (bc.IsRegenerative)
            {
                new RegenerativeTimer(bc).Start();
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
                    int staticGrowth = Utility.RandomMinMax(8, 10);
                    int dynamicGrowth = AOS.Scale(m_Owner.HitsMax, 2);
                    m_Owner.Hits += staticGrowth > dynamicGrowth ? staticGrowth : dynamicGrowth;
                    Delay = Interval = m_Owner.HitsMax < m_Owner.HitsMax * .75 ? FastRegenRate : CPUSaverRate;
                }
                else
                {
                    Stop();
                }
            }
        }
        private class ElementalDamageTimer : Timer
        {
            private readonly BaseCreature m_Owner;

            public ElementalDamageTimer(Mobile m)
                : base(ElementalDamageRate, ElementalDamageRate)
            {
                m_Owner = m as BaseCreature;
            }

            protected override void OnTick()
            {
                if (!m_Owner.Deleted && (m_Owner.IsToxic || m_Owner.IsElectrified || m_Owner.IsBurning || m_Owner.IsFrozen) && m_Owner.Map != Map.Internal)
                {
                    CheckElementalAoe(m_Owner);
                    Delay = Interval = ElementalDamageRate;
                }
                else
                {
                    Stop();
                }
            }
        }
        private class CorruptorTimer : Timer
        {
            private readonly BaseCreature m_Owner;
            private int m_Modifier;

            public CorruptorTimer(Mobile m)
                : base(CorruptionRate, CorruptionRate)
            {
                m_Modifier = 0;
                m_Owner = m as BaseCreature;
            }

            protected override void OnTick()
            {
                if (!m_Owner.Deleted && m_Owner.IsCorruptor && m_Owner.Map != Map.Internal)
                {
                    foreach(Mobile mobile in m_Owner.GetMobilesInRange(CorruptionRange))
                    {
                        if (mobile is BaseCreature creature)
                        {
                            if (creature.ControlMaster != null || m_Owner == mobile || creature.IsInvulnerable || creature.IsBoss || creature.IsParagon || creature.IsEthereal || creature.IsReflective || Core.AOS && !m_Owner.InLOS(mobile))
                            {
                                continue;
                            }

                            if (creature.IsCorrupted && m_Owner.Combatant is not null && !m_Owner.IsEnemy(creature))
                            {
                                creature.Combatant = m_Owner.Combatant;
                            }
                            creature.IsCorrupted = true;
                        } else if (mobile is PlayerMobile player && m_Owner.ControlMaster != player) {
                            if (CheckElementalEffect(5 + m_Modifier))
                            {
                                m_Modifier = 0;
                                player.SendSound(0x37D);
                                player.Paralyze(TimeSpan.FromSeconds(10));
                                player.Fear(Utility.Random(10));
                            }
                            else
                            {
                                m_Modifier++;
                            }
                        }
                    }
                    Delay = Interval = CorruptionRate;
                }
                else
                {
                    Stop();
                }
            }
        }

        public static void RemoveRandomBuff(BaseCreature creature)
        {
            if (creature.IsBoss && Utility.RandomBool())
            {
                creature.IsBoss = false;
                return;
            }

            if (creature.IsBurning && Utility.RandomBool())
            {
                creature.IsBurning = false;
                return;
            }

            if (creature.IsCorrupted && Utility.RandomBool())
            {
                creature.IsCorrupted = false;
                return;
            }

            if (creature.IsCorruptor && Utility.RandomBool())
            {
                creature.IsCorruptor = false;
                return;
            }

            if (creature.IsElectrified && Utility.RandomBool())
            {
                creature.IsElectrified = false;
                return;
            }

            if (creature.IsEthereal && Utility.RandomBool())
            {
                creature.IsEthereal = false;
                return;
            }

            if (creature.IsFrozen && Utility.RandomBool())
            {
                creature.IsFrozen = false;
                return;
            }

            if (creature.IsHeroic && Utility.RandomBool())
            {
                creature.IsHeroic = false;
                return;
            }

            if (creature.IsIllusionist && Utility.RandomBool())
            {
                creature.IsIllusionist = false;
                return;
            }

            if (creature.IsReflective && Utility.RandomBool())
            {
                creature.IsReflective = false;
                return;
            }

            if (creature.IsRegenerative && Utility.RandomBool())
            {
                creature.IsRegenerative = false;
                return;
            }

            if (creature.IsToxic && Utility.RandomBool())
            {
                creature.IsToxic = false;
                return;
            }

            if (creature.IsVeteran && Utility.RandomBool())
            {
                creature.IsVeteran = false;
                return;
            }

            if (creature.IsMagicResistant && Utility.RandomBool())
            {
                creature.IsMagicResistant = false;
                return;
            }

            if (creature.IsSoulFeeder && Utility.RandomBool())
            {
                creature.IsSoulFeeder = false;
            }
        }
    }
}


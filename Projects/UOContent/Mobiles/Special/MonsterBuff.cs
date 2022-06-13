using System;
using System.Collections.Generic;
using Server.Items;
using Server.Talent;
using Server.Utilities;
using Server.Network;

namespace Server.Mobiles
{
    public static class MonsterBuff
    {
        private static readonly TimeSpan FastRegenRate = TimeSpan.FromSeconds(1);
        private static readonly TimeSpan CPUSaverRate = TimeSpan.FromSeconds(3);
        private static readonly TimeSpan CorruptionRate = TimeSpan.FromSeconds(5);
        private static readonly TimeSpan ElementalDamageRate = TimeSpan.FromSeconds(6);

        public static readonly int CorruptionRange = 12;

        public static double BossGoldBuff = 1.15;
        public static double BossHitsBuff = 1.51;
        public static double BossStrBuff = 1.15;
        public static double BossIntBuff = 1.15;
        public static double BossDexBuff = 1.15;
        public static double BossSkillsBuff = 1.09;
        public static double BossSpeedBuff = 1.09;
        public static double BossFameBuff = 1.15;
        public static double BossKarmaBuff = 1.17;
        public static int BossDamageBuff = 2;

        public static double MinionGoldBuff = 1.07;
        public static double MinionHitsBuff = 1.08;
        public static double MinionStrBuff = 1.07;
        public static double MinionIntBuff = 1.07;
        public static double MinionDexBuff = 1.07;
        public static double MinionSkillsBuff = 1.07;
        public static double MinionSpeedBuff = 1.07;
        public static double MinionFameBuff = 1.09;
        public static double MinionKarmaBuff = 1.09;
        public static int MinionDamageBuff = 1;

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
            } else if (bc.IsBoss)
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
                    MinionDamageBuff
                );
                bc.PublicOverheadMessage(MessageType.Regular, 0x0481, false, "* This creature grows in strength *");
            }
        }

        public static void AddElementalProperties(BaseCreature bc)
        {
            bc.Name = MonsterName.Generate();
            CheckHues(bc);
            Convert(bc, NoBuff, BossHitsBuff, NoBuff, NoBuff, NoBuff, BossSkillsBuff, NoBuff, NoBuff, NoBuff, BossDamageBuff);

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
            }
            else if (bc.IsFrozen)
            {
                bc.SetResistance(ResistanceType.Cold, 100);
                if (bc.Backpack != null) {
                    bc.Backpack.AddItem(new IcyHeart());
                }
            }
            AddLoot(bc, Utility.Random(2, 4));
            new ElementalDamageTimer(bc).Start();
        }
        public static void RemoveElementalProperties(BaseCreature bc)
        {
            CheckHues(bc);
            UnConvert(bc, NoBuff, BossHitsBuff, NoBuff, NoBuff, NoBuff, BossSkillsBuff, NoBuff, NoBuff, NoBuff, BossDamageBuff);
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
            }
            else if (bc.IsFrozen)
            {
                bc.SetResistance(ResistanceType.Cold, 0);
            }
        }

        public static void AddEthereal(BaseCreature bc)
        {
            bc.Name = MonsterName.Generate();
            CheckHues(bc);
            Convert(bc, NoBuff, BossHitsBuff, NoBuff, NoBuff, NoBuff, BossSkillsBuff, NoBuff, NoBuff, NoBuff, BossDamageBuff);
            AddLoot(bc, Utility.Random(2, 4));
        }
        public static void RemoveEthereal(BaseCreature bc)
        {
            CheckHues(bc);
            UnConvert(bc, NoBuff, BossHitsBuff, NoBuff, NoBuff, NoBuff, BossSkillsBuff, NoBuff, NoBuff, NoBuff, BossDamageBuff);
        }
        public static void AddCorrupted(BaseCreature bc)
        {
            CheckHues(bc);
            Convert(bc, NoBuff, BossHitsBuff, NoBuff, NoBuff, NoBuff, BossSkillsBuff, NoBuff, NoBuff, NoBuff, 1);

        }
        public static void RemoveCorrupted(BaseCreature bc)
        {
            CheckHues(bc);
            UnConvert(bc, NoBuff, BossHitsBuff, NoBuff, NoBuff, NoBuff, BossSkillsBuff, NoBuff, NoBuff, NoBuff, 1);
        }
        public static void AddCorruptor(BaseCreature bc)
        {
            bc.Name = MonsterName.Generate();
            CheckHues(bc);
            Convert(bc, BossGoldBuff, BossHitsBuff, BossStrBuff, BossIntBuff, BossDexBuff, BossSkillsBuff, BossSpeedBuff, BossFameBuff, BossKarmaBuff, BossDamageBuff);
            AddLoot(bc, Utility.Random(2, 4));
            new CorruptorTimer(bc).Start();
        }
        public static void RemoveCorruptor(BaseCreature bc)
        {
            CheckHues(bc);
            UnConvert(bc, BossGoldBuff, BossHitsBuff, BossStrBuff, BossIntBuff, BossDexBuff, BossSkillsBuff, BossSpeedBuff, BossFameBuff, BossKarmaBuff, BossDamageBuff);
        }

        public static void AddIllusion(BaseCreature bc, Mobile from)
        {
            BaseCreature illusion = Activator.CreateInstance(bc.GetType()) as BaseCreature;
            if (illusion != null)
            {
                illusion.MoveToWorld(bc.Location, bc.Map);
                illusion.Attack(@from);
                illusion.SetHits(Utility.Random(5, 15));
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
            Convert(bc, BossGoldBuff, BossHitsBuff, NoBuff, NoBuff, NoBuff, BossSkillsBuff, NoBuff, BossFameBuff, BossKarmaBuff, 0);
            AddLoot(bc, Utility.Random(2, 4));
        }
        public static void RemoveIllusionist(BaseCreature bc)
        {
            CheckHues(bc);
            UnConvert(bc, BossGoldBuff, BossHitsBuff, NoBuff, NoBuff, NoBuff, BossSkillsBuff, NoBuff, BossFameBuff, BossKarmaBuff, 0);
        }
        public static void AddRegenerative(BaseCreature bc)
        {
            bc.Name = MonsterName.Generate();
            CheckHues(bc);
            AddLoot(bc, Utility.Random(1, 3));
            new RegenerativeTimer(bc).Start();
        }
        public static void RemoveRegenerative(BaseCreature bc)
        {
            CheckHues(bc);
        }
        public static void AddReflective(BaseCreature bc)
        {
            bc.Name = MonsterName.Generate();
            AddLoot(bc, Utility.Random(1, 3));
            Convert(bc, BossGoldBuff, BossHitsBuff, NoBuff, NoBuff, NoBuff, BossSkillsBuff, NoBuff, BossFameBuff, BossKarmaBuff, 0);
            CheckHues(bc);
        }
        public static void RemoveReflective(BaseCreature bc)
        {
            UnConvert(bc, BossGoldBuff, BossHitsBuff, NoBuff, NoBuff, NoBuff, BossSkillsBuff, NoBuff, BossFameBuff, BossKarmaBuff, 0);
            CheckHues(bc);
        }
        public static void AddMagicResistant(BaseCreature bc) {
            bc.Name = MonsterName.Generate();
            CheckHues(bc);
            bc.SetResistance(ResistanceType.Fire, 100);
            bc.SetResistance(ResistanceType.Cold, 100);
            bc.SetResistance(ResistanceType.Poison, 100);
            bc.SetResistance(ResistanceType.Energy, 100);
            bc.Skills.MagicResist.Base = 120.0;
            Convert(bc, BossGoldBuff, BossHitsBuff, NoBuff, NoBuff, NoBuff, BossSkillsBuff, NoBuff, BossFameBuff, BossKarmaBuff, 0);
            AddLoot(bc, Utility.Random(1, 3));
        }
        public static void RemoveMagicResistant(BaseCreature bc)
        {
            CheckHues(bc);
            bc.SetResistance(ResistanceType.Fire, 0);
            bc.SetResistance(ResistanceType.Cold, 0);
            bc.SetResistance(ResistanceType.Poison, 0);
            bc.SetResistance(ResistanceType.Energy, 0);
            UnConvert(bc, BossGoldBuff, BossHitsBuff, NoBuff, NoBuff, NoBuff, BossSkillsBuff, NoBuff, BossFameBuff, BossKarmaBuff, 0);
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
                Point3D point = new Point3D(bc.Location.X + Utility.Random(2, 3), bc.Location.Y + Utility.Random(2, 3), bc.Y);
                int attempts = 10;
                while (!bc.InLOS(point))
                {
                    if (attempts <= 0)
                    {
                        point = new Point3D(bc.Location.X, bc.Location.Y, bc.Y);
                    }
                    else
                    {
                        point = new Point3D(bc.Location.X + Utility.Random(2, 3), bc.Location.Y + Utility.Random(2, 3), bc.Y);
                    }
                    attempts--;
                }

                if (minion != null)
                {
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
                    AddLoot(minion, 1);
                    minions.Add(minion);
                }
            }
            bc.Minions = minions;
        }
        public static void Bossify(BaseCreature bc)
        {
            bc.Name = MonsterName.Generate();
            CheckHues(bc);
            bc.NumberOfMinions = Utility.Random(4, 6);
            bc.SetResistance(ResistanceType.Physical, 100);
            Convert(bc, BossGoldBuff, BossHitsBuff, BossStrBuff, BossIntBuff, BossDexBuff, BossSkillsBuff, BossSpeedBuff, BossFameBuff, BossKarmaBuff, BossDamageBuff);
            AddLoot(bc, Utility.Random(2, 4));
        }
        public static void AddLoot(BaseCreature bc, int lootLevel)
        {
            int rand = Utility.Random(1, 3);
            switch (lootLevel)
            {
                case 1:
                    bc.ForceRandomLoot(LootPack.Poor, rand);
                    break;
                case 2:
                    bc.ForceRandomLoot(LootPack.Meager, rand);
                    break;
                case 3:
                    bc.ForceRandomLoot(LootPack.Average, rand);
                    break;
                case 4:
                    bc.ForceRandomLoot(LootPack.Rich, rand);
                    break;
                case 5:
                    bc.ForceRandomLoot(LootPack.FilthyRich, rand);
                    break;
                case 6:
                    bc.ForceRandomLoot(LootPack.UltraRich, rand);
                    break;
                case 7:
                    bc.ForceRandomLoot(LootPack.SuperBoss, rand);
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

        public static void CheckFreezeHue(Mobile defender, int originalHue)
        {
            if (defender.Frozen)
            {
                defender.HueMod = 0xC1;
                Timer.StartTimer(TimeSpan.FromSeconds(2), () => CheckFreezeHue(defender, originalHue));
            }
            else
            {
                defender.HueMod = originalHue;
            }
        }

        public static Poison GetPoison()
        {
            switch(Utility.Random(3))
            {
                case 1:
                    return Poison.Regular;
                    break;
                case 2:
                    return Poison.Greater;
                    break;
                default:
                    return Poison.Lesser;
                    break;
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
                damage = Utility.Random(3, 5);
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
                    Blindness.BlindTarget(mobile, Utility.Random(5, 7), "* Blinded by electrical shock *");
                }
            }
            else if (owner.IsBurning)
            {
                damage = Utility.Random(5, 12);
                fireDam = 100;
                Effects.SendLocationEffect(mobile, 0x398F, 16, 10, 0);
                sound = 0x5CF;
            }
            else if (owner.IsToxic)
            {
                damage = Utility.Random(3, 5);
                if (CheckElementalEffect())
                {
                    Poison poison = GetPoison();
                    mobile.ApplyPoison(owner, poison);
                }
                poisonDam = 100;
                Effects.SendLocationEffect(mobile, 0x3924, 16, 10, 0);
                sound = 0x205;
            }
            else if (owner.IsFrozen)
            {
                damage = Utility.Random(3, 5);
                BaseTalent warmth = null;
                PlayerMobile player = null;
                int frozenAvoidanceChance = 0;
                if (mobile is PlayerMobile) {
                    player = (PlayerMobile)mobile;
                    warmth = player.GetTalent(typeof(Warmth));
                    if (warmth != null) {
                        frozenAvoidanceChance = warmth.ModifySpellMultiplier();
                    }
                }

                if (CheckElementalEffect() && !(Utility.Random(100) < frozenAvoidanceChance))
                {
                    mobile.Freeze(TimeSpan.FromSeconds(Utility.Random(5, 7)));
                    mobile.PublicOverheadMessage(
                    MessageType.Regular,
                    0x3B2,
                    false,
                    "* Frozen by intense cold *"
                    );
                    CheckFreezeHue(mobile, mobile.HueMod);
                } else if (player != null)
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
                if (mobile != owner && mobile is PlayerMobile && owner.InLOS(mobile))
                {
                    CheckElementalAttack(owner, mobile);
                    mobile.RevealingAction();
                }
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
                    m_Owner.Hits += Utility.Random(1, 3);
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

            public CorruptorTimer(Mobile m)
                : base(CorruptionRate, CorruptionRate)
            {

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
                            if (m_Owner == mobile || creature.IsInvulnerable || creature.IsBoss || creature.IsParagon || creature.IsEthereal || creature.IsReflective || Core.AOS && !m_Owner.InLOS(mobile))
                            {
                                continue;
                            }
                            creature.IsCorrupted = true;
                        } else if (mobile is PlayerMobile player) {
                            if (CheckElementalEffect(5)) {
                                player.Fear(Utility.Random(10));
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
    }
}


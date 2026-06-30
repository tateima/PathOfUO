using ModernUO.Serialization;
using System;
using Server.Collections;
using Server.ContextMenus;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class PlagueSpawn : BaseCreature
    {
        [Constructible]
        public PlagueSpawn(Mobile owner = null) : base(AIType.AI_Melee)
        {
            Owner = owner;
            ExpireTime = Core.Now + TimeSpan.FromMinutes(1.0);

            Hue = Utility.Random(0x11, 15);

            switch (Utility.Random(12))
            {
                case 0: // earth elemental
                    Body = 14;
                    BaseSoundID = 268;
                    break;
                case 1: // headless one
                    Body = 31;
                    BaseSoundID = 0x39D;
                    break;
                case 2: // person
                    Body = Utility.RandomList(400, 401);
                    break;
                case 3: // gorilla
                    Body = 0x1D;
                    BaseSoundID = 0x9E;
                    break;
                case 4: // serpent
                    Body = 0x15;
                    BaseSoundID = 0xDB;
                    break;
                default: // slime
                    Body = 51;
                    BaseSoundID = 456;
                    break;
            }
            LevelRange = [30, 60];
            StrPerLevel = [2, 3];
            IntPerLevel = [2, 3];
            DexPerLevel = [2, 3];
            ResistancePerLevel = [1, 2];

            SetStr(36, 75);
            SetDex(30, 80);
            SetInt(32, 50);

            SetHits(40, 90);

            SetDamage(3, 5);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 25);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 5, 15);
            SetResistance(ResistanceType.Poison, 15, 35);
            SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.MagicResist, 25.0, 50.0);
            SetSkill(SkillName.Tactics, 25.0, 50.0);
            SetSkill(SkillName.Wrestling, 25.0, 50.0);

            Fame = 1000;
            Karma = -1000;

            VirtualArmor = 20;
        }

        public override string CorpseName => "a plague spawn corpse";

        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner { get; set; }

        [CommandProperty(AccessLevel.GameMaster)]
        public DateTime ExpireTime { get; set; }

        public override bool AlwaysMurderer => true;

        public override string DefaultName => "a plague spawn";

        public override void DisplayPaperdollTo(Mobile to)
        {
        }

        public override void GetContextMenuEntries(Mobile from, ref PooledRefList<ContextMenuEntry> list)
        {
            base.GetContextMenuEntries(from, ref list);

            for (var i = 0; i < list.Count; ++i)
            {
                if (list[i] is PaperdollEntry)
                {
                    list.RemoveAt(i--);
                }
            }
        }

        public override void OnThink()
        {
            if (Owner != null && (Core.Now >= ExpireTime || Owner.Deleted || Map != Owner.Map || !InRange(Owner, 16)))
            {
                PlaySound(GetIdleSound());
                Delete();
            }
            else
            {
                base.OnThink();
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Poor);
            AddLoot(LootPack.Gems);
        }
    }
}

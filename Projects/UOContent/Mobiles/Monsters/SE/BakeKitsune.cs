using ModernUO.Serialization;
using System;
using Server.Engines.Plants;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class BakeKitsune : BaseCreature
    {
        private TimerExecutionToken _disguiseTimerToken;

        [Constructible]
        public BakeKitsune() : base(AIType.AI_Mage)
        {
            Body = 246;

            SetStr(171, 220);
            SetDex(126, 145);
            SetInt(376, 425);

            SetHits(301, 350);

            SetDamage(15, 22);

            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Energy, 30);

            SetResistance(ResistanceType.Physical, 40, 60);
            SetResistance(ResistanceType.Fire, 70, 90);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Poison, 40, 60);
            SetResistance(ResistanceType.Energy, 40, 60);

            SetSkill(SkillName.EvalInt, 80.1, 90.0);
            SetSkill(SkillName.Magery, 80.1, 90.0);
            SetSkill(SkillName.MagicResist, 80.1, 100.0);
            SetSkill(SkillName.Tactics, 70.1, 90.0);
            SetSkill(SkillName.Wrestling, 50.1, 55.0);

            Fame = 8000;
            Karma = -8000;

            Tamable = true;
            ControlSlots = 2;
            MinTameSkill = 80.7;

            if (Utility.RandomDouble() < .25)
            {
                PackItem(Seed.RandomBonsaiSeed());
            }
        }

        public override string CorpseName => "a bake kitsune corpse";
        public override string DefaultName => "a bake kitsune";

        public override int Meat => 5;
        public override int Hides => 10;
        public override HideType HideType => HideType.Barbed;
        public override FoodType FavoriteFood => FoodType.Fish;
        public override bool ShowFameTitle => false;
        public override bool ClickTitle => false;
        public override bool PropertyTitle => false;

        private static MonsterAbility[] _abilities = { MonsterAbilities.BloodBathAttack };
        public override MonsterAbility[] GetMonsterAbilities() => _abilities;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.MedScrolls, 2);
        }

        public override void OnCombatantChange()
        {
            if (Combatant == null && !IsBodyMod && !Controlled && !_disguiseTimerToken.Running && Utility.RandomBool())
            {
                Timer.StartTimer(TimeSpan.FromSeconds(Utility.RandomMinMax(15, 30)), Disguise, out _disguiseTimerToken);
            }
        }

        public override bool OnBeforeDeath()
        {
            RemoveDisguise();

            return base.OnBeforeDeath();
        }

        public override int GetAngerSound() => 0x4DE;

        public override int GetIdleSound() => 0x4DD;

        public override int GetAttackSound() => 0x4DC;

        public override int GetHurtSound() => 0x4DF;

        public override int GetDeathSound() => 0x4DB;

        [AfterDeserialization]
        private void AfterDeserialization()
        {
            if (PhysicalResistance > 60)
            {
                SetResistance(ResistanceType.Physical, 40, 60);
                SetResistance(ResistanceType.Fire, 70, 90);
                SetResistance(ResistanceType.Cold, 40, 60);
                SetResistance(ResistanceType.Poison, 40, 60);
                SetResistance(ResistanceType.Energy, 40, 60);
            }

            Timer.StartTimer(RemoveDisguise, out _disguiseTimerToken);
        }

        public void Disguise()
        {
            if (Combatant != null || IsBodyMod || Controlled)
            {
                return;
            }

            FixedEffect(0x376A, 8, 32);
            PlaySound(0x1FE);

            Female = Utility.RandomBool();

            if (Female)
            {
                BodyMod = 0x191;
                Name = NameList.RandomName("female");
            }
            else
            {
                BodyMod = 0x190;
                Name = NameList.RandomName("male");
            }

            Title = "the mystic llama herder";
            Hue = Race.Human.RandomSkinHue();
            HairItemID = Race.Human.RandomHair(this);
            HairHue = Race.Human.RandomHairHue();
            FacialHairItemID = Race.Human.RandomFacialHair(this);
            FacialHairHue = HairHue;

            switch (Utility.Random(4))
            {
                case 0:
                    {
                        AddItem(new Shoes(Utility.RandomNeutralHue()));
                        break;
                    }
                case 1:
                    {
                        AddItem(new Boots(Utility.RandomNeutralHue()));
                        break;
                    }
                case 2:
                    {
                        AddItem(new Sandals(Utility.RandomNeutralHue()));
                        break;
                    }
                case 3:
                    {
                        AddItem(new ThighBoots(Utility.RandomNeutralHue()));
                        break;
                    }
            }

            AddItem(new Robe(Utility.RandomNondyedHue()));

            _disguiseTimerToken.Cancel();
            Timer.StartTimer(TimeSpan.FromSeconds(75), RemoveDisguise, out _disguiseTimerToken);
        }

        public void RemoveDisguise()
        {
            _disguiseTimerToken.Cancel();

            if (!IsBodyMod)
            {
                return;
            }

            Name = null;
            Title = null;
            BodyMod = 0;
            Hue = 0;
            HairItemID = 0;
            HairHue = 0;
            FacialHairItemID = 0;
            FacialHairHue = 0;

            DeleteItemOnLayer(Layer.OuterTorso);
            DeleteItemOnLayer(Layer.Shoes);
        }

        public void DeleteItemOnLayer(Layer layer)
        {
            FindItemOnLayer(layer)?.Delete();
        }
    }
}

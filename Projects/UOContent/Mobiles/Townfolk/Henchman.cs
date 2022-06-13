using Server.Items;

namespace Server.Mobiles
{
    public class Henchman : BaseCreature
    {
        [Constructible]
        public Henchman()
            : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 1.4)
        {
            SpeechHue = Utility.RandomDyedHue();

            SetStr(125, 150);
            SetDex(81, 95);
            SetInt(61, 75);

            SetSkill(SkillName.Fencing, 60.0, 60.0);
            SetSkill(SkillName.Macing, 60.0, 60.0);
            SetSkill(SkillName.MagicResist, 60.0, 60.0);
            SetSkill(SkillName.Swords, 60.0, 60.0);
            SetSkill(SkillName.Tactics, 60.0, 60.0);
            SetSkill(SkillName.Wrestling, 60.0, 60.0);
            SetSkill(SkillName.Anatomy, 60.0, 60.0);
            SetSkill(SkillName.Healing, 60.0, 60.0);

            Hue = Race.Human.RandomSkinHue();
            if (Female == Utility.RandomBool())
            {
                Title = "the hench woman";
                Body = 0x191;
                Name = NameList.RandomName("female");
                AddItem(
                    Utility.Random(5) switch
                    {
                        0 => new FemaleLeatherChest(),
                        1 => new FemaleStuddedChest(),
                        2 => new LeatherBustierArms(),
                        3 => new StuddedBustierArms(),
                        _ => new FemalePlateChest() // 4
                    }
                );
                AddItem(new PlateGloves());
                AddItem(new PlateGorget());
            }
            else
            {
                Title = "the henchman";
                Body = 0x190;
                Name = NameList.RandomName("male");
                if (Utility.Random(2) < 1)
                {
                    AddItem(new StuddedArms());
                    AddItem(new StuddedChest());
                    AddItem(new StuddedGloves());
                    AddItem(new StuddedLegs());
                    AddItem(new Sandals());
                } else
                {
                    AddItem(new ChainChest());
                    AddItem(new ChainCoif());
                    AddItem(new ChainLegs());
                    AddItem(new PlateArms());
                    AddItem(new PlateGloves());
                    AddItem(new PlateGorget());
                }

            }

            AddItem(new Cloak());
            AddItem(
                Utility.Random(5) switch
                {
                    0 => new Longsword(),
                    1 => new Bardiche(),
                    2 => new Mace(),
                    3 => new Kryss(),
                    _ => new Spear() // 4
                }
            );

            Utility.AssignRandomHair(this);

            if (Utility.RandomBool())
            {
                Utility.AssignRandomFacialHair(this, HairHue);
            }
            Container pack = new Backpack();
            pack.Movable = false;
            AddItem(pack);
        }

        public Henchman(Serial serial)
            : base(serial)
        {
        }

        public override bool CanHeal => true;
        public override bool CanTeach => false;
        public override bool ClickTitle => false;

        public override bool CheckGold(Mobile from, Item dropped) => dropped is Gold gold && OnGoldGiven(from, gold);
        public override bool OnGoldGiven(Mobile from, Gold dropped)
        {
            if (Controlled && (ControlMaster == from) & dropped.Amount >= 1000)
            {
                Loyalty += dropped.Amount / 1000;
                SayTo(from, "Thanks for the gold, I'll hang around");
                if (Body == 0x191) // female
                {
                    switch (Utility.RandomMinMax(1, 4))
                    {
                        case 1:
                            from.SendSound(0x30B);
                            break;
                        case 2:
                            from.SendSound(0x30C);
                            break;
                        case 3:
                            from.SendSound(0x30F);
                            break;
                        case 4:
                            from.SendSound(0x30A);
                            break;
                    }
                }
                else
                {
                    switch (Utility.RandomMinMax(1, 4))
                    {
                        case 1:
                            from.SendSound(0x419);
                            break;
                        case 2:
                            from.SendSound(0x41A);
                            break;
                        case 3:
                            from.SendSound(0x41B);
                            break;
                        case 4:
                            from.SendSound(0x41E);
                            break;
                    }
                }
            }
            else
            {
                if (Body == 0x191) // female
                {
                    from.SendSound(0x31D);
                }
                else
                {
                    from.SendSound(0x42D);
                }

                SayTo(from, "Give me 1000 gold or more and we'll talk");
            }
            if (Loyalty > MaxLoyalty)
            {
                Loyalty = MaxLoyalty;
            }
            return true;
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);

            var version = reader.ReadInt();
        }
    }
}

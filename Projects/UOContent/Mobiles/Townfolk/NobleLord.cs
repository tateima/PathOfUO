using Server.Items;

namespace Server.Mobiles
{
    public class NobleLord : BaseCreature
    {
        [Constructible]
        public NobleLord(int maxStrength = 3) : base(AIType.AI_Melee, FightMode.Aggressor)
        {
            Title = "the noble lord";
            SetHumanoidStrength(maxStrength);

            SetSkill(SkillName.Parry, 95.0, 120.0);
            SetSkill(SkillName.Swords, 95.0, 120.0);
            SetSkill(SkillName.Macing, 95.0, 120.0);
            SetSkill(SkillName.Fencing, 95.0, 120.0);
            SetSkill(SkillName.Bushido, 95.0, 120.0);
            SetSkill(SkillName.Tactics, 95.0, 120.0);
            SetResistance(ResistanceType.Physical, 30, 75);
            SetResistance(ResistanceType.Fire, 10, 75);
            SetResistance(ResistanceType.Cold, 10, 75);
            SetResistance(ResistanceType.Poison, 15, 75);
            SetResistance(ResistanceType.Energy, 15, 75);
            VirtualArmor = 40;
            Hue = Race.Human.RandomSkinHue();
            var lowHue = GetRandomHue();
            Female = Utility.RandomBool();
            if (Female)
            {
                AddItem(new FancyDress());
                Body = 401;
                Name = NameList.RandomName("female");
                AddItem(new ThighBoots(lowHue));
            }
            else
            {
                AddItem(new BodySash(lowHue));
                AddItem(new Boots(lowHue));
                AddItem(new FancyShirt(GetRandomHue()));
                Body = 400;
                Name = NameList.RandomName("male");
            }

            AddItem(new ShortPants(lowHue));

            AddItem(new Cloak(GetRandomHue()));

            AddItem(Loot.RandomWeapon());

            Utility.AssignRandomHair(this);

            PackGold(200, 250);
        }

        public NobleLord(Serial serial) : base(serial)
        {
        }

        public override bool CanHeal => true;
        public override bool InitialInnocent => false;
        public override bool AlwaysAttackable => true;

        private static int GetRandomHue()
        {
            return Utility.Random(6) switch
            {
                1 => Utility.RandomBlueHue(),
                2 => Utility.RandomGreenHue(),
                3 => Utility.RandomRedHue(),
                4 => Utility.RandomYellowHue(),
                5 => Utility.RandomNeutralHue(),
                _ => 0
            };
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

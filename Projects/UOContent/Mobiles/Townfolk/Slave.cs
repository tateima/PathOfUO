using Server.Items;
using Server.Talent;
using System;

namespace Server.Mobiles
{
    public class Slave : BaseCreature
    {
        [Constructible]
        public Slave() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Title = "the slave";
            InitStats(31, 41, 51);
            SpeechHue = Utility.RandomDyedHue();
            Female = Utility.RandomBool();
            if (Female)
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                AddItem(new Kilt(Utility.RandomDyedHue()));
                AddItem(new ThighBoots());
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                AddItem(new ShortPants(Utility.RandomNeutralHue()));
                AddItem(new Sandals());
            }
            Utility.AssignRandomHair(this);
            Container pack = new Backpack();
            pack.Movable = false;
            AddItem(pack);
        }


        public Slave(Serial serial) : base(serial)
        {
        }

        public string MasterSpeech { get; set; }
        public int MasterLevel { get; set; }
        public Mobile Master { get; set; }
        public TimerExecutionToken _slaveTimerToken;

        public override bool HandlesOnSpeech(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                BaseTalent slaveDriver = player.GetTalent(typeof(SlaveDriver));
                if (slaveDriver != null)
                {
                    MasterLevel = slaveDriver.Level;
                    Master = from;
                }
                return (slaveDriver != null && player.InRange(Location, 3));
            }
            return false;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            if (e.Handled || !e.Mobile.InRange(Location, 3))
            {
                return;
            }

            MasterSpeech = e.Speech.ToLower();

            if (MasterSpeech.Contains("ore") || MasterSpeech.Contains("log") || MasterSpeech.Contains("cloth") || MasterSpeech.Contains("hide"))
            {
                Say("I will be back later");
                FixedParticles(0x376A, 9, 32, 0x13AF, EffectLayer.Waist);
                PlaySound(0x1FE);
                Point3D location = new Point3D();
                location.X = 0;
                location.Y = 0;
                location.Z = 0;
                MoveToWorld(location, Map);
                Timer.StartTimer(TimeSpan.FromMinutes(10), ReturnWithResources, out _slaveTimerToken);
            }
            else
            {
                Say("I don't understand what you mean. Does master want ore, logs, cloth or hide?");
            }
        }

        public void ReturnWithResources()
        {
            MoveToWorld(Master.Location, Master.Map);
            FixedParticles(0x376A, 9, 32, 0x13AF, EffectLayer.Waist);
            PlaySound(0x1FE);
            Say("Here master, I am worked to death...");
            if (Utility.Random(100) < 6 - MasterLevel)
            {
                Say("I failed to find anything...");
            } else
            {
                int randomAmount = Utility.Random(1, 10) + MasterLevel;
                Item item = null;
                if (MasterSpeech.Contains("ore"))
                {
                    item = Utility.Random(1, 5) switch
                    {
                        1 => new IronOre(),
                        2 => new ShadowIronOre(),
                        3 => new CopperOre(),
                        4 => new BronzeOre(),
                        5 => new DullCopperOre(),
                        _ => null
                    };
                }
                else if (MasterSpeech.Contains("log"))
                {
                    if (Core.AOS)
                    {
                        item = Utility.Random(1, 4) switch
                        {
                            1 => new Log(),
                            2 => new AshLog(),
                            3 => new OakLog(),
                            4 => new HeartwoodLog(),
                            _ => null
                        };
                    } else
                    {
                        item = new Log();
                    }
                }
                else if (MasterSpeech.Contains("cloth"))
                {
                    item = Utility.Random(1, 3) switch
                    {
                        1 => new Wool(),
                        2 => new Flax(),
                        3 => new Cotton(),
                        _ => null
                    };
                }
                else
                {
                    item = Utility.Random(1, 3) switch
                    {
                        1 => new Hides(),
                        2 => new HornedHides(),
                        3 => new SpinedHides(),
                        _ => null
                    };
                }

                if (item != null)
                {
                    item.Amount = randomAmount;
                    AddToBackpack(item);
                }
            }

            Master.Karma -= 50;
            Kill();
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

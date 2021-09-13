using System.Collections.Generic;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
    public class TavernKeeper : BaseVendor
    {
        private readonly List<SBInfo> m_SBInfos = new();

        [Constructible]
        public TavernKeeper() : base("the tavern keeper")
        {
        }

        public TavernKeeper(Serial serial) : base(serial)
        {
        }

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBTavernKeeper());
        }

        public override void InitOutfit()
        {
            base.InitOutfit();

            AddItem(new HalfApron());
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            string speech = e.Speech.ToLower();
            if (speech.Contains("bed"))
            {
                e.Handled = true;
                BeginBed(e.Mobile);

            } else if (speech.Contains("fetch"))
            {
                PlayerMobile player = (PlayerMobile)e.Mobile;
                if (player.Henchmen.Count + player.RestedHenchmen.Count > 2)
                {
                    Say("You have too many henchmen already. I shall not fetch them.");
                } else
                {
                    foreach (Mobile henchman in player.RestedHenchmen)
                    {
                        ((Henchman)henchman).SetControlMaster(player);
                        ((Henchman)henchman).MoveToWorld(player.Location, player.Map);
                        ((Henchman)henchman).ControlMaster = player;
                        ((Henchman)henchman).ControlTarget = player;
                    }
                }
                
            } else
            {
                e.Handled = true;
                base.OnSpeech(e);
            }
        }
        public void BeginBed(Mobile from)
        {
            if (Deleted || !from.CheckAlive())
            {
                return;
            }
            Container bank = from.FindBankNoCreate();
            if (!(from.Backpack?.GetAmount(typeof(Gold)) >= 30) &&
                !(Banker.GetBalance(from) >= 30))
            {
                SayTo(from, 1042556); // Thou dost not have enough gold, not even in thy bank account.
            }
            else
            {
                from.SendMessage("Which person wouldst thou like to rest here?");
                from.Target = new BedTarget(this);
            }
        }

        public void EndBed(Mobile from, Henchman henchman)
        {
            if (Deleted || !from.CheckAlive())
            {
                return;
            }

            if (henchman.ControlMaster != from)
            {
                Say("That is not your henchman!");
            }
            else if (!henchman.CheckAlive())
            {
                Say("I am not a healer.");
            }
            else if (((PlayerMobile)from).Henchmen.Count >= 2)
            {
                Say("You cannot have any more henchmen");
            }
            else
            {
                Container bank = from.FindBankNoCreate();

                if (from.Backpack?.ConsumeTotal(typeof(Gold), 30) == true || Banker.Withdraw(from, 30))
                {
                    henchman.ControlTarget = null;
                    henchman.ControlOrder = OrderType.Stay;
                    henchman.Internalize();

                    henchman.SetControlMaster(null);
                    henchman.SummonMaster = null;

                    ((PlayerMobile)from).RestedHenchmen.Add(henchman);

                    Say("I have given them a bed.");
                }
                else
                {
                    SayTo(from, 502677); // But thou hast not the funds in thy bank account!
                }
            }
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

        private class BedTarget : Target
        {
            private readonly TavernKeeper m_TavernKeeper;

            public BedTarget(TavernKeeper tavernKeeper) : base(12, false, TargetFlags.None) => m_TavernKeeper = tavernKeeper;

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Henchman henchman)
                {
                    m_TavernKeeper.EndBed(from, henchman);
                }
                else if (targeted == from)
                {
                    m_TavernKeeper.Say("I cannot find a bed for you.");
                }
                else
                {
                    m_TavernKeeper.Say("I cannot find a bed for them."); 
                }
            }
        }
    }
}

using Server.Talent;
using System;

namespace Server.Mobiles
{
    public class VendorAI : BaseAI
    {
        public VendorAI(BaseCreature m) : base(m)
        {
        }

        public override bool DoActionWander()
        {
            m_Mobile.DebugSay("I'm fine");

            if (m_Mobile.Combatant != null)
            {
                if (m_Mobile.Debug)
                {
                    m_Mobile.DebugSay("{0} is attacking me", m_Mobile.Combatant.Name);
                }

                m_Mobile.Say(Utility.RandomList(1005305, 501603));

                Action = ActionType.Flee;
            }
            else
            {
                if (m_Mobile.FocusMob != null)
                {
                    if (m_Mobile.Debug)
                    {
                        m_Mobile.DebugSay("{0} has talked to me", m_Mobile.FocusMob.Name);
                    }

                    Action = ActionType.Interact;
                }
                else
                {
                    m_Mobile.Warmode = false;

                    base.DoActionWander();
                }
            }

            return true;
        }

        public override bool DoActionInteract()
        {
            var customer = m_Mobile.FocusMob;

            if (m_Mobile.Combatant != null)
            {
                if (m_Mobile.Debug)
                {
                    m_Mobile.DebugSay("{0} is attacking me", m_Mobile.Combatant.Name);
                }

                m_Mobile.Say(Utility.RandomList(1005305, 501603));

                Action = ActionType.Flee;

                return true;
            }

            if (customer?.Deleted != false || customer.Map != m_Mobile.Map)
            {
                m_Mobile.DebugSay("My customer have disapeared");
                m_Mobile.FocusMob = null;

                Action = ActionType.Wander;
            }
            else
            {
                if (customer.InRange(m_Mobile, m_Mobile.RangeFight))
                {
                    if (m_Mobile.Debug)
                    {
                        m_Mobile.DebugSay("I am with {0}", customer.Name);
                    }

                    m_Mobile.Direction = m_Mobile.GetDirectionTo(customer);
                }
                else
                {
                    if (m_Mobile.Debug)
                    {
                        m_Mobile.DebugSay("{0} is gone", customer.Name);
                    }

                    m_Mobile.FocusMob = null;

                    Action = ActionType.Wander;
                }
            }

            return true;
        }

        public override bool DoActionGuard()
        {
            m_Mobile.FocusMob = m_Mobile.Combatant;
            return base.DoActionGuard();
        }

        public override bool HandlesOnSpeech(Mobile from)
        {
            if (from.InRange(m_Mobile, 4))
            {
                return true;
            }

            return base.HandlesOnSpeech(from);
        }

        // Temporary
        public override void OnSpeech(SpeechEventArgs e)
        {
            base.OnSpeech(e);

            var from = e.Mobile;

            if (m_Mobile is BaseVendor vendor && from.InRange(m_Mobile, Core.AOS ? 1 : 4) && !e.Handled)
            {
                // if someone is trying to collect tax and their serials match
                if (e.Speech.ToLower().Contains("collect tax") && vendor.TaxCollectorSerial == from.Serial.ToInt32() && vendor.NextCollectionTime <= DateTime.Now)  
                {
                    TaxCollector taxCollector = (TaxCollector)((PlayerMobile)from).GetTalent(typeof(TaxCollector));
                    if (taxCollector != null)
                    {
                        int random = Utility.Random(75); // up to 75 gold per level
                        int taxAmount = taxCollector.Level * random;
                        if (taxCollector.CanAffordLoss((PlayerMobile)from, taxAmount))
                        {
                            taxCollector.ProcessGoldGain((PlayerMobile)from, taxAmount, taxCollector.VendorCantPay());
                            vendor.NextCollectionTime = DateTime.Now.AddHours(3); // every 3 hours
                        } else
                        {
                            m_Mobile.Say("Thou cannot afford to collect taxes right now.");
                        }
                    }
                } else if (vendor.TaxCollectorSerial != from.Serial.ToInt32() && vendor.TaxCollectorSerial > 0)
                {
                    m_Mobile.Say("I am funded by another adventurer.");
                } else if (e.Speech.ToLower().Contains("collect tax"))
                {
                    m_Mobile.Say("Thou cannot collect taxes from me.");
                }

                if (e.HasKeyword(0x14D)) // *vendor sell*
                {
                    e.Handled = true;

                    vendor.VendorSell(from);
                    vendor.FocusMob = from;
                }
                else if (e.HasKeyword(0x3C)) // *vendor buy*
                {
                    e.Handled = true;

                    vendor.VendorBuy(from);
                    vendor.FocusMob = from;
                }
                else if (WasNamed(e.Speech))
                {
                    if (e.HasKeyword(0x177)) // *sell*
                    {
                        e.Handled = true;

                        vendor.VendorSell(from);
                    }
                    else if (e.HasKeyword(0x171)) // *buy*
                    {
                        e.Handled = true;

                        vendor.VendorBuy(from);
                    }

                    vendor.FocusMob = from;
                }
            }
        }
    }
}

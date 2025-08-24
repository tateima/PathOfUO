using System.Runtime.CompilerServices;

namespace Server.Mobiles;
using Server.Talent;
using System;
using Server.Gumps;

public class VendorAI : BaseAI
{
    // Guards! A villan attacks me!
    // Guards! Help!
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetRandomGuardMessage() => Utility.RandomBool() ? 1005305 : 501603;

    public VendorAI(BaseCreature m) : base(m)
    {
    }

    public override bool DoActionWander()
    {
        DebugSay("I'm fine");

        if (Mobile.Combatant != null)
        {
            this.DebugSayFormatted($"{Mobile.Combatant.Name} is attacking me");

            Mobile.Say(GetRandomGuardMessage());
            Action = ActionType.Flee;
        }
        else if (Mobile.FocusMob != null)
        {
            this.DebugSayFormatted($"{Mobile.FocusMob.Name} has talked to me");

            Action = ActionType.Interact;
        }
        else
        {
            Mobile.Warmode = false;

            base.DoActionWander();
        }

            return true;
        }

    public override bool DoActionInteract()
    {
        var customer = Mobile.FocusMob;

        if (Mobile.Combatant != null)
        {
            this.DebugSayFormatted($"{Mobile.Combatant.Name} is attacking me");

            Mobile.Say(GetRandomGuardMessage());

                Action = ActionType.Flee;

                return true;
            }

        if (customer?.Deleted != false || customer.Map != Mobile.Map)
        {
            DebugSay("My customer has disappeared");

            Mobile.FocusMob = null;

            Action = ActionType.Wander;
        }
        else if (customer.InRange(Mobile, Mobile.RangeFight))
        {
            this.DebugSayFormatted($"I am with {customer.Name}");

            Mobile.Direction = Mobile.GetDirectionTo(customer);
        }
        else
        {
            this.DebugSayFormatted($"{customer.Name} is gone");

            Mobile.FocusMob = null;
            Action = ActionType.Wander;
        }

            return true;
        }

    public override bool DoActionGuard()
    {
        Mobile.FocusMob = Mobile.Combatant;
        return base.DoActionGuard();
    }

    public override bool HandlesOnSpeech(Mobile from)
    {
        if (from.InRange(Mobile, 4))
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

        if (Mobile is BaseVendor vendor && from.InRange(Mobile, Core.AOS ? 1 : 4) && !e.Handled)
        {
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
                    // if someone is trying to collect tax and their serials match
                    if (e.Speech.ToLower().Contains("collect tax") && vendor.TaxCollectorSerial == from.Serial.ToInt32() &&
                        vendor.NextCollectionTime <= DateTime.Now)
                    {
                        TaxCollector taxCollector = (TaxCollector)((PlayerMobile)from).GetTalent(typeof(TaxCollector));
                        if (taxCollector != null)
                        {
                            from.CloseGump<TaxCollectorGump>();
                            int random = Utility.RandomMinMax(1, 75); // up to 75 gold per level
                            int taxAmount = taxCollector.Level * random;
                            bool loss = taxCollector.VendorCantPay();
                            if (taxCollector.CanAffordLoss((PlayerMobile)from, taxAmount))
                            {
                                taxCollector.ProcessGoldGain((PlayerMobile)from, taxAmount, loss);
                                var now = DateTime.Now;
                                vendor.NextCollectionTime = now.AddHours(3); // every 3 hours
                                vendor.SayTo(
                                    from,
                                    loss
                                        ? $"I am sorry but I will need {taxAmount} gold pieces to fund my business"
                                        : $"Here is thy tax payment, {taxAmount} gold pieces"
                                );
                                from.SendSound(0x32);
                            }
                            else
                            {
                                vendor.SayTo(from, "Thou cannot afford to collect taxes right now.");
                            }

                            from.SendGump(new TaxCollectorGump(from));
                        }
                    }
                    else if (vendor.TaxCollectorSerial != from.Serial.ToInt32() && vendor.TaxCollectorSerial > 0)
                    {
                        vendor.SayTo(from, "I am funded by another adventurer.");
                    }
                    else if (e.Speech.ToLower().Contains("collect tax"))
                    {
                        vendor.SayTo(from, "Thou cannot collect taxes from me.");
                    }

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

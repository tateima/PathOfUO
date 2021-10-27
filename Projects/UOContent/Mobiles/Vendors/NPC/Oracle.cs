using System.Collections.Generic;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    public class Oracle : BaseVendor
    {
        private readonly List<SBInfo> m_SBInfos = new();

        [Constructible]
        public Oracle() : base("the oracle")
        {
        }

        public Oracle(Serial serial) : base(serial)
        {
        }

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBOracle());
        }

        public override void InitOutfit()
        {
            AddItem(new Sandals());
            AddItem(new MonkRobe());
        }

        public override bool HandlesOnSpeech(Mobile from) => true;

        public override void OnSpeech(SpeechEventArgs e)
        {
            if (Deleted || !e.Mobile.CheckAlive())
            {
                return;
            }

            if (e.Mobile.InRange(this, 4))
            {
                string speech = e.Speech.ToLower();
                PlayerMobile player = (PlayerMobile)e.Mobile;

                if (!e.Handled && speech == "reset talents")
                {
                    e.Handled = true;
                    bool canReset = false;
                    if (player.TalentResets == 0)
                    {
                        canReset = true;
                        SayTo(player, "I shall do this free of charge as it is your first reset");
                    } else if (player.TalentResets < 5)
                    {
                        // 10k gold for each reset
                        int requiredGold = player.TalentResets * 10000;
                        Container bank = player.FindBankNoCreate();
                        if (player.Backpack?.ConsumeTotal(typeof(Gold), requiredGold) == true || Banker.Withdraw(player, requiredGold))
                        {
                            canReset = true;
                        }
                        else
                        {
                            SayTo(player, 1042556); // Thou dost not have enough gold, not even in thy bank account.
                        }
                    } else
                    {
                        SayTo(player, "You have reached the allowed limit of five talent resets");
                    }

                    if (canReset)
                    {
                        player.ResetTalents();
                        SendMessage("Your talents have been reset");
                        player.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                        PlaySound(0x202);
                        int remaining = 5 - player.TalentResets;
                        SayTo(player, "I have reset thy talents, you have " + remaining.ToString() + " resets left");
                    }
                }
                else
                {
                    base.OnSpeech(e);
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
    }
}

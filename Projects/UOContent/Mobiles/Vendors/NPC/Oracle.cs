using System.Collections.Generic;
using Server;
using Server.ContextMenus;
using Server.Gumps;
using Server.Items;
using Server.Pantheon;

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

        private void DeleteAlignmentItems(List<Item> items)
        {
            foreach (Item item in items.ToArray())
            {
                if (item is IPantheonItem pantheonItem && !string.Equals(pantheonItem.AlignmentRaw, Deity.Alignment.None.ToString()))
                {
                    item.Delete();
                }
            }
        }

        private void TryAlignmentChange(ref SpeechEventArgs e, PlayerMobile player)
        {
            e.Handled = true;
            if (player.Alignment is Deity.Alignment.None || player.DeityPoints >= 60 * 35)
            {
                player.DeityPoints = 0;
                if (player.BankBox is { Items: { } })
                {
                    DeleteAlignmentItems(player.BankBox.Items);
                }
                if (player.Backpack is { Items: { } })
                {
                    DeleteAlignmentItems(player.Backpack.Items);
                }

                player.SendGump(new ChoosePathGump(player, 2, false));
            }
            else
            {
                SayTo(player, $"You are not eligible for an alignment change. You either need {(60 * 35).ToString()} points of deity loyalty or be of no current alignment.");
            }
        }

        public void TryReset(ref SpeechEventArgs e, PlayerMobile player, bool skipConsume = false) {
            e.Handled = true;
            Container bank = player.FindBankNoCreate();
            int requiredGold = player.TalentResets * 10000;
            int remaining = 5 - player.TalentResets;
            if (remaining > 0) {
                if (skipConsume) {
                    requiredGold = 0;
                }
                if (player.Backpack?.ConsumeTotal(typeof(Gold), requiredGold) == true || Banker.Withdraw(player, requiredGold))
                {
                    player.ResetTalents();
                    player.ResetSkills();
                    remaining--;
                    player.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                    PlaySound(0x202);
                    SayTo(player, $"I have reset thy path, you have {remaining.ToString()} resets remaining");
                }
                else
                {
                    SayTo(player, $"Thou needs {requiredGold.ToString()} gold to reset thy path.");
                }
            } else {
                SayTo(player, "You have reached the allowed limit of five path resets");
            }

        }
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

                if (!e.Handled) {
                    if (string.Equals(speech, "reset path")) {
                        TryReset(ref e, player, player.TalentResets == 0);
                    } else if (string.Equals(speech, "new alignment"))
                    {
                        TryAlignmentChange(ref e, player);
                    } else if (string.Equals(speech, "seek challenge"))
                    {
                        TryChallenge(ref e, player);
                    }
                }
                else
                {
                    base.OnSpeech(e);
                }
                if (!e.Handled) {
                    SayTo(player, "I do not understand thee. If you wish to reset your path, speak 'reset path', to change your alignment, speak 'new alignment', to receive a challenge, speak 'seek challenge'");
                }
            }
        }

        private void TryChallenge(ref SpeechEventArgs speechEventArgs, PlayerMobile player)
        {
            if (player.NextDeityChallenge > Core.Now
                // || player.Level < 35
                )
            {
                SayTo(player, "You cannot face a new challenge from your deity yet.");
            }
            else
            {
                Deity.BeginChallenge(player);
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

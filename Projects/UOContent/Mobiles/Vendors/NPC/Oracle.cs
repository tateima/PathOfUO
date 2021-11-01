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

        public void TryReset(ref SpeechEventArgs e, PlayerMobile player, string type, bool skipConsume = false) {
            e.Handled = true;
            Container bank = player.FindBankNoCreate();
            int requiredGold = player.SkillResets * 20000;
            int remaining = 5;
            if (string.Equals("talent", type)) {
                requiredGold = player.TalentResets * 10000;
                remaining -= player.TalentResets;
            } else {
                remaining -= player.SkillResets;
            }
            if (remaining > 0) {
                if (skipConsume) {
                    requiredGold = 0;
                }
                if (player.Backpack?.ConsumeTotal(typeof(Gold), requiredGold) == true || Banker.Withdraw(player, requiredGold))
                {
                    if (type == "talent") {
                        player.ResetTalents();
                        player.TalentResets++;
                    } else if (type == "skill") {
                        player.ResetSkills();
                        player.SkillResets++;
                    }
                    remaining--;   
                    player.FixedParticles(0x376A, 9, 32, 5030, EffectLayer.Waist);
                    PlaySound(0x202);
                    SayTo(player, string.Format("I have reset thy {0}s, you have {1} resets remaining", type, remaining.ToString()));
                }
                else
                {
                    SayTo(player, string.Format("Thou needs {0} gold to reset thy {1}s.", requiredGold.ToString(), type)); 
                }
            } else {
                  SayTo(player, "You have reached the allowed limit of five {0} resets", type);
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
                    if (speech == "reset talents") {
                        TryReset(ref e, player, "talent", player.TalentResets == 0);
                    } else if (speech == "reset skills") {
                        TryReset(ref e, player, "skill");
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

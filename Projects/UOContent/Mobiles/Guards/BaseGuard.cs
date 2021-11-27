using Server.Items;
using Server.Talent;

namespace Server.Mobiles
{
    public abstract class BaseGuard : Mobile
    {
        public BaseGuard(Mobile target)
        {
            if (target != null)
            {
                Location = target.Location;
                Map = target.Map;

                Effects.SendLocationParticles(
                    EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x3728,
                    10,
                    10,
                    5023
                );
            }
        }

        public BaseGuard(Serial serial) : base(serial)
        {
        }

        public abstract Mobile Focus { get; set; }

        public override bool HandlesOnSpeech(Mobile from) => true;

        public override void OnSpeech(SpeechEventArgs e)
        {
            if (Deleted || !e.Mobile.CheckAlive())
            {
                return;
            }

            if (e.Mobile.InRange(this, 2))
            {
                string speech = e.Speech.ToLower();
                PlayerMobile player = (PlayerMobile)e.Mobile;
                Detective detective = player.GetTalent(typeof(Detective)) as Detective;
                CaseNote note = detective.GetPlayerCaseNote(player);
                if (!e.Handled) {
                    if (string.Equals(speech, "give me a case")) {
                        e.Handled = true;
                        if (detective != null) {
                            if (note != null) {
                                SayTo(player, "Thy already have an active case");    
                            } else {
                                detective.GiveCaseNote(player);
                                SayTo(player, "Here is a new active case");    
                            }
                        } else {
                            SayTo(player, "Thou art not talented enough for this operation");
                        }
                    } else if (string.Equals(speech, "here is my case notes")) {
                        e.Handled = true;
                        if (note != null && detective.GiveRewards(player, note)) {
                            SayTo(player, "Thank thee for your assistance, here is your reward");
                        } else {
                            SayTo(player, "Thou has no case notes to show me");
                        }
                    }
                    if (!e.Handled && detective != null) {
                        SayTo(player, "I do not understand thee. If you wish to give me a case say 'here is my case'. If you wish to receive a new case say 'give me a case'.");
                    }
                }
                else
                {
                    base.OnSpeech(e);
                }
            }            
        }

        public static void Spawn(Mobile caller, Mobile target, int amount = 1, bool onlyAdditional = false)
        {
            if (target?.Deleted != false)
            {
                return;
            }

            foreach (var m in target.GetMobilesInRange(15))
            {
                if (m is BaseGuard g)
                {
                    if (g.Focus == null) // idling
                    {
                        g.Focus = target;

                        --amount;
                    }
                    else if (g.Focus == target && !onlyAdditional)
                    {
                        --amount;
                    }
                }
            }

            while (amount-- > 0)
            {
                caller.Region.MakeGuard(target);
            }
        }

        public override bool OnBeforeDeath()
        {
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3728,
                10,
                10,
                2023
            );

            PlaySound(0x1FE);

            Delete();

            return false;
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

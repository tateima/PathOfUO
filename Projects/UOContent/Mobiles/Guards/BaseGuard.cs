using ModernUO.Serialization;
using Server.Items;
using Server.Talent;

namespace Server.Mobiles;

[SerializationGenerator(0)]
public abstract partial class BaseGuard : BaseCreature
{
    public BaseGuard() : base(AIType.AI_Melee, FightMode.None)
    {
    }

    public abstract Mobile Focus { get; set; }

    public override bool HandlesOnSpeech(Mobile from) => true;

    public override bool CanRummageCorpses => true;

    public override bool InitialInnocent => false;

    public override bool AlwaysAttackable => true;

    public override void OnSpeech(SpeechEventArgs e)
    {
        if (Deleted || !e.Mobile.CheckAlive())
        {
            return;
        }

        if (e.Mobile.InRange(this, 10))
        {
            if (!e.Handled)
            {
                if (e.Speech.ToLower().Contains("guards") && e.Mobile.Combatant is not null)
                {
                    Combatant = e.Mobile.Combatant;
                    e.Handled = true;
                }
            }
        }

        if (e.Mobile.InRange(this, 2))
        {
            string speech = e.Speech.ToLower();
            PlayerMobile player = (PlayerMobile)e.Mobile;
            Detective detective = player.GetTalent(typeof(Detective)) as Detective;
            if (detective?.HasSkillRequirement(e.Mobile) != null)
            {
                CaseNote note = Detective.GetPlayerCaseNote(player);
                if (!e.Handled)
                {
                    if (string.Equals(speech, "give me a case"))
                    {
                        e.Handled = true;
                        if (note != null)
                        {
                            SayTo(player, "Thy already have an active case");
                        }
                        else
                        {
                            Detective.GiveCaseNote(player);
                            SayTo(player, "Here is a new active case");
                        }
                    }
                    else if (string.Equals(speech, "here are my case notes"))
                    {
                        e.Handled = true;
                        if (note != null && detective.GiveRewards(player, note))
                        {
                            SayTo(player, "Thank thee for your assistance, here is your reward");
                        }
                        else
                        {
                            SayTo(player, "Thou has no case notes to show me");
                        }
                    }

                    if (!e.Handled)
                    {
                        SayTo(
                            player,
                            "I do not understand thee. If you wish to give me a case say 'here is my case'. If you wish to receive a new case say 'give me a case'."
                        );
                    }
                }
            }
            else
            {
                base.OnSpeech(e);
            }
        }
    }

    public AIType AiType { get; set; }

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
}

using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Pantheon;
using Server.Targeting;

namespace Server.Talent
{
    public class Zealot : BaseTalent
    {
        public Zealot()
        {
            StatModNames = new[] { "Zealot" };
            DisplayName = "Zealot";
            DeityAlignment = Deity.Alignment.Light;
            RequiresDeityFavor = true;
            CanBeUsed = true;
            Description =
                "Empowers the stats, skills and weapon swing speed of an ally for 60 seconds.";
            AdditionalDetail = "This empowerment increases by 5% per level.";
            ImageID = 415;
            CooldownSeconds = 180;
            ManaRequired = 40;
            GumpHeight = 230;
            AddEndY = 70;
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown)
            {
                if (from.Mana > ManaRequired)
                {
                    from.Target = new InternalTarget(this);
                }
                else
                {
                    from.SendMessage($"You need {ManaRequired.ToString()} mana to use {DisplayName}.");
                }
            }
        }
        private class InternalTarget : Target
        {
            private readonly Zealot _zealot;
            private Mobile _ally;
            private List<DefaultSkillMod> _skillMods;

            public InternalTarget(Zealot zealot) : base(
                10,
                false,
                TargetFlags.None
            ) =>
                _zealot = zealot;

            private void ExpireBuff()
            {
                foreach (var skillMod in _skillMods)
                {
                    _ally.RemoveSkillMod(skillMod);
                }
                CheckSetTalentEffect(_ally, null);
            }
            protected override void OnTarget(Mobile from, object targeted)
            {
                from.RevealingAction();
                if (targeted is Mobile target)
                {
                    if (Core.AOS && !target.InLOS(from))
                    {
                        from.SendMessage("Thou cannot send target into a zealous rage.");
                    }
                    else
                    {
                        var validTarget = Deity.CanReceiveAlignment(target, Deity.Alignment.Light);
                        if (validTarget)
                        {
                            List<Skill> skills = new List<Skill>();
                            _ally = target;
                            // get their three highest skills and increase them
                            GetTopSkills(_ally, ref skills, 3);
                            _skillMods = GetTopDefaultSkillMods(skills, 5.0 * _zealot.Level, "Zealot");
                            foreach (var skillMod in _skillMods)
                            {
                                _ally.AddSkillMod(skillMod);
                            }
                            CheckSetTalentEffect(_ally, _zealot);

                            _ally.AddStatMod(new StatMod(StatType.All, _zealot.StatModNames[0], _zealot.Level * 5, TimeSpan.FromSeconds(60)));
                            _ally.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
                            _ally.PlaySound(0x1EA);

                            _zealot.OnCooldown = true;
                            Timer.StartTimer(TimeSpan.FromSeconds(60), ExpireBuff);
                            Timer.StartTimer(TimeSpan.FromSeconds(_zealot.CooldownSeconds), _zealot.ExpireTalentCooldown, out _zealot._talentTimerToken);
                        }
                        else
                        {
                            from.SendMessage("This target is of the wrong deity alignment.");
                        }
                    }
                }
            }
        }
    }
}

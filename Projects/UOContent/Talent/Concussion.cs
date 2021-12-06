using System;
using Server.Items;

namespace Server.Talent
{
    public class Concussion : BaseTalent
    {
        public Concussion()
        {
            TalentDependency = typeof(MaceSpecialist);
            RequiredWeaponSkill = SkillName.Macing;
            RequiredWeapon = new[]
            {
                typeof(Mace), typeof(Maul), typeof(Club), typeof(DiamondMace), typeof(MagicWand), typeof(HammerPick),
                typeof(Scepter), typeof(WarMace)
            };
            DisplayName = "Concussion";
            CanBeUsed = true;
            Description = "Next hit drains 50% + level*2 mana for 20 seconds. 1 min cooldown.";
            ImageID = 167;
            GumpHeight = 75;
            AddEndY = 95;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, int damage)
        {
            if (Activated)
            {
                Activated = false;
                OnCooldown = true;
                target.FixedParticles(0x3779, 10, 15, 5004, EffectLayer.Head);
                target.PlaySound(0x1E4);
                target.AddStatMod(
                    new StatMod(
                        StatType.Int,
                        "ConcussionTalent",
                        -AOS.Scale(target.RawInt, 50 + Level * 2),
                        TimeSpan.FromSeconds(20)
                    )
                );
                Timer.StartTimer(TimeSpan.FromSeconds(60), ExpireTalentCooldown, out _talentTimerToken);
            }
        }

        public override void OnUse(Mobile from)
        {
            var weapon = from.Weapon as BaseWeapon;
            if (weapon is Mace or Maul or Club or DiamondMace or MagicWand or HammerPick or Scepter or WarMace)
            {
                base.OnUse(from);
            }
            else
            {
                from.SendMessage("You do not have a one handed mace equipped.");
            }
        }
    }
}

using System;
using Server.Items;

namespace Server.Talent
{
    public class CranialStrike : BaseTalent
    {
        public CranialStrike()
        {
            TalentDependencies = new[] { typeof(MaceSpecialist) };
            RequiredWeaponSkill = SkillName.Macing;
            RequiredWeapon = new[]
            {
                typeof(Mace), typeof(Maul), typeof(Club), typeof(DiamondMace), typeof(MagicWand), typeof(HammerPick),
                typeof(Scepter), typeof(WarMace)
            };
            DisplayName = "Cranial strike";
            CanBeUsed = true;
            Description = "Next hit drains 50% + level * 2 mana for 20 seconds.";
            StamRequired = 15;
            ImageID = 167;
            CooldownSeconds = 60;
            GumpHeight = 75;
            AddEndY = 95;
        }

        public override void CheckHitEffect(Mobile attacker, Mobile target, ref int damage)
        {
            if (Activated && attacker.Stam >= StamRequired + 1)
            {
                Activated = false;
                OnCooldown = true;
                ApplyStaminaCost(attacker);
                target.FixedParticles(0x3779, 10, 15, 5004, EffectLayer.Head);
                target.PlaySound(0x22C);
                target.AddStatMod(
                    new StatMod(
                        StatType.Int,
                        "CranialStrike",
                        -AOS.Scale(target.RawInt, 50 + Level * 2),
                        TimeSpan.FromSeconds(20)
                    )
                );
                Timer.StartTimer(TimeSpan.FromSeconds(CooldownSeconds), ExpireTalentCooldown, out _talentTimerToken);
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

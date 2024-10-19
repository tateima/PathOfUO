using System;
using Server.Spells;
using Server.Pantheon;
using Server.Spells.Fourth;
using Server.Targeting;

namespace Server.Talent
{
    public class FlameWave : BaseTalent
    {
        public FlameWave()
        {
            DisplayName = "Flame wave";
            DeityAlignment = Deity.Alignment.Chaos;
            RequiresDeityFavor = true;
            CanBeUsed = true;
            Description =
                "Send a wave of flame towards a target.";
            AdditionalDetail = "The duration of the flames increases by 5 seconds per level. Only players with chaotic alignment can use this.";
            ImageID = 413;
            CooldownSeconds = 120;
            ManaRequired = 50;
            GumpHeight = 230;
            AddEndY = 70;
        }

        public override void OnUse(Mobile from)
        {
            if (!OnCooldown && HasSkillRequirement(from))
            {
                if (from.Mana > ManaRequired)
                {
                    from.Target = new InternalTarget(from, this);
                }
                else
                {
                    from.SendMessage($"You need {ManaRequired.ToString()} mana to summon this wave of flame.");
                }
            }
            else
            {
                from.SendMessage(FailedRequirements);
            }
        }
        private class InternalTarget : Target
        {
            private readonly FlameWave _flameWave;
            private readonly Mobile _mobile;

            public InternalTarget(Mobile from, FlameWave flameWave) : base(
                10,
                false,
                TargetFlags.None
            )
            {
                _flameWave = flameWave;
                _mobile = from;
            }


            protected override void OnTarget(Mobile from, object targeted)
            {
                from.RevealingAction();
                if (targeted is Mobile target)
                {
                    if (target == _mobile || !target.CanBeHarmful(from, false) ||
                        Core.AOS && !target.InLOS(from))
                    {
                        from.SendMessage("Thou cannot cast a flame wave at this target");
                    } else if (SpellHelper.CheckTown(target.Location, from))
                    {
                        _flameWave.ApplyManaCost(from);
                        IPoint3D point = target.Location;
                        SpellHelper.Turn(from, target.Location);

                        SpellHelper.GetSurfaceTop(ref point);

                        var loc = new Point3D(point);

                        var eastToWest = SpellHelper.GetEastToWest(from.Location, loc);

                        from.Direction =  from.GetDirectionTo(point);

                        var distance = eastToWest ? Math.Abs(loc.Y - from.Location.Y) : Math.Abs(loc.X - from.Location.X);
                        if (distance > 10)
                        {
                            distance = 10;
                        }
                        var itemID = eastToWest ? 0x398C : 0x3996;
                        var targetLoc = CalculatePushbackFromAnchor(from.Location, 1, target);
                        for (int i = 0; i < distance; i++)
                        {
                            var duration = TimeSpan.FromSeconds(25 + 5 * _flameWave.Level);
                            var jStart = -1;
                            var jEnd = 0;
                            if (Utility.RandomBool())
                            {
                                jStart = 0;
                                jEnd = 1;
                            }

                            if (Utility.RandomBool())
                            {
                                jStart = -1;
                                jEnd = 1;
                            }

                            for (var j = jStart; j <= jEnd; ++j)
                            {
                                Effects.PlaySound(loc, from.Map, 0x20C);
                                var chaoticLoc = new Point3D(eastToWest ? targetLoc.X + j : targetLoc.X, eastToWest ? targetLoc.Y : targetLoc.Y + j, targetLoc.Z);
                                new FireFieldItem(itemID, chaoticLoc, from, from.Map, duration, j);
                            }
                            targetLoc = CalculatePushbackFromAnchor(targetLoc, 1, target);
                        }
                        _flameWave.OnCooldown = true;
                        Timer.StartTimer(TimeSpan.FromSeconds(_flameWave.CooldownSeconds), _flameWave.ExpireTalentCooldown, out _flameWave._talentTimerToken);
                    }
                }
            }
        }
    }
}

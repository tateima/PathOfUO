using System;
using Server.Collections;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Targeting;
using Server.Talent;

namespace Server.Spells.Fourth
{
    public class FireFieldSpell : MagerySpell, ISpellTargetingPoint3D
    {
        private static readonly SpellInfo _info = new(
            "Fire Field",
            "In Flam Grav",
            215,
            9041,
            false,
            Reagent.BlackPearl,
            Reagent.SpidersSilk,
            Reagent.SulfurousAsh
        );

        public FireFieldSpell(Mobile caster, Item scroll = null) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Fourth;

        public void Target(IPoint3D p)
        {
            if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);

                SpellHelper.GetSurfaceTop(ref p);

                var loc = new Point3D(p);

                var eastToWest = SpellHelper.GetEastToWest(Caster.Location, loc);

                Effects.PlaySound(loc, Caster.Map, 0x20C);

                var itemID = eastToWest ? 0x398C : 0x3996;

                var duration = Core.AOS
                    ? TimeSpan.FromSeconds((15 + Caster.Skills.Magery.Fixed / 5.0) / 4.0)
                    : TimeSpan.FromSeconds(4.0 + Caster.Skills.Magery.Value * 0.5);

                for (var i = -2; i <= 2; ++i)
                {
                    var targetLoc = new Point3D(eastToWest ? loc.X + i : loc.X, eastToWest ? loc.Y : loc.Y + i, loc.Z);

                    new FireFieldItem(itemID, targetLoc, Caster, Caster.Map, duration, i);
                }
            }

            FinishSequence();
        }

        public override void OnCast()
        {
            Caster.Target = new SpellTargetPoint3D(this, range: Core.ML ? 10 : 12);
        }

        [DispellableField]
        public class FireFieldItem : Item
        {
            private Mobile m_Caster;
            private int m_Damage;
            private DateTime m_End;
            private Timer m_Timer;
            private int m_Fire;
            private int m_Cold;
            private int m_Hue;

            public FireFieldItem(
                int itemID, Point3D loc, Mobile caster, Map map, TimeSpan duration, int val,
                int damage = 2
            ) : base(itemID)
            {
                var canFit = SpellHelper.AdjustField(ref loc, map, 12, false);

                Visible = false;
                Movable = false;
                Light = LightType.Circle300;

                MoveToWorld(loc, map);

                m_Caster = caster;
                m_Fire = 100;
                m_Cold = 0;
                m_Hue = 0;
                if (caster is PlayerMobile player)
                {
                    BaseTalent fireAffinity = player.GetTalent(typeof(FireAffinity));
                    if (fireAffinity != null)
                    {
                        damage += fireAffinity.ModifySpellMultiplier();
                    }
                    BaseTalent frostFire = player.GetTalent(typeof(FrostFire));
                    if (frostFire != null) {
                        ((FrostFire)frostFire).ModifyFireSpell(ref m_Fire, ref m_Cold, null, hue: ref m_Hue);
                    }
                }
                m_Damage = damage;

                m_End = Core.Now + duration;
                m_Timer = new InternalTimer(this, TimeSpan.FromSeconds(val.Abs() * 0.2), caster.InLOS(this), canFit, m_Hue, m_Fire, m_Cold);
                m_Timer.Start();
            }

            public FireFieldItem(Serial serial) : base(serial)
            {
            }

            public override bool BlocksFit => true;

            public override void OnAfterDelete()
            {
                base.OnAfterDelete();

                m_Timer?.Stop();
            }

            public override void Serialize(IGenericWriter writer)
            {
                base.Serialize(writer);

                writer.Write(3); // version
                writer.Write(m_Hue);
                writer.Write(m_Fire);
                writer.Write(m_Cold);
                writer.Write(m_Damage);
                writer.Write(m_Caster);
                writer.WriteDeltaTime(m_End);
            }

            public override void Deserialize(IGenericReader reader)
            {
                base.Deserialize(reader);

                var version = reader.ReadInt();
                m_Hue = 0;
                m_Fire = 100;
                m_Cold = 0;

                switch (version)
                {
                    case 3:
                        {
                            m_Hue = reader.ReadInt();
                            m_Fire = reader.ReadInt();
                            m_Cold = reader.ReadInt();
                            goto case 2;
                        }
                    case 2:
                        { 
                            m_Damage = reader.ReadInt();
                            goto case 1;
                        }
                    case 1:
                        {
                            m_Caster = reader.ReadEntity<Mobile>();

                            goto case 0;
                        }
                    case 0:
                        {
                            m_End = reader.ReadDeltaTime();

                            m_Timer = new InternalTimer(this, TimeSpan.Zero, true, true, m_Hue, m_Fire, m_Cold);
                            m_Timer.Start();

                            break;
                        }
                }

                if (version < 2)
                {
                    m_Damage = 2;
                }
            }

            public override bool OnMoveOver(Mobile m)
            {
                if (Visible && m_Caster != null && (!Core.AOS || m != m_Caster) &&
                    SpellHelper.ValidIndirectTarget(m_Caster, m) && m_Caster.CanBeHarmful(m, false))
                {
                    if (SpellHelper.CanRevealCaster(m))
                    {
                        m_Caster.RevealingAction();
                    }

                    m_Caster.DoHarmful(m);

                    var damage = m_Damage;

                    if (!Core.AOS && m.CheckSkill(SkillName.MagicResist, 0.0, 30.0))
                    {
                        damage = 1;

                        m.SendLocalizedMessage(501783); // You feel yourself resisting magical energy.
                    }
                    if (m_Cold > 0)
                    {
                        if (m is BaseCreature creature)
                        {
                            creature.ActiveSpeed /= 2;
                        } else if (m is PlayerMobile player)
                        {
                            player.Slow(Utility.Random(6));
                        }
                    }
                    AOS.Damage(m, m_Caster, damage, 0, m_Fire, m_Cold, 0, 0);
                    m.PlaySound(0x208);

                    (m as BaseCreature)?.OnHarmfulSpell(m_Caster);
                }

                return true;
            }

            private class InternalTimer : Timer
            {
                private readonly bool m_CanFit;
                private readonly bool m_InLOS;
                private readonly FireFieldItem m_Item;

                private readonly int m_Hue;
                private readonly int m_Fire;
                private readonly int m_Cold;

                public InternalTimer(FireFieldItem item, TimeSpan delay, bool inLOS, bool canFit, int hue, int fire, int cold) : base(delay, TimeSpan.FromSeconds(1.0))
                {
                    m_Item = item;
                    m_InLOS = inLOS;
                    m_CanFit = canFit;
                    m_Hue = hue;
                    m_Fire = fire;
                    m_Cold = cold;
                }

                protected override void OnTick()
                {
                    if (m_Item.Deleted)
                    {
                        return;
                    }

                    if (!m_Item.Visible)
                    {
                        if (m_InLOS && m_CanFit)
                        {
                            m_Item.Visible = true;
                        }
                        else
                        {
                            m_Item.Delete();
                        }

                        if (!m_Item.Deleted)
                        {
                            m_Item.ProcessDelta();
                            Effects.SendLocationParticles(
                                EffectItem.Create(m_Item.Location, m_Item.Map, EffectItem.DefaultDuration),
                                0x376A,
                                9,
                                10,
                                m_Hue,
                                0,
                                5029,
                                0
                            );
                        }
                    }
                    else if (Core.Now > m_Item.m_End)
                    {
                        m_Item.Delete();
                        Stop();
                    }
                    else
                    {
                        var map = m_Item.Map;
                        var caster = m_Item.m_Caster;

                        if (map == null || caster == null)
                        {
                            return;
                        }

                        using var queue = PooledRefQueue<Mobile>.Create();
                        foreach (var m in m_Item.GetMobilesInRange(0))
                        {
                            if (m.Z + 16 > m_Item.Z && m_Item.Z + 12 > m.Z && (!Core.AOS || m != caster) &&
                                SpellHelper.ValidIndirectTarget(caster, m) && caster.CanBeHarmful(m, false))
                            {
                                queue.Enqueue(m);
                            }
                        }

                        while (queue.Count > 0)
                        {
                            var m = queue.Dequeue();
                            if (m == null)
                            {
                                continue;
                            }

                            if (SpellHelper.CanRevealCaster(m))
                            {
                                caster.RevealingAction();
                            }

                            caster.DoHarmful(m);

                            var damage = m_Item.m_Damage;

                            if (!Core.AOS && m.CheckSkill(SkillName.MagicResist, 0.0, 30.0))
                            {
                                damage = 1;

                                m.SendLocalizedMessage(501783); // You feel yourself resisting magical energy.
                            }

                            AOS.Damage(m, caster, damage, 0, m_Fire, m_Cold, 0, 0);
                            m.PlaySound(0x208);

                            (m as BaseCreature)?.OnHarmfulSpell(caster);
                        }
                    }
                }
            }
        }
    }
}

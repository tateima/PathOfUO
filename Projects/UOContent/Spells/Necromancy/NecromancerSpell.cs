using Server.Items;
using Server.Talent;
using Server.Mobiles;

namespace Server.Spells.Necromancy;

public abstract class NecromancerSpell : Spell
{
    private BaseTalent _darkAffinity;
    public BaseTalent DarkAffinity
    {
        get { return _darkAffinity; }
        set { _darkAffinity = value; }
    }

    private BaseTalent _pellMind;
    public BaseTalent SpellMind
    {
        get { return _pellMind; }
        set { _pellMind = value; }
    }
    public NecromancerSpell(Mobile caster, Item scroll, SpellInfo info) : base(caster, scroll, info)
    {
        if (Caster is PlayerMobile player)
        {
            DarkAffinity = player.GetTalent(typeof(DarkAffinity));
            SpellMind = player.GetTalent(typeof(SpellMind));
        }
    }

    public abstract double RequiredSkill { get; }
    public abstract int RequiredMana { get; }

    public override SkillName CastSkill => SkillName.Necromancy;
    public override SkillName DamageSkill => SkillName.SpiritSpeak;

    public override bool ClearHandsOnCast => false;

    // Necromancer spells are not affected by fast cast items, though they are by fast cast recovery
    public override double CastDelayFastScalar => Core.SE ? base.CastDelayFastScalar : 0;

    public override int ComputeKarmaAward()
    {
        // TODO: Verify this formula being that Necro spells don't HAVE a circle.
        // int karma = -(70 + (10 * (int)Circle));
        var karma = -(40 + (int)(10 * (CastDelayBase.TotalSeconds / CastDelaySecondsPerTick)));

        // Pub 36: "Added a new property called Increased Karma Loss which grants higher karma loss for casting necromancy spells."
        if (Core.ML)
        {
            karma += AOS.Scale(karma, AosAttributes.GetValue(Caster, AosAttribute.IncreasedKarmaLoss));
        }

        return karma;
    }

    public override void GetCastSkills(out double min, out double max)
    {
        min = RequiredSkill;
        max = Scroll != null ? min : RequiredSkill + 40.0;
    }

    public bool CheckSpellMind()
    {
        return SpellMind != null;
    }

    public void SpellMindDamage(ref int damage)
    {
        if (CheckSpellMind())
        {
            damage += SpellMind.Level;
        }
    }
    public void SpellMindDamage(ref double damage)
    {
        if (CheckSpellMind())
        {
            damage += SpellMind.Level;
        }
    }

    public void SpellMindScalar(ref double scalar)
    {
        if (CheckSpellMind())
        {
            scalar += SpellMind.ModifySpellScalar();
        }
    }
    public bool CheckDarkAffinity()
    {
        return DarkAffinity != null;
    }
    public void DarkAffinityScalar(ref double scalar)
    {
        if (CheckDarkAffinity())
        {
            scalar += DarkAffinity.ModifySpellScalar();
        }
    }
    public void DarkAffinityDamage(ref int damage)
    {
        if (CheckDarkAffinity())
        {
            // increase damage by fixed multiplier
            damage += DarkAffinity.Level;
        }
    }

    public void DarkAffinityDamage(ref double damage)
    {
        if (CheckDarkAffinity())
        {
            // increase damage by fixed multiplier
            damage += DarkAffinity.Level;
        }
    }

    public int DarkAffinityDuration()
    {
        if (CheckDarkAffinity())
        {
            return DarkAffinity.Level*3;
        }
        return 0;
    }

    public override bool ConsumeReagents() => base.ConsumeReagents() || ArcaneGem.ConsumeCharges(Caster, 1);

    public override int GetMana() => RequiredMana;
}

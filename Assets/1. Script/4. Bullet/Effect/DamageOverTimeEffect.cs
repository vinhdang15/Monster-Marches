public class DamageOverTimeEffect : EffectBase
{
    public DamageOverTimeEffect(string type, float value, float duration, int occursTime, float range)
    {
        base.Init(type, value, duration, occursTime,range);
    }

    public override void Apply(UnitBase enemy)
    {
        enemy.ApplyDamageOverTimeCoroutine(this, type, value, duration, occursTime, range);
    }
}

using System.Collections.Generic;

public static class CreateItemEffectName 
{
    // Start is called before the first frame update
    private static readonly Dictionary<ItemEffect, string> effectNames = new Dictionary<ItemEffect, string>
    {
        { ItemEffect.AttackDamage, "공격력" },
        { ItemEffect.AttackDelay, "공격 딜레이" },
        { ItemEffect.AttackSpeed, "공격 속도" },
        { ItemEffect.ConditionPower, "상태이상 위력" },
        { ItemEffect.ConditionTime, "상태이상 지속시간" },
        { ItemEffect.RollingSpeed, "구르기 속도" },
        { ItemEffect.RollDuration, "구르기 거리" },
        { ItemEffect.None, "없음" }
    };

    public static string GetName(ItemEffect effect)
    {
        //TryGetValue 존재할때 반환 없다면 대채 코드 수행
        // 사용이유 ->예외없이 안전하게 처리가 가능
        return effectNames.TryGetValue(effect, out var name) ? name : effect.ToString();
    }
}

using System.Collections.Generic;

public static class CreateItemEffectName 
{
    // Start is called before the first frame update
    private static readonly Dictionary<ItemEffect, string> effectNames = new Dictionary<ItemEffect, string>
    {
        { ItemEffect.AttackDamage, "���ݷ�" },
        { ItemEffect.AttackDelay, "���� ������" },
        { ItemEffect.AttackSpeed, "���� �ӵ�" },
        { ItemEffect.ConditionPower, "�����̻� ����" },
        { ItemEffect.ConditionTime, "�����̻� ���ӽð�" },
        { ItemEffect.RollingSpeed, "������ �ӵ�" },
        { ItemEffect.RollDuration, "������ �Ÿ�" },
        { ItemEffect.None, "����" }
    };

    public static string GetName(ItemEffect effect)
    {
        //TryGetValue �����Ҷ� ��ȯ ���ٸ� ��ä �ڵ� ����
        // ������� ->���ܾ��� �����ϰ� ó���� ����
        return effectNames.TryGetValue(effect, out var name) ? name : effect.ToString();
    }
}

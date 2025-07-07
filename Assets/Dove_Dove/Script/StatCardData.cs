using System.Collections.Generic;
using UnityEngine;

//��ȹ�������� ���� �ߴ� (��Ÿ ���� �� ���� ������ ������ ȯ�濡 �°� ����)

[System.Flags]
public enum StatCardEffect
{
    None = 0,
    Cuting = 1 << 1,
    Thunder = 1 << 2,
    HolyCross = 1 << 3,
}



[CreateAssetMenu(fileName = "Stat Card Data", menuName = "Scriptable/Stat Card Data", order = int.MaxValue)]

public class StatCardData : ScriptableObject
{
    [SerializeField]
    private string statCardName;
    public string StatCardName => statCardName;
    //    public string StatCardName {  get { return statCardName; }} ������ �̰� �б� ���뿡�� ���� ����


    [SerializeField]
    private Sprite statCardImg;
    public Sprite StatCardImg => statCardImg;

    [SerializeField]
    private string statExplanation;
    public string StatExplanation => statExplanation;

    //����Ʈȭ �Ѱ�
    [System.Serializable]
    public class StatEffectData
    {
        public StatCardEffect effectType; // 
        public float value;               //
    }

    [SerializeField]
    private List<StatEffectData> effects;
    public List<StatEffectData> Effects => effects;
}

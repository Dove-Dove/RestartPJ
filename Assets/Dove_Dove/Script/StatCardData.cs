using System.Collections.Generic;
using UnityEngine;

//계획변경으로 인한 중단 (기타 오류 및 수정 사항이 있으면 환경에 맞게 수정)

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
    //    public string StatCardName {  get { return statCardName; }} 간결형 이고 읽기 전용에서 자주 나옴


    [SerializeField]
    private Sprite statCardImg;
    public Sprite StatCardImg => statCardImg;

    [SerializeField]
    private string statExplanation;
    public string StatExplanation => statExplanation;

    //리스트화 한것
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

using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum StatCardEffect
{
    None = 0,
    Power = 1 << 0,
    Speed = 1 << 1,
    MaxHP = 1 << 2,

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

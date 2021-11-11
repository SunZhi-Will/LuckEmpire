using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Unity Royale/Card Data")]
public class CardData : ScriptableObject
{
    /// <summary>
    /// 定義消耗類別
    /// 行動、魔力
    /// </summary>
    public enum ConsumptionType{
        Action,
        Magic
    };
    /// <summary>
    /// 消耗類別
    /// </summary>
    public ConsumptionType m_consumptionType;
    /// <summary>
    /// 消耗
    /// </summary>
    public int consume;

    /// <summary>
    /// 卡牌ID
    /// </summary>
    public int cardId;
    
    /// <summary>
    /// 效果數值
    /// </summary>
    public EffectData[] cardEffect;
}

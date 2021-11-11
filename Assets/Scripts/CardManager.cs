using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{   
    /// <summary>
    /// 玩家牌組
    /// </summary>
    public RoleData PlayDeck;

    /// <summary>
    /// 抽牌效果
    /// </summary>
    private HandCards handCards;

    private void Awake() {
        handCards = GetComponent<HandCards>();
    }
}

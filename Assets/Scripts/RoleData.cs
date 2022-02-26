using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRole", menuName = "Unity Royale/Role Data")]
/// <summary>
/// 角色檔案
/// </summary>
public class RoleData : ScriptableObject
{
    /// <summary>
    /// 角色名字
    /// </summary>
    public string roleName;
    /// <summary>
    /// 角色說明
    /// </summary>
    public string roleExplanation;
    public Sprite roleSprite;

    /// <summary>
    /// 初始卡牌
    /// </summary>
    public CardData[] cardData;

    /// <summary>
    /// 最大行動值
    /// </summary>
    public int maxActionValue;

    /// <summary>
    /// 魔力值
    /// </summary>
    public int magicValue;

    /// <summary>
    /// 最大血量
    /// </summary>
    public int maxHP;

    /// <summary>
    /// 抽牌數
    /// </summary>
    public int numberCardsDrawn;
    

    /// <summary>
    /// 未用卡牌
    /// </summary>
    public List<CardData> unusedCards;
    /// <summary>
    /// 現在卡牌
    /// </summary>
    public List<CardData> nowCard;
    /// <summary>
    /// 棄牌區
    /// </summary>
    public List<CardData> tombCard;
    /// <summary>
    /// 現在血量
    /// </summary>
    public int hp;
    /// <summary>
    /// 行動值
    /// </summary>
    public int actionValue;


    public RoleData RoleDataCopy(){
        return (RoleData)this.MemberwiseClone();
    }
    public void Initialization(){
        unusedCards.Clear();
        nowCard.Clear();
        tombCard.Clear();
        hp = maxHP;
        actionValue = maxActionValue;
    }


}

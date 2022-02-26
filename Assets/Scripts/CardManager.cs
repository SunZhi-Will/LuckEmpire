#define debug

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardManager : MonoBehaviour
{   
    /// <summary>
    /// 整體紀錄
    /// </summary>
    public SiteDate playSiteDeck;

    [Header("玩家")]
    public Text nameText;
    public Text hpText;
    public Text actionValueText;
    public Text mpText;
    public Text harmText;

    [Header("敵人")]
    public Text enemyNameText;
    public Text enemyHpText;
    public Text enemyActionValueText;
    public Text enemyMpText;
    public Text enemyHarmText;
    
    /// <summary>
    /// 玩家牌組
    /// </summary>
    private RoleData playDeck;
    /// <summary>
    /// 敵人牌組
    /// </summary>
    private RoleData enemyDeck;

    /// <summary>
    /// 抽牌效果
    /// </summary>
    private HandCards handCards;

    /// <summary>
    /// 手牌資訊
    /// </summary>
    /// <typeparam name="CardInformation">手牌類別</typeparam>
    private List<CardUIEffect> cardUIEffect = new List<CardUIEffect>();

    /// <summary>
    /// 是否為玩家回合
    /// </summary>
    private bool playerTurn = true;

    private void Awake() {
        #region 測試用
        #if debug
            playSiteDeck.Initialization();
            playDeck = playSiteDeck.GetRoleData();
            enemyDeck = playSiteDeck.GetEnemyRoleDate(0);
        #endif
        #endregion

        handCards = GetComponent<HandCards>();

        playDeck.Initialization();
        enemyDeck.Initialization();

        foreach (var item in playDeck.cardData)
        {
            playDeck.unusedCards.Add(item);
        }
        foreach (var item in enemyDeck.cardData)
        {
            enemyDeck.unusedCards.Add(item);
        }
        


        UIDisplayUpdate();
    }

    private void Start() {
        handCards.OnUseCards += UseCards;
        RoundStart(playDeck);
        
    }
    /// <summary>
    /// UI顯示更新
    /// </summary>
    private void UIDisplayUpdate() {
        nameText.text = playDeck.roleName;
        hpText.text = playDeck.hp + "/" + playDeck.maxHP;
        actionValueText.text = playDeck.actionValue.ToString();
        mpText.text = playDeck.magicValue.ToString();

        enemyNameText.text = enemyDeck.roleName;
        enemyHpText.text = enemyDeck.hp + "/" + enemyDeck.maxHP;
        enemyActionValueText.text = enemyDeck.actionValue.ToString();
        enemyMpText.text = enemyDeck.magicValue.ToString();
    }
    /// <summary>
    /// 傷害顯示
    /// </summary>
    private void DamageDisplay(int _damage){
        string _str = _damage.ToString();
        if(_damage > 0){
            _str = "+" + _str;
        }
        if(playerTurn){
            enemyHarmText.text = _str;
            StartCoroutine(ShowAnimationTemporarily(enemyHarmText));
        }else{
            harmText.text = _str;
            StartCoroutine(ShowAnimationTemporarily(harmText));
        }
        
    }
    /// <summary>
    /// 暫時顯示動畫
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowAnimationTemporarily(Text _harmText){
        GameObject _HarmTextGo = Instantiate(_harmText.gameObject, enemyHarmText.transform.parent);
        _HarmTextGo.SetActive(true);
        Tweener tweener = _HarmTextGo.transform.DOMove(_HarmTextGo.transform.position + new Vector3(0f, 50f, 0f),1f);
        yield return new WaitForSeconds(1f);
        Destroy(_HarmTextGo);
    }

    /// <summary>
    /// 回合切換
    /// </summary>
    public void RoundSwitch(){
        
        CardBan();
        
        playerTurn = !playerTurn;
        RoundStart(playerTurn ? playDeck : enemyDeck);

        if(!playerTurn){
            Debug.Log("-------敵人回合-------");
            List<CardData> _enemyCard = ConsumptionJudgment(enemyDeck.nowCard, enemyDeck);
            
            //可否出牌
            bool _canCardBePlayed = _enemyCard.Count > 0;
            int i = 0;
            while (_canCardBePlayed && i !=50)
            {
                int _r = Random.Range(0, _enemyCard.Count);
                UseCards(_enemyCard[_r]);
                enemyDeck.nowCard.Remove(_enemyCard[_r]);
                _enemyCard = ConsumptionJudgment(enemyDeck.nowCard, enemyDeck);
                _canCardBePlayed = _enemyCard.Count > 0;
                i++;
            }
            
            Debug.Log("-------敵人出牌結束-------");
            RoundSwitch(); 
        }
    }

    /// <summary>
    /// 回合開始
    /// </summary>
    public void RoundStart(RoleData _thisTheRole){
        _thisTheRole.actionValue = _thisTheRole.maxActionValue;

        UIDisplayUpdate();
        if(playerTurn){
            JudgmentAvailable();
        }

        for (int i = 0; i < _thisTheRole.numberCardsDrawn; i++)
        {
            if(_thisTheRole.unusedCards.Count > 0){
                Debug.Log("剩餘抽牌數:" + _thisTheRole.unusedCards.Count);

                int _r = Random.Range(0, _thisTheRole.unusedCards.Count);

                //玩家抽卡
                if(playerTurn){
                    CardUIEffect _cardInformation = handCards.Extract(_thisTheRole.unusedCards[_r]);
                    PlayerCardEffect(_cardInformation);
                }
                //AI抽卡
                _thisTheRole.nowCard.Add(_thisTheRole.unusedCards[_r]);
                
                
                
                
                _thisTheRole.unusedCards.Remove(_thisTheRole.unusedCards[_r]);
            }else if(_thisTheRole.tombCard.Count > 0){
                _thisTheRole.unusedCards = new List<CardData>(_thisTheRole.tombCard);
                _thisTheRole.tombCard.Clear();
                i -=1 ;
            }else{
                break;
            }
        }
        
    }
    /// <summary>
    /// 玩家卡牌效果
    /// 這是測試版，修改版需優化這塊。
    /// </summary>
    private void PlayerCardEffect(CardUIEffect _cardInformation){
        switch ((int)_cardInformation.thisCardData.m_consumptionType)
        {
            case 0:
                _cardInformation.ActivateCard(_cardInformation.thisCardData.consume <= playDeck.actionValue);
            break;

            case 1:
                _cardInformation.ActivateCard(_cardInformation.thisCardData.consume <= playDeck.magicValue);
            break;

            default:
            break;
        }
        cardUIEffect.Add(_cardInformation); 
    }

    /// <summary>
    /// 使用卡片
    /// </summary>
    private void UseCards(CardData _cardDatas){
        
        RoleData _attackPlay;
        RoleData _defensive;
        if(playerTurn){
            _attackPlay = playDeck;
            _defensive = enemyDeck;
        }else{
            _attackPlay = enemyDeck;
            _defensive = playDeck;
        }
        


        switch ((int)_cardDatas.m_consumptionType)
        {
            case 0: //消耗體力
                _attackPlay.actionValue -= _cardDatas.consume;
            break;

            case 1: //消耗魔力
                _attackPlay.magicValue -= _cardDatas.consume;
            break;

            default:
            break;
        }

        //使用卡片
        foreach (var item in _cardDatas.cardEffect)
        {
            switch (item.effectCategory)
            {
                case 0: //攻擊
                    _defensive.hp -= item.effectValue;
                    DamageDisplay(-item.effectValue);
                    Debug.Log(_attackPlay.roleName + " 攻擊 " + _defensive.roleName + " 造成 " + item.effectValue + " 傷害， " + _defensive.roleName + " 血量剩 " +_defensive.hp);
                break;
                default:
                break;
            }
        }
        
        //使用完卡片
        _attackPlay.tombCard.Add(_cardDatas);
        _attackPlay.nowCard.Remove(_cardDatas);
        
        UIDisplayUpdate();
        JudgmentAvailable();
    }
    /// <summary>
    /// 判斷卡牌可用度
    /// </summary>
    private void JudgmentAvailable(){
        foreach (CardUIEffect item in cardUIEffect)
        {
            if(item == null){
                
            }else{
                switch ((int)item.thisCardData.m_consumptionType)
                {
                    case 0:
                        item.ActivateCard(item.thisCardData.consume <= playDeck.actionValue);
                    break;

                    case 1:
                        item.ActivateCard(item.thisCardData.consume <= playDeck.magicValue);
                    break;

                    default:
                    break;
                }
            }
        }
    }
    /// <summary>
    /// 卡片禁用
    /// </summary>
    private void CardBan(){
        foreach (CardUIEffect item in cardUIEffect)
        {
            if(item == null){
                
            }else{
                item.ActivateCard(false);
            }
            
        }
    }

    /// <summary>
    /// 消耗判斷
    /// </summary>
    /// <param name="_nowCard">手牌</param>
    private List<CardData> ConsumptionJudgment(List<CardData> _nowCard, RoleData _playDeck){
        List<CardData> _enemyCard = new List<CardData>();
        foreach (CardData item in _nowCard)
        {
            
            switch ((int)item.m_consumptionType)
            {
                case 0:
                    if(item.consume <= _playDeck.actionValue)
                        _enemyCard.Add(item);
                break;
                case 1:
                    if(item.consume <= _playDeck.magicValue)
                        _enemyCard.Add(item);
                break;

                default:
                break;
            }
        }
        return _enemyCard;
    }
}

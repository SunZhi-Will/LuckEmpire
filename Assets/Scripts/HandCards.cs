using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// 卡牌效果
/// </summary>
public class HandCards : MonoBehaviour
{
    /// <summary>
    /// 卡牌觸發類別
    /// </summary>
    public LayerMask playingFieldMask;
    /// <summary>
    /// 卡牌
    /// </summary>
    public GameObject card;
    
    /// <summary>
    /// 圖層位置
    /// </summary>
    public GameObject location;

    /// <summary>
    /// 卡牌群組
    /// </summary>
    public GameObject cardGroup;
    /// <summary>
    /// 抓住手牌
    /// </summary>
    public GameObject grabCard;

    /// <summary>
    /// 卡片寬度
    /// </summary>
    public float cardWidth = 150;

    /// <summary>
    /// 卡片間距
    /// </summary>
    public float maxSpacing = 10;

    /// <summary>
    /// 最大卡牌數(超過間距縮小)
    /// </summary>
    public float maxCard = 3;

    /// <summary>
    /// 使用卡片函式
    /// </summary>
    public UnityAction<CardData> OnUseCards;


    /// <summary>
    /// 手牌
    /// </summary>
    /// <typeparam name="GameObject">卡牌</typeparam>
    /// <returns></returns>
    private List<GameObject> ownCards = new List<GameObject>();

    /// <summary>
    /// 卡牌圖層
    /// </summary>
    /// <typeparam name="GameObject">圖層物件</typeparam>
    /// <returns></returns>
    private List<GameObject> cardLocation = new List<GameObject>();

    /// <summary>
    /// 卡片位置
    /// </summary>
    /// <typeparam name="Vector2">位置</typeparam>
    /// <returns></returns>
    private List<Vector2> cardPosition  = new List<Vector2>();

    /// <summary>
    /// 抓取卡牌號
    /// </summary>
    private int cardId = -1;

    /// <summary>
    /// 指向卡牌號
    /// </summary>
    private int pointCard = -1;

    private RectTransform grabCardRect;
    /// <summary>
    /// 觸碰卡牌
    /// </summary>
    private Transform triggerCard;

    private void Update()
    {
        for (int i = 0; i < ownCards.Count; i++)
        {
            RectTransform rt = ownCards[i].GetComponent<RectTransform>();
            if(cardId != i){
                rt.anchoredPosition3D = Vector3.Lerp(rt.anchoredPosition3D, cardPosition[i], 0.1f);
                
                if(pointCard != i){ //使否為指向卡
                    //當不等於指向卡時回復原狀
                    Transform _transform = ownCards[i].transform.GetChild(0);
                    _transform.localScale = Vector3.Lerp(_transform.localScale, new Vector2(1f, 1f), 0.1f);
                    _transform.position = Vector3.Lerp(_transform.position, rt.position, 0.1f);
                }else if(triggerCard){ //當沒有觸碰卡片時，不執行
                    Vector3 currentPosition = triggerCard.GetComponent<RectTransform>().anchoredPosition3D;
                    currentPosition = Vector3.Lerp(currentPosition, cardPosition[i], 0.1f);
                }
            }
            rt.localRotation = Quaternion.Euler(0f,0f,0f);
        }
    }
    
    /// <summary>
    /// 抽牌
    /// </summary>
    public CardUIEffect Extract(CardData cardData){

        GameObject _cardGo = Instantiate(card);
        GameObject _location = Instantiate(location);
        _location.transform.SetParent(cardGroup.transform);
        _location.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-5f);
        _cardGo.transform.SetParent(_location.transform);

        CardUIEffect _card = _cardGo.GetComponent<CardUIEffect>();
        

        _card.OnDragAction += OnCardDrag;
        _card.OnTapDownAction += OnCardDown;
        _card.OnTapReleaseAction += OnCardUp;
        _card.OnEnterAction += OnCardEnter;
        _card.OnExitAction += OnCardExit;
        _card.cardId = ownCards.Count;
        
        ownCards.Add(_cardGo);
        cardLocation.Add(_location);

        PositionAdjustment();

        
        _card.thisCardData = cardData;
        return _card;
    }
    
    /// <summary>
    /// 位置調整
    /// </summary>
    private void PositionAdjustment(){
        cardPosition  = new List<Vector2>();

        // 控制在一定寬度內
        float _width = cardWidth + maxSpacing;
        if(ownCards.Count > maxCard){
            _width = _width / (1f + (ownCards.Count - maxCard) / (maxCard - 1));
        }

        //是否為奇數
        float odd  = 1f;
        if(ownCards.Count % 2 == 1){
            odd = 0;
        }
        
        //最左邊的位置
        float x = -((_width * (int)(ownCards.Count / 2))) + _width / 2 * odd;
        //4為最左邊牌的旋轉角度，5為當滿五張牌時，最旁邊的牌角度為4
        //也就是改成6，滿六張牌旁邊才會為4
        float z = 4f / (maxCard - 1f) * (ownCards.Count - 1f);
        z = z > 4f? 4f: z;

        for (int i = 0; i < ownCards.Count; i++)
        {
            float z1 = ((float)ownCards.Count - 1f);
            z1 = z1 == 0 ? 1 : z1;
            cardLocation[i].transform.rotation = Quaternion.Euler(0f, 0f, z - (z * 2f) / z1  * i);

            cardPosition.Add(new Vector2(x + _width * i, transform.position.y));
            
        }
        
    }

    /// <summary>
    /// 卡牌拖拉
    /// </summary>
    /// <param name="cardId">卡片陣列序</param>
    private void OnCardDrag(int _cardId, Vector2 cardPos){
        Vector2 v2 = Camera.main.ScreenToViewportPoint( Input.mousePosition );
        Vector2 newPoint = new Vector2( v2.x - 0.5f, v2.y - 0.5f );
        Vector2 temp = new Vector2( newPoint.x * Screen.width, newPoint.y * Screen.height);

        cardId = _cardId;

        grabCardRect = ownCards[cardId].GetComponent<RectTransform>();
        grabCardRect.anchoredPosition = temp;
        grabCardRect.pivot = new Vector2(grabCardRect.pivot.x, 0.5f);
        ownCards[cardId].transform.localScale = new Vector2(0.5f, 0.5f);

        ownCards[cardId].GetComponent<Animator>().SetBool("Drag", true);

    }
    private void OnCardDown(int _cardId){
        cardId = _cardId;

        if(triggerCard != null){
            triggerCard.SetParent(ownCards[cardId].transform, true);
            triggerCard.localRotation = Quaternion.Euler(0f,0f,0f);
        }
        grabCardRect = ownCards[cardId].GetComponent<RectTransform>();
        ownCards[cardId].transform.SetParent(grabCard.transform, true);
        ownCards[cardId].transform.GetChild(0).transform.localScale = new Vector2(1.5f, 1.5f);
    }

    
    private void OnCardUp(int _cardId){
        cardId = _cardId;
        
        Ray ray = Camera.main.ScreenPointToRay(ownCards[_cardId].transform.position);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 10, playingFieldMask);
        
        if(hit.collider){
            ownCards[cardId].GetComponent<Animator>().SetTrigger("Destroy");

            GameObject go = ownCards[cardId];
            GameObject go2 = cardLocation[cardId];
            ownCards.Remove(go);
            
            cardLocation.Remove(go2);
            Destroy(go2);

            cardPosition.Remove(cardPosition[cardId]);

            for (int i = cardId; i < ownCards.Count; i++)
            {
                ownCards[i].GetComponent<CardUIEffect>().cardId -= 1;
            }

            PositionAdjustment(); //位置調整
            cardId = -1;

            OnUseCards(go.GetComponent<CardUIEffect>().thisCardData);
            Destroy(go.GetComponent<CardUIEffect>());
            Destroy(go, 1.2f);
        }else{
            ownCards[cardId].GetComponent<Animator>().SetBool("Drag", false);
            ownCards[cardId].transform.SetParent(cardLocation[cardId].transform);
            grabCardRect.pivot = new Vector2(grabCardRect.pivot.x, 0f);
            ownCards[cardId].transform.localScale = new Vector2(1f, 1f);
            cardId = -1;
        }
    }
    
    private void OnCardEnter(int _cardId){
        if(cardId == -1){
            pointCard = _cardId;
            triggerCard = ownCards[_cardId].transform.GetChild(0);
            triggerCard.SetParent(cardGroup.transform);
            triggerCard.localScale = new Vector2(1.5f, 1.5f);
            triggerCard.localRotation = Quaternion.Euler(0f,0f,0f);
        }
        
    }
    private void OnCardExit(int _cardId){
        if(cardId == -1){
            triggerCard.SetParent(ownCards[_cardId].transform);
            triggerCard.localRotation = Quaternion.Euler(0f,0f,0f);
        }
        pointCard = -1;
    }
    

}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace deck
{
    /// <summary>
    /// 플레이어 보유 장비아이템 하나의 정보를 보여주는 객체
    /// </summary>
    public class EquipItemListItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        /// <summary>
        /// 필요한 장비아이템
        /// </summary>
        public EquipItem item { get; private set; }
        /// <summary>
        /// 드래그 이벤트 처리를 위한 자기를 감싸는 아이템 슬롯 객체의 위치정보
        /// </summary>
        Transform itemInventorySlot;
        /// <summary>
        /// 드래그 이벤트 처리를 위한 캔버스
        /// </summary>
        Transform canvas;
        /// <summary>
        /// 이 UI의 위치 정보
        /// </summary>
        RectTransform rect;
        /// <summary>
        /// 이 UI의 캔버스 그룹
        /// </summary>
        CanvasGroup canvasGroup;
        /// <summary>
        /// 아이템이름 출력하는 텍스트 컴포넌트
        /// </summary>
        [SerializeField]
        TextMeshProUGUI itemName;
        /// <summary>
        /// 아이템 이미지 출력하는 이미지 컴포넌트
        /// </summary>
        [SerializeField]
        Image itemImage;
        /// <summary>
        /// 아이템 착용여부를 보여주는 이미지 컴포넌트
        /// </summary>
        [SerializeField]
        GameObject itemEquipMark;

        void Awake()
        {
            // canvas = FindObjectOfType<Canvas>().transform;
            rect = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        void Update()
        {
            // 아이템이 장착되었다면 장착마크를 표기함
            if (item.isEquip())
            {
                setEquipMark(true);
            }
            else
            {
                setEquipMark(false);
            }
        }

        public void setEquipMark(bool isEquip)
        {
            itemEquipMark.SetActive(isEquip);
        }

        /// <summary>
        /// 아이템 세부 정보창 열기
        /// </summary>
        public void openItemDetail()
        {
            // EquipItemManager.Instance().openItemDetail(item);
            MyDeckFactory.Instance().detailCanvasManager.openItemDetail(item);
        }

        /// <summary>
        /// UI 초기화메서드
        /// </summary>
        /// <param name="item">UI가 보여줄 아이템 정보</param>
        /// <param name="itemInventorySlot">자기자신을 감싸는 UI</param>
        /// <param name="canvas">드래그 처리를 위한 캔버스</param>
        public void Initialize(EquipItem item, Transform itemInventorySlot, Transform canvas)
        {
            this.item = item;
            this.itemInventorySlot = itemInventorySlot;
            this.canvas = canvas;
            itemName.text = this.item.getItemName();
            // itemImage.color = this.item.itemColor;
            itemImage.sprite = this.item.getItemIconImage(); 
        }

        /// <summary>
        /// 드래그 시작시 이벤트
        /// </summary>
        /// <param name="eventData"></param>
        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            // 드래그 시작시 UI를 캔버스의 마지막 자식으로 설정
            transform.SetParent(canvas);
            transform.SetAsLastSibling();

            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false; // ?
        }

        /// <summary>
        /// 드래그 진행 중 처리
        /// </summary>
        /// <param name="eventData"></param>
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            rect.anchorMin = new Vector2(0f, 0f);
            rect.anchorMax = new Vector2(0f, 0f);
            rect.anchoredPosition = Input.mousePosition;
        }

        /// <summary>
        /// 드래그 종료시 처리. 다시 원래 위치로 복귀
        /// </summary>
        /// <param name="eventData"></param>
        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            transform.SetParent(itemInventorySlot);
            rect.position = itemInventorySlot.position;

            canvasGroup.alpha = 1.0f;
            canvasGroup.blocksRaycasts = true; // ?
        }

    }

}

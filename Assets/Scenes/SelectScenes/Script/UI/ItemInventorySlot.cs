using deck;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace deck
{
    /// <summary>
    /// 캐릭터 아이템 정보보여주는 UI를 감싸는 컴포넌트
    /// </summary>
    /// <remarks>
    /// 드래그 이벤트 이후에도 아이템이 원래 위치로 돌아올 수 있는 용도로만 사용됨
    /// </remarks>
    /// <example>
    /// <code>
    ///     GameObject newPrefab = Instantiate(itemInventoryItemPrefeb, itemInventoryGrid);
    ///     newPrefab.GetComponent<ItemInventorySlot>().Initialize(item, canvas.transform);
    /// </code>
    /// </example>
    public class ItemInventorySlot : MonoBehaviour
    {
        /// <summary>
        /// 내가 감쌀 아이템 정보 UI
        /// </summary>
        [SerializeField]
        EquipItemListItem myEquipItemListItem;

        /// <summary>
        /// 자신이 감싸는 아이템 정보 UI에 대한 초기화를 위한 메서드 
        /// </summary>
        /// <param name="item">아이템 정보</param>
        /// <param name="canvas">드래그 이벤트 처리를 위한 canvas</param>
        public void Initialize(EquipItem item, Transform canvas)
        {
            myEquipItemListItem.Initialize(item, transform, canvas);
        }

        RectTransform myItem;
        RectTransform myRect;

        private void Start()
        {
            myRect = GetComponent<RectTransform>();
            myItem = myEquipItemListItem.gameObject.GetComponent<RectTransform>();

        }
        private void Update()
        {
            myItem.sizeDelta = myRect.sizeDelta;
        }
    }
}

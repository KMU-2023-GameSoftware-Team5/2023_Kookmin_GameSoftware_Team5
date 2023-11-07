using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck {
    public class CharacterDetailItemList : MonoBehaviour
    {
        /// <summary>
        /// 플레이어가 가지고 있는 모든 장비아이템의 배열
        /// </summary>
        List<EquipItem> items;
        List<GameObject> itemUIs;
        /// <summary>
        /// Drag 이벤트 처리를 위한 캔버스
        /// </summary>
        [SerializeField]
        GameObject canvas;
        /// <summary>
        /// 플레이어 보유 아이템이 보여질 위치
        /// </summary>
        [SerializeField]
        Transform itemInventoryGrid;

        /// <summary>
        /// 아이템 인벤토리 아이콘
        /// </summary>
        [SerializeField]
        GameObject itemInventoryItemPrefab;

        public void openItemList()
        {
            gameObject.SetActive(true);
            items = PlayerManager.Instance().playerEquipItems;
            if(itemUIs != null)
            {
                foreach(GameObject item in itemUIs)
                {
                    Destroy(item);
                }
            }
            itemUIs = new List<GameObject>();
            // 플레이어 보유 아이템에 대한 UI 생성
            foreach (EquipItem item in items)
            {
                GameObject go = Instantiate(itemInventoryItemPrefab, itemInventoryGrid);
                go.GetComponent<ItemInventorySlot>().Initialize(item, canvas.transform);
                itemUIs.Add(go);
            }
        }
        public void closeItemList()
        {
            gameObject.SetActive(false);
            if (itemUIs != null)
            {
                foreach (GameObject item in itemUIs)
                {
                    Destroy(item);
                }
            }

        }

    }
}

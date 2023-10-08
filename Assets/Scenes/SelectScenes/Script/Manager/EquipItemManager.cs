using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck
{
    /// <summary>
    /// 플레이어가 보유한 아이템에 대한 관리 객체
    /// </summary>
    public class EquipItemManager : MonoBehaviour
    {
        public static int MAX_INVENTORY_SIZE = 2;

        private static EquipItemManager instance;
        public static EquipItemManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<EquipItemManager>();
                }
                return instance;
            }
        }

        /// <summary>
        /// 플레이어가 가지고 있는 모든 장비아이템의 배열
        /// </summary>
        public List<EquipItem> items;

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
        GameObject itemInventoryItemPrefeb; 

        /// <summary>
        /// 아이템 세부 정보창
        /// </summary>
        [SerializeField]
        EquipItemDetails equipItemDetail;

        void Start()
        {
            // 플레이어 매니저에게서 아이템 보유목록 받기
            items = PlayerManager.Instance().playerEquipItems;
            
            if(items.Count == 0)
            {
                // 임시 데이터 생성
                PlayerManager.Instance().addEquipItemByName("sheild");
                PlayerManager.Instance().addEquipItemByName("sword");
                PlayerManager.Instance().addEquipItemByName("scroll");
                PlayerManager.Instance().addEquipItemByName("ring");
                PlayerManager.Instance().addEquipItemByName("wand");
                PlayerManager.Instance().addEquipItemByName("saber");
            }

            // 플레이어 보유 아이템에 대한 UI 생성
            foreach (EquipItem item in items)
            {
                createEquipItemInventoryPrefeb(item);
            }
        }

        /// <summary>
        /// 플레이어 보유 아이템에 대한 UI 생성
        /// </summary>
        /// <param name="item">아이템 정보</param>
        void createEquipItemInventoryPrefeb(EquipItem item)
        {
            GameObject newPrefab = Instantiate(itemInventoryItemPrefeb, itemInventoryGrid);
            newPrefab.GetComponent<ItemInventorySlot>().Initialize(item, canvas.transform);
        }

        /// <summary>
        /// 아이템에 대한 세부 정보창 열기
        /// </summary>
        /// <param name="item">정보를 열 아이템</param>
        public void openItemDetail(EquipItem item)
        {
            equipItemDetail.openItemDetail(item);
        }

    }

}

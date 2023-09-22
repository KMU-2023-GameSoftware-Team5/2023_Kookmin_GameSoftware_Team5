using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jslee
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
        public EquipItem[] items;

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
            // 임시 데이터 생성
            items = new EquipItem[6];
            items[0] = new EquipItem("blue", Color.blue);
            items[1] = new EquipItem("magenta", Color.magenta);
            items[2] = new EquipItem("cyan", Color.cyan);
            items[3] = new EquipItem("yellow", Color.yellow);
            items[4] = new EquipItem("red", Color.red);
            items[5] = new EquipItem("green", Color.green);

            // 플레이어 보유 아이템에 대한 UI 생성
            for (int i = 0; i < items.Length; i++)
            {
                createEquipItemInventoryPrefeb(items[i]); 
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
        /// 캐릭터에게 아이템 장착 이벤트
        /// </summary>
        /// <param name="character">아이템을 장착할 캐릭터</param>
        /// <param name="equipId">캐릭터가 몇번 인벤토리에 아이템을 장착할 것인지</param>
        /// <param name="item">장착할 아이템</param>
        public void equip(TmpCharacter character, int equipId, EquipItem item)
        {
            character.equip(equipId, item);
        }

        /// <summary>
        /// 캐릭터 아이템 장착 해제 이벤트
        /// </summary>
        /// <param name="character">아이템을 해제할 캐릭터</param>
        /// <param name="equipId">해제될 아이템의 인벤토리상 위치</param>
        public void unEquip(TmpCharacter character, int equipId)
        {
            character.unEquip(equipId);
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

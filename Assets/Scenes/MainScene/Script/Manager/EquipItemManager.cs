using jslee;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace jslee
{
    public class EquipItemManager : MonoBehaviour
    {
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

        /* 
            * 플레이어의 아이템 관리하는 객체  
            * TODO 
                * 씬 넘어가도 사용할 수 있게 하기 
                * Item 장착-해제 로직 개선하기
                * 장비 아이템 추가 제거 구현
                * 장비 아이템이 아닌 아이템 관리 구현
            * 속성
                * EquipItemGrid : 장비 아이템을 나열할 그리드
                * EquipItemInfoPrefab : 장비아이템 프리펩
                * equipItems : 플레이어가 소유하고 있는 장비 아이템
            * 메서드
                * openEquipItemForSelectEvent : 아이템 선택 가능하도록 처리
                * selectEquipItemEvent : 아이템이 선택된 이후 처리
         */

        public Transform EquipItemGrid;
        public GameObject EquipItemInfoPrefab;
        public EquipItem[] equipItems;

        // 장비아이템 착용처리를 위한 이벤트
        public UnityEvent openItemSelect;
        public UnityEvent closeItemSelect;

        void Start()
        {

            // 테스트를 위한 아이템 객체 생성 코드 
            equipItems = new EquipItem[10];
            for (int i = 0; i < equipItems.Length; i++)
            {
                equipItems[i] = new EquipItem($"Item Name {i}");
                createItemPrefeb(equipItems[i]);
            }
        }

        void createItemPrefeb(EquipItem item)
        {
            GameObject newPrefab = Instantiate(EquipItemInfoPrefab, EquipItemGrid);
            newPrefab.GetComponent<ItemInfoListItem>().setItemInfo(item);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void openEquipItemForSelectEvent()
        {
            // 장비 아이템 리스트로 이동(착용을 위해)
            Debug.Log("3. openSelectItem Event By ItemManger's open message");
            MainSceneEvent.Instance.onClickItem();
            openItemSelect.Invoke();
        }

        public void selectEquipItemEvent(EquipItem item)
        {
            // 장비 아이템이 선택 
            Debug.Log("5. openSelectItem Event By ItemManger's close Message");
            closeItemSelect.Invoke();
            MainSceneEvent.Instance.OnClickItemClose();
            CharacterSelectManager.Instance.closeSelectEquipItemEvent(item);
        }
    }

}
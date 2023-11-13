using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace deck
{
    /// <summary>
    /// 캐릭터 세부정보창에서 캐릭터 인벤토리 UI 관리하는 객체
    /// </summary>
    public class CharacterEquipItemSlot : MonoBehaviour, IDropHandler
    {
        /// <summary>
        /// 인벤토리가 비어있는 경우 보여줄 UI
        /// </summary>
        [SerializeField]
        GameObject emptySlot;
        /// <summary>
        /// 아이템 이미지 보여줄 컴포넌트
        /// </summary>
        [SerializeField]
        Image itemImage;
        /// <summary>
        /// 아이템 이름 보여줄 컴포넌트
        /// </summary>
        [SerializeField]
        TextMeshProUGUI itemName;

        /// <summary>
        /// 자신의 부모 캐릭터 디테일창
        /// </summary>
        CharacterDetails characterDetails;

        public PixelCharacter character { 
            get {
                return this.character;
            }
            set {
                this.character = value;
            }
        }

        /// <summary>
        /// 인벤토리상에서의 아이디
        /// </summary>
        int equipId;
        /// <summary>
        /// 인벤토리상 아이템이 있는 경우 그 아이템
        /// </summary>
        EquipItem equipItem;

        public void Initialize(int equipId, CharacterDetails characterDetails) {
            this.equipId = equipId;
            this.characterDetails = characterDetails;
        }

        /// <summary>
        /// 아이템 객체가 드래그 앤 드롭된 경우 아이템 착용 이벤트 처리
        /// </summary>
        /// <param name="eventData"></param>
        void IDropHandler.OnDrop(PointerEventData eventData)
        {
            
            if (eventData.pointerDrag != null)
            {
                EquipItemListItem newItem = eventData.pointerDrag.GetComponent<EquipItemListItem>(); ;

                if (newItem == null)
                {
                    return;
                }
                equip(newItem.item);
            }
        }

        /// <summary>
        /// 캐릭터 세부정보창 불러올 때 호출하여 초기설정할 때 사용되는 메서드
        /// </summary>
        /// <param name="item">장착할 아이템</param>
        public void setItem(EquipItem item)
        {
            equipItem = item;
        }

        /// <summary>
        /// 아이템 장비 착용 로직 호출
        /// </summary>
        /// <param name="newItem">새로 장착할 아이템</param>
        void equip(EquipItem newItem)
        {
            bool equip = characterDetails.equip(equipId, newItem);
            if (equip) // 장비 성공시에
            {
                equipItem = newItem;
            }
        }

        /// <summary>
        /// 아이템 장비 해제 로직 해제
        /// </summary>
        public void unEquip()
        {
            bool unequip = characterDetails.unEquip(equipId); 
            if (unequip)
            {
                equipItem = null;
            }
        }

        void Update()
        {
            if (equipItem == null) {
                emptySlot.SetActive(true);
            }
            else {
                emptySlot.SetActive(false);
                // itemImage.color = equipItem.itemColor;
                itemImage.sprite = equipItem.getItemIconImage();
                itemName.text = equipItem.getItemName();
            }
        }

        /// <summary>
        /// 아이템 세부 정보창 열기
        /// </summary>
        public void openItemDetail()
        {
            EquipItemManager.Instance().openItemDetail(equipItem);
        }
    }
}


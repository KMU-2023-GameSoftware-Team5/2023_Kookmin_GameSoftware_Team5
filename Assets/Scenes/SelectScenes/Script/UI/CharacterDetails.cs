using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace deck
{
    /// <summary>
    /// 캐릭터 세부 정보창 관리 컴포넌트
    /// </summary>
    public class CharacterDetails : MonoBehaviour
    {
        /// <summary>
        /// 캐릭터 세부 정보창 UI 
        /// </summary>
        [SerializeField]
        GameObject details;
        /// <summary>
        /// 세부정보창에서 보여질 캐릭터
        /// </summary>
        PixelCharacter character;

        /// <summary>
        /// 캐릭터 이미지를 보여질 컴포넌트
        /// </summary>
        [SerializeField]
        CharacterBuilderControl chracterImage;

        /// <summary>
        /// 캐릭터 이름보여줄 텍스트 컴포넌트
        /// </summary>
        [SerializeField]
        TextMeshProUGUI characterName;
        /// <summary>
        /// 캐릭터 세부 정보 보여줄 텍스트 컴포넌트
        /// </summary>
        [SerializeField]
        TextMeshProUGUI characterDescription;

        /// <summary>
        /// 캐릭터 인벤토리 정보 위치 컴포넌트
        /// </summary>
        [SerializeField]
        Transform characterEquipItemGird;
        /// <summary>
        /// 캐릭터 인벤토리 정보 프리펩
        /// </summary>
        [SerializeField]
        GameObject characterEquipItemSlotPrefeb; 
        /// <summary>
        /// 캐릭터 인벤토리 정보 슬롯
        /// </summary>
        CharacterEquipItemSlot[] charactrerEquipItemSlot;


        void Awake()
        {
            // 캐릭터 인벤토리 슬롯 생성
            charactrerEquipItemSlot = new CharacterEquipItemSlot[EquipItemManager.MAX_INVENTORY_SIZE];
            for(int i=0; i<charactrerEquipItemSlot.Length; i++)
            {
                charactrerEquipItemSlot[i] = createCharacterEquipItemSlot(i);
            }
        }

        /// <summary>
        /// 캐릭터 인벤토리 슬롯 정보 생성
        /// </summary>
        /// <param name="equipId">캐릭터 인벤토리에서 몇번 슬롯인지</param>
        /// <returns>생성된 캐릭터 인벤토리 슬롯 UI</returns>
        CharacterEquipItemSlot createCharacterEquipItemSlot(int equipId)
        {
            GameObject newPrefab = Instantiate(characterEquipItemSlotPrefeb, characterEquipItemGird);
            CharacterEquipItemSlot ret = newPrefab.GetComponent<CharacterEquipItemSlot>();
            ret.Initialize(equipId, this);
            return ret;
        }

        /// <summary>
        /// 캐릭터 세부 정보창을 여는 함수
        /// </summary>
        /// <remarks>외부에서 호출하는 함수</remarks>
        /// <param name="character">세부 정보창에서 보여질 캐릭터</param>
        public void openCharacterDetails(PixelCharacter character)
        {
            details.SetActive(true);
            this.character = character;
            characterName.text = this.character.getName();
            characterDescription.text = this.character.getDescribe();
            chracterImage.buildCharacter(this.character.characterName);
            for(int i = 0; i < this.character.Inventory.Length; i++)
            {
                charactrerEquipItemSlot[i].setItem(this.character.Inventory[i]);
            }
        }

        /// <summary>
        /// 아이템 착용 이벤트
        /// </summary>
        /// <remarks>아이템 매니저의 equip 메서드 호출</remarks>
        /// <param name="equipId">아이템 장착할 캐릭터의 인벤토리 슬롯</param>
        /// <param name="item">장착할 아이템</param>
        public bool equip(int equipId, EquipItem item)
        {
            return CharacterSelectManager.Instance.equip(character, equipId,item);
        }

        /// <summary>
        /// 아이템 해제 메서드
        /// </summary>
        /// <param name="equipId">아이템 해제할 인벤토리 슬롯</param>
        public bool unEquip(int equipId)
        {
            return CharacterSelectManager.Instance.unEquip(character, equipId);
        }

    }

}

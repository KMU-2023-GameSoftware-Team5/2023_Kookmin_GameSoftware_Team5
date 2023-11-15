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
        /// 세부정보창에서 보여질 캐릭터
        /// </summary>
        PixelCharacter character;
        
        [Header("characterIconLayer")]
        [SerializeField] CharacterIcon characterIcon;


        [Header("characterStatLayer")]
        /// <summary>
        /// 캐릭터 스텟을 보여줄 UI 컴포넌트
        /// </summary>
        [SerializeField] CharacterDetailsStatUI characterStats;
        /// <summary>
        /// 캐릭터 장착아이템 
        /// </summary>
        [SerializeField]
        Transform characterEquipItemGird;
        /// <summary>
        /// 캐릭터 인벤토리 정보 프리펩
        /// </summary>
        [SerializeField]
        GameObject characterEquipItemSlotPrefab;
        /// <summary>
        /// 캐릭터 인벤토리 정보 슬롯
        /// </summary>
        CharacterEquipItemSlot[] charactrerEquipItemSlot;



        [Header("CharacterDescriptionLayer")]
        /// <summary>
        /// 캐릭터 세부 정보 보여줄 텍스트 컴포넌트
        /// </summary>
        [SerializeField] TextMeshProUGUI characterDescription;
        [SerializeField] GameObject itemTab;

        [SerializeField] GameObject[] star;

        void Awake()
        {
            // 캐릭터 인벤토리 슬롯 생성
            charactrerEquipItemSlot = new CharacterEquipItemSlot[PlayerManager.MAX_INVENTORY_SIZE];
            for(int i=0; i<charactrerEquipItemSlot.Length; i++)
            {
                charactrerEquipItemSlot[i] = createCharacterEquipItemSlot(i);
            }
            if(nickNameInput != null)
            {
                nickNameInput.onEndEdit.AddListener(onNickNameChange);
            }
        }

        /// <summary>
        /// 캐릭터 인벤토리 슬롯 정보 생성
        /// </summary>
        /// <param name="equipId">캐릭터 인벤토리에서 몇번 슬롯인지</param>
        /// <returns>생성된 캐릭터 인벤토리 슬롯 UI</returns>
        CharacterEquipItemSlot createCharacterEquipItemSlot(int equipId)
        {
            GameObject newPrefab = Instantiate(characterEquipItemSlotPrefab, characterEquipItemGird);
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
            characterIcon.Initialize(character);
            this.character = character;
            characterDescription.text = this.character.getDescribe();
            characterStats.setCharacter(character);
            for(int i = 0; i < this.character.Inventory.Length; i++)
            {
                charactrerEquipItemSlot[i].setItem(this.character.Inventory[i]);
            }
            itemTab.SetActive(character.playerOwned);

            foreach(GameObject go in star)
            {
                go.SetActive(false);
            }
            star[character.tier - 1].SetActive(true);
            nickNameInput.readOnly = !character.playerOwned;
        }

        /// <summary>
        /// 아이템 착용 이벤트
        /// </summary>
        /// <remarks>아이템 매니저의 equip 메서드 호출</remarks>
        /// <param name="equipId">아이템 장착할 캐릭터의 인벤토리 슬롯</param>
        /// <param name="item">장착할 아이템</param>
        public bool equip(int equipId, EquipItem item)
        {
            bool equipSuccess = PlayerManager.Instance().equip(character, equipId,item);
            characterStats.updateStat();
            return equipSuccess;
        }

        /// <summary>
        /// 아이템 해제 메서드
        /// </summary>
        /// <param name="equipId">아이템 해제할 인벤토리 슬롯</param>
        public bool unEquip(int equipId)
        {
            bool unEquipSuccess = PlayerManager.Instance().unEquip(character, equipId);
            characterStats.updateStat();
            return unEquipSuccess;
        }

        [SerializeField] TMP_InputField nickNameInput;
        public void onNickNameChange(string value)
        {
            if(value.Length <= 0)
            {
                characterIcon.characterNickNameText.text = character.characterNickName;
                return;
            }
            character.characterNickName = value;
            MyDeckFactory.Instance().nickNameChangeEvent.Invoke(character.ID);
        }
    }

}

using jslee;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace jslee
{
    public class CharacterSelector : MonoBehaviour
    {
        /*
         * 현재 선택된 캐릭터의 정보를 보여주는 컴포넌트
         * 속성
            * selectId : 캐릭터 선택창에서 몇번인지 
            * CharacterInfo : 현재 선택된 캐릭터창에서 캐릭터가 선택되었을 경우 캐릭터 정보를 보여줌
            * tmpCharacter : 현재 선택된 캐릭터 정보
            * CharacaterItemSelect[] items : 현 캐릭터 장착 아이템 정보 UI 컴포넌트
            * itemSelectorList
            * itemSelectPrefab
         * 메서드 
            * chooseCharacter : 캐릭터 선택기능작동
            * setCharInfo : 선택된 캐릭터에 대한 정보를 받는 setter 함수
            * setItem : 현 캐릭터의 item 정보를 하위 컴포넌트에게 제공
            * equip : 캐릭터의 아이템 착용 처리
         */
        public int selectId;
        public GameObject CharacterInfoUI;
        public GameObject chooseCharcterUI;
        public TmpCharacter tmpCharacter;
        public TextMeshProUGUI CharacterDescription;

        // 아이템 관련 속성
        public CharacaterItemSelect[] items;
        public Transform itemSelectorList;
        public GameObject itemSelectPrefab;


        void Start()
        {
            for(int i=0; i< items.Length; i++)
            {
                items[i] = createEquipItemPrefeb(i);
            }
            if(tmpCharacter != null) {
                setItem();
            }
        }

        public void setCharInfo(TmpCharacter character)
        {
            tmpCharacter = character;
        }

        // Update is called once per frame
        void Update()
        {
            if (tmpCharacter == null)
            {
                chooseCharcterUI.SetActive(true);
                CharacterInfoUI.SetActive(false);
            }
            else
            {
                chooseCharcterUI.SetActive(false);
                CharacterInfoUI.SetActive(true);
                CharacterDescription.text = tmpCharacter.ToString();
                setItem();
            }
        }

        public void chooseCharacter()
        {
            CharacterSelectManager.Instance.openCharacterList(selectId);
        }

        // 아이템 관련 메서드 
        CharacaterItemSelect createEquipItemPrefeb(int i)
        {
            GameObject newPrefab = Instantiate(itemSelectPrefab, itemSelectorList);
            CharacaterItemSelect itemComp = newPrefab.GetComponent<CharacaterItemSelect>();
            itemComp.setSelectId(selectId, i);
            return itemComp;
        }

        public void setItem()
        {
            for(int i=0; i < items.Length; i++)
            {
                items[i].setItemInfo(tmpCharacter.Inventory[i]);
            }
        }

        public void equip(int itemId, EquipItem item)
        {
            tmpCharacter.equip(itemId, item);
            // items[itemId].setItemInfo(item);
        }
    }

}

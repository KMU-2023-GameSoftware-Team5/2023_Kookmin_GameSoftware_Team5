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
         * ���� ���õ� ĳ������ ������ �����ִ� ������Ʈ
         * �Ӽ�
            * selectId : ĳ���� ����â���� ������� 
            * CharacterInfo : ���� ���õ� ĳ����â���� ĳ���Ͱ� ���õǾ��� ��� ĳ���� ������ ������
            * tmpCharacter : ���� ���õ� ĳ���� ����
            * CharacaterItemSelect[] items : �� ĳ���� ���� ������ ���� UI ������Ʈ
            * itemSelectorList
            * itemSelectPrefab
         * �޼��� 
            * chooseCharacter : ĳ���� ���ñ���۵�
            * setCharInfo : ���õ� ĳ���Ϳ� ���� ������ �޴� setter �Լ�
            * setItem : �� ĳ������ item ������ ���� ������Ʈ���� ����
            * equip : ĳ������ ������ ���� ó��
         */
        public int selectId;
        public GameObject CharacterInfoUI;
        public GameObject chooseCharcterUI;
        public TmpCharacter tmpCharacter;
        public TextMeshProUGUI CharacterDescription;

        // ������ ���� �Ӽ�
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

        // ������ ���� �޼��� 
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

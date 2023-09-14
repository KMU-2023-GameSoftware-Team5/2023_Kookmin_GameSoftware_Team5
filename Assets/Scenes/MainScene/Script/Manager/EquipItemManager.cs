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
            * �÷��̾��� ������ �����ϴ� ��ü  
            * TODO 
                * �� �Ѿ�� ����� �� �ְ� �ϱ� 
                * Item ����-���� ���� �����ϱ�
                * ��� ������ �߰� ���� ����
                * ��� �������� �ƴ� ������ ���� ����
            * �Ӽ�
                * EquipItemGrid : ��� �������� ������ �׸���
                * EquipItemInfoPrefab : �������� ������
                * equipItems : �÷��̾ �����ϰ� �ִ� ��� ������
            * �޼���
                * openEquipItemForSelectEvent : ������ ���� �����ϵ��� ó��
                * selectEquipItemEvent : �������� ���õ� ���� ó��
         */

        public Transform EquipItemGrid;
        public GameObject EquipItemInfoPrefab;
        public EquipItem[] equipItems;

        // �������� ����ó���� ���� �̺�Ʈ
        public UnityEvent openItemSelect;
        public UnityEvent closeItemSelect;

        void Start()
        {

            // �׽�Ʈ�� ���� ������ ��ü ���� �ڵ� 
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
            // ��� ������ ����Ʈ�� �̵�(������ ����)
            Debug.Log("3. openSelectItem Event By ItemManger's open message");
            MainSceneEvent.Instance.onClickItem();
            openItemSelect.Invoke();
        }

        public void selectEquipItemEvent(EquipItem item)
        {
            // ��� �������� ���� 
            Debug.Log("5. openSelectItem Event By ItemManger's close Message");
            closeItemSelect.Invoke();
            MainSceneEvent.Instance.OnClickItemClose();
            CharacterSelectManager.Instance.closeSelectEquipItemEvent(item);
        }
    }

}
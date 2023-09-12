using jslee;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    /* TODO 
        * �̱������� ����� 
        * �� �Ѿ�� ����� �� �ְ� �ϱ� 
        * Item �߰� �� ���� �����ϱ� 
        * ĳ���Ϳ� ������ �߰� ���� �����ϱ�
     */

    public Transform itemGrid; 
    public GameObject itemInfoPrefab; 

    public Item[] items;

    // Start is called before the first frame update
    void Start()
    {
        // �׽�Ʈ�� ���� ������ ��ü ���� �ڵ� 
        items = new Item[10];
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = new Item($"Item Name {i}");
            createItemPrefeb(items[i]);
        }
    }

    void createItemPrefeb(Item item)
    {
        GameObject newPrefab = Instantiate(itemInfoPrefab, itemGrid);
        newPrefab.GetComponent<ItemInfoListItem>().setItemInfo(item);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

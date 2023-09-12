using jslee;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Transform itemGrid; 
    public GameObject itemInfoPrefab; 

    public Item[] items;

    // Start is called before the first frame update
    void Start()
    {
        // 테스트를 위한 아이템 객체 생성 코드 
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
        newPrefab.GetComponent<ItemInfoChange>().setItemInfo(item);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

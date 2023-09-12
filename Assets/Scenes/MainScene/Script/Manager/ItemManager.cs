using jslee;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    /* TODO 
        * 싱글톤으로 만들기 
        * 씬 넘어가도 사용할 수 있게 하기 
        * Item 추가 및 제거 구현하기 
        * 캐릭터에 아이템 추가 제거 구현하기
     */

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
        newPrefab.GetComponent<ItemInfoListItem>().setItemInfo(item);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

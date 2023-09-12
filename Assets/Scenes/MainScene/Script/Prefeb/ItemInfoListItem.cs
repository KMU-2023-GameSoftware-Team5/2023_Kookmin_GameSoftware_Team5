using jslee;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoListItem : MonoBehaviour
{
    //  tmp item info 프리펩을 관리하는 객체
    // TODO
      // 아이템을 선택할 수 있게 하여 캐릭터에게 장착하도록 하기 

    public Item itemInfo;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;
    // Start is called before the first frame update
    private void Start()
    {

    }

    public void setItemInfo(Item item)
    {
        itemInfo = item;
    }

    // Update is called once per frame
    void Update()
    { 
        itemName.text = itemInfo.getItemName();
        itemDescription.text = itemInfo.ToString();
    }
}

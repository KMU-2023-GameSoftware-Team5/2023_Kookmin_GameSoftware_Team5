using jslee;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoListItem : MonoBehaviour
{
    //  tmp item info �������� �����ϴ� ��ü
    // TODO
      // �������� ������ �� �ְ� �Ͽ� ĳ���Ϳ��� �����ϵ��� �ϱ� 

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

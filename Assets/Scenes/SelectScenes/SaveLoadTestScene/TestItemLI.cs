using deck;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace testSL
{
    public class TestItemLI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI itemName;

        public void Initialize(EquipItem item)
        {
            itemName.text = item.getItemName();
        }

        public void destroy()
        {
            Destroy(gameObject);
        }

    }

}

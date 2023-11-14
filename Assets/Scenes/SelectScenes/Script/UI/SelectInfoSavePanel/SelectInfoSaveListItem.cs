using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace deck
{
    public class SelectInfoSaveListItem : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI presetName;
        [SerializeField] Image bg;  
        SelectInfoSaveList parent;
        int idx;

        public void Initlaize(SelectInfoSaveList parent, int idx)
        {
            this.parent = parent;
            this.idx = idx;
            this.presetName.text = $"Preset{idx}";

        }

        public void onClickDelete()
        {
            PlayerManager.Instance().selectedCharacters.RemoveAt(idx);
            parent.Initialize();
        }

        public void onClickSelect(bool isSelect)
        {
            if (isSelect)
            {
                bg.color = Color.gray;
                parent.loadIdx = idx;
            }
            else
            {
                bg.color = Color.black;
            }
        }
    }

}

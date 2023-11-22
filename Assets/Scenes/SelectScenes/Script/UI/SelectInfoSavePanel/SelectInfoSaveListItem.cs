using Newtonsoft.Json.Linq;
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
        [SerializeField] Transform presetList;
        int idx;

        public void Initlaize(SelectInfoSaveList parent, int idx)
        {
            this.parent = parent;
            this.idx = idx;
            this.presetName.text = $"{idx}";
            //this.presetName.text = $"Preset{idx}";
            loadPresetInfo();
        }

        public void loadPresetInfo()
        {
            JArray presetInfo = (JArray)PlayerManager.Instance().selectedCharacters[idx];
            List<PixelCharacter> characters = PlayerManager.Instance().playerCharacters;

            for (int i = presetInfo.Count - 1; i >= 0; i--) // 배치된 캐릭터 정보
            {
                PixelCharacter tmp = null;
                foreach (PixelCharacter character in characters) // 현재 UI를 다 돈다
                {
                    if (character.ID == presetInfo[i]["id"].ToString()) // 두 캐릭터 객체의 ID가 같으면 작동
                    {
                        tmp = character;
                        break;
                    }
                }
                if (tmp != null)
                {
                    parent.createPresetCharacterInfo(tmp, presetList);
                }
                else
                {
                    // 캐릭터가 삭제되었다면 제거
                    presetInfo.RemoveAt(i);
                }
            }
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

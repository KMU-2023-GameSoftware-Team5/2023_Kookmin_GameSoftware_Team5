using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck
{
    public class SelectInfoSaveList : MonoBehaviour
    {
        [SerializeField]GameObject listGameObject;
        [SerializeField]GameObject selectInfoSaveListPrefab;
        List<SelectInfoSaveListItem> childs;

        int _loadIdx;
        public int loadIdx
        {
            get
            {
                return _loadIdx;
            }
            set
            {
                if(_loadIdx != 0 && _loadIdx != value)
                    childs[_loadIdx-1].onClickSelect(false);
                _loadIdx = value;
            }
        }

        public void listSetActive(bool active=true)
        {
            listGameObject.SetActive(active);
            if (active)
            {
                Initialize();
            }
        }
        
        public void Initialize()
        {
            _loadIdx = 0;
            if(childs != null) { 
                foreach(SelectInfoSaveListItem item in childs)
                {
                    Destroy(item.gameObject);
                }
            }
            childs = new List<SelectInfoSaveListItem>();
            JArray target = PlayerManager.Instance().selectedCharacters;
            for(int i = 1; i < target.Count; i++) {
                GameObject go = Instantiate(selectInfoSaveListPrefab, transform);
                SelectInfoSaveListItem tmp = go.GetComponent<SelectInfoSaveListItem>();
                tmp.Initlaize(this, i);
                childs.Add(tmp);
            }
        }

        public void onClickSaveSelectInfo()
        {
            JArray target = (JArray) CharacterSelectManager.Instance().saveSelectedCharacterInfo().DeepClone();
            PlayerManager.Instance().selectedCharacters.Add(target);
            PlayerManager.save();
            Initialize();
        }

        public void onClickLoadSelectInfo()
        {
            if (_loadIdx == 0) return;  
            CharacterSelectManager.Instance().initializePlacement();
            JArray target = (JArray)PlayerManager.Instance().selectedCharacters[_loadIdx];
            CharacterSelectManager.Instance().loadSelectedCharacterInfo(target);
            listSetActive(false);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck
{
    public class NewGameSelectorList : MonoBehaviour
    {

        [SerializeField] Transform targetGrid;
        [SerializeField] GameObject ngSelectorItemPrefab;
        List<NewGameSelectorItem> ngsItems;
        NewGameSelectorItem nowSelectCharcterList;
        NewGameSceneManager sceneManager;

        public void Initialize(NewGameSceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
        }

        public void openNGSelector()
        {
            gameObject.SetActive(true);
            List<List<PixelCharacter>>  selectTemplate = MyDeckFactory.Instance().getNGSelectos();
            ngsItems = new List<NewGameSelectorItem>();
            nowSelectCharcterList = null;
            foreach (List<PixelCharacter> clist in selectTemplate)
            {
                createNGSitem(clist);
            }
        }

        public void createNGSitem(List<PixelCharacter> clist)
        {
            GameObject go = Instantiate(ngSelectorItemPrefab, targetGrid);
            NewGameSelectorItem item = go.GetComponent<NewGameSelectorItem>();
            item.Initialize(clist, this);
            ngsItems.Add(item);
        }

        public void onSelect(NewGameSelectorItem nowSelectCharcterList)
        {
            if(this.nowSelectCharcterList != null) {
                this.nowSelectCharcterList.unSelect();
            }
            this.nowSelectCharcterList = nowSelectCharcterList;
        }

        public void unSelect()
        {
            nowSelectCharcterList = null;
        }

        public void onClickNewGame()
        {
            if(nowSelectCharcterList == null)
            {
                MyDeckFactory.Instance().displayInfoMessage("하나 선택해주십시오");
                return;
            }
            List<PixelCharacter> clist = nowSelectCharcterList.clist;
            sceneManager.newGameStart(clist);
        }

        /// <summary>
        /// 리셋
        /// </summary>
        public void onCancel()
        {
            gameObject.SetActive(false);
            foreach(NewGameSelectorItem item in ngsItems)
            {
                Destroy(item.gameObject);
            }
        }

        
    }
}

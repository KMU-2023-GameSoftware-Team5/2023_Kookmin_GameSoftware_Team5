using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace deck
{
    public class NewGameSelectorItem : MonoBehaviour
    {
        [SerializeField] Image bg; 
        [SerializeField] Color normalColor;
        [SerializeField] Color selectColor;
        NewGameSelectorList parent;
        [SerializeField] Transform targetGrid;
        bool isSelect;

        public List<PixelCharacter> clist { get; private set; }

        public void Initialize(List<PixelCharacter> clist, NewGameSelectorList parent)
        {
            isSelect = false;
            this.parent = parent;
            this.clist = clist;
            foreach (PixelCharacter c in clist)
            {
                MyDeckFactory.Instance().createCharacterInventoryPrefab(c, targetGrid, sortingOrder:3);
                // Light characterLI 생성
            }
        }

        public void onClickNGSitem()
        {
            if(!isSelect) {
                parent.onSelect(this);
                bg.color = selectColor;
                isSelect = true;
            }
            else
            {
                _unSelect();
            }
        }

        public void unSelect()
        {
            bg.color = normalColor;
            isSelect = false;
        }

        void _unSelect()
        {
            unSelect();
            parent.unSelect();
        }
    }

}

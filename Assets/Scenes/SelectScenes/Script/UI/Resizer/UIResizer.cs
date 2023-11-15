using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck
{
    public class UIResizer : MonoBehaviour
    {
        [SerializeField]RectTransform rect;
        int resizeCount = 0;

        void Start()
        {
            if (rect == null)
            {
                return;
            }
            rect.sizeDelta = new Vector2(rect.rect.height, 0);
        }

        // Update is called once per frame
        void Update()
        {
            if(resizeCount <= 100)
            {
                if (rect == null)
                {
                    return;
                }
                rect.sizeDelta = new Vector2(rect.rect.height, 0);
                resizeCount++;
            }

        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck
{
    public class UIResizer : MonoBehaviour
    {
        [SerializeField]RectTransform rect;

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

        }
    }

}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace deck
{
    /// <summary>
    /// 부모의 rectTransform을 받아서 resizer
    /// </summary>
    public class ParentResizer : MonoBehaviour
    {
        RectTransform parent;
        public float sizefactor = 0.9f;
        [SerializeField] RectTransform myRect;
        public bool isHeight = true;

        public void initialize(RectTransform parent)
        {
            this.parent = parent;
            float size;
            if (isHeight)
            {
                size = parent.rect.height;
            }
            else
            {
                size = parent.rect.width;
            }
            size *= sizefactor;
            myRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
            myRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
        }

        // Start is called before the first frame update
        void Start()
        {

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck
{
    public class ImageResizer : MonoBehaviour
    {
        public RectTransform parentRect;
        public RectTransform imageRect;
        public float sizefactor = 0.9f;
        public bool useHeight = true;

        void Start()
        {
            float size;
            if (useHeight)
            {
                size = parentRect.rect.height;
            }
            else
            {
                size = parentRect.rect.width;
            }
            size *= sizefactor;
            imageRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
            imageRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}

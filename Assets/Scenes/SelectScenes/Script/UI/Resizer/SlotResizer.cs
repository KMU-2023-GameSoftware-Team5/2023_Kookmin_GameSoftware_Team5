using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck
{
    public class SlotResizer : MonoBehaviour
    {
        [SerializeField] RectTransform parentRect;
        [SerializeField] RectTransform myRect;
        public bool useHeight=true;
        public float sizeFactor = 0.84f;

        private void Start()
        {
            RectTransform rect = parentRect.transform.parent.gameObject.GetComponent<RectTransform>();
            float size;
            if(useHeight )
            {
                size = rect.rect.height;
            }
            else
            {
                size = rect.rect.width;
            }
            size *= sizeFactor;
            parentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
            parentRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
            myRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
            myRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);

        }
    }

}

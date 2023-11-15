using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck
{
    public class SlotResizer : MonoBehaviour
    {
        [SerializeField] RectTransform parentRect;
        [SerializeField] RectTransform myRect;


        private void Update()
        {
            float parentWidth = parentRect.rect.width;
            float parentHeight = parentRect.rect.height;
            float minRect = parentWidth < parentHeight ? parentWidth : parentHeight;
            myRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, minRect);
            myRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, minRect);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck
{
    public class CloseResizer : MonoBehaviour
    {
        [SerializeField] RectTransform parentRect;
        [SerializeField] RectTransform myRect;
        public bool useHeight = true;

        private void Start()
        {
            float size = parentRect.rect.height;
            myRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
            myRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);

        }
    }

}

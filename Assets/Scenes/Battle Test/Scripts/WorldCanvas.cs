using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lee
{
    public class WorldCanvas : StaticGetter<WorldCanvas> 
    {
        public RectTransform rect;

        private void Start()
        {
            rect.position = Vector3.zero;
        }
    }
}

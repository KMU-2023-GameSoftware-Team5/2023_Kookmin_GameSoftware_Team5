using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace battle
{
    public class WorldCanvas : StaticComponentGetter<WorldCanvas> 
    {
        public RectTransform rect;

        private void Start()
        {
            rect.position = Vector3.zero;
        }
    }
}

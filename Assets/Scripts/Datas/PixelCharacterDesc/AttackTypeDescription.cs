using Castle.Core;
using data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace deck
{
    [CreateAssetMenu(fileName = "attackTypeDescription", menuName = "shop/attackTypeDescription", order = 3)]
    public class AttackTypeDescription : ScriptableObject
    {
        public EDefualtAttackType type;
        public string desc;
        public Sprite img;
    }

}

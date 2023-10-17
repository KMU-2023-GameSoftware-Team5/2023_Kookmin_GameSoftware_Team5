using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

namespace battle
{
    public struct Vector3Bool
    {
        public Vector3Bool(bool x, bool y, bool z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3Bool(bool value) : this(value, value, value) { }

        public static Vector3Bool True = new Vector3Bool(true);
        public static Vector3Bool False = new Vector3Bool(false);

        public bool x;
        public bool y;
        public bool z;
    }

    public static class Utility
    {
        public enum Direction2 { Left, Right }
        public enum Direction4 { Left, Right, Up, Down }

        /// <summary>
        /// root가 취해지지 않은 거리를 리턴한다. 
        /// </summary>
        /// <returns></returns>
        public static float GetSquaredDistanceBetween(Transform transform1, Transform transform2)
        {
            Vector3 delta = transform1.position - transform2.position;
            return delta.x * delta.x + delta.y * delta.y + delta.z * delta.z;
        }

        public static float GetDistanceBetween(Transform transform1, Transform transform2)
        {
            float distance = GetSquaredDistanceBetween (transform1, transform2);
            return Mathf.Sqrt(distance);
        }

        public static float GetDistanceBetween(Vector3 position1, Vector3 position2)
        {
            Vector3 delta = position1 - position2;
            return delta.sqrMagnitude;
        }
    }
}
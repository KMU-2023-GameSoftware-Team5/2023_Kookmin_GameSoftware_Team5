using battle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MobsSet", menuName = "data/MobSet", order = 1)]
public class MobSetData : ScriptableObject
{
    [Serializable]
    public struct Mob
    {
        public string CharacterName;
        public Vector2 Position;
    }

    public Mob[] mobs;
}

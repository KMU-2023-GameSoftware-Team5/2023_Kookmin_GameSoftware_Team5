using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound Data", menuName = "data/Sound", order = 1)]
public class SoundData : ScriptableObject
{
    public AudioClip meleeAttack;
    public AudioClip rangedAttack;

    public AudioClip hit;
}

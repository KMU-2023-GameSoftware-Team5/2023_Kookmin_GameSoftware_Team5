using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using battle;

public class CustomSkillData : ScriptableObject
{
    [Header("CustomSkillData")]
    public string skillName;
    public string skillDescription;
    public Sprite skillIcon;

    public virtual PixelHumanoid.State CreateSkillState() { return null; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using battle;

public class CustomSkillData : ScriptableObject
{
    public string skillName;
    public string skillDescription;

    public virtual PixelHumanoid.State CreateSkillState() { return null; }
}

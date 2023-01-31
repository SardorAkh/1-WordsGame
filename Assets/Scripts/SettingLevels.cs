using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SettingLevels",menuName = "Settings/SettingLevels",order =0)]
public class SettingLevels : ScriptableObject
{
    public List<Level> levels;
}

[System.Serializable]
public struct Level
{
    public string Word;
    public Sprite sprite;
}
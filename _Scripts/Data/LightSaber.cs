using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LightSaberSO", menuName = "DataSO/LightSaberSO")]
public class LightSaberSO : ScriptableObject
{
    public LightSaberData[] lightSaberData;
}

[Serializable]
public class LightSaberData : DataBase
{
    public GameObject prefab;
    public Color      defaultColor;
}
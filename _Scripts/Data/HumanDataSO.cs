using System;
using UnityEngine;

[CreateAssetMenu(fileName = "HumanDataSO", menuName = "DataSO/HumanDataSO")]
public class HumanDataSO : ScriptableObject
{
    public HumanData[] humanData;
}

[Serializable]
public class HumanData : DataBase
{
    public Sprite imageCollection;
}
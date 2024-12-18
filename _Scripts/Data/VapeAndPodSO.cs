using System;
using UnityEngine;

[CreateAssetMenu(fileName = "VapeAndPodSO", menuName = "DataSO/VapeAndPodSO")]
public class VapeAndPodSO : ScriptableObject
{
    public VapeAndPodData[] vapeAndPodData;
    public DripTip[]        dripTips;
    public Tank[]           tanks;
    public Juice[]          juices;

    [Header("--")]
    public Color juiceDefault;
}

[Serializable]
public class VapeAndPodData : DataBase
{
    public GameObject prefab;
    public int        defaultTank;
}

//Đầu đốt tinh dầu
[Serializable]
public struct DripTip
{
    public GameObject prefab;
}

//Buồng chứa tinh dầu
[Serializable]
public class Tank : DataBase
{
    public GameObject prefab;
}

//Tinh dầu
[Serializable]
public class Juice : DataBase
{
    public Color color;
}